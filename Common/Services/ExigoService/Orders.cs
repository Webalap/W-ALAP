using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using AutoMapper;
using Common;
using Common.Api.ExigoOData;
using Common.Api.ExigoWebService;
namespace ExigoService
{
    public static partial class Exigo
    {
        public static IEnumerable<Order> GetEventOrders(int eventId)
        {
            var orders = new List<Order>();
            if (eventId == 0)
            {
                throw new ArgumentException("Event ID is required.");
            }


            using (var Context = Exigo.Sql())
            {
                //Getting order list for selected event
                string sqlProcedure = string.Format("GetEventsOrders {0}", eventId);
                orders = Context.Query<Order>(sqlProcedure).ToList();
            }

            return orders.ToList().Select(s =>
            {
                var order = s;
                order.Customer = GetCustomer(s.CustomerID);
                return order;
            }).ToList();
        }

        public static Order GetCustomerOrdersDetail(int orderId)
        {
            using (var contextsql = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetOrdersDetail {0}", orderId);
                return contextsql.Query<Order>(sql).FirstOrDefault();
            }
        }

        public static IEnumerable<Order> GetCustomerOrders(GetCustomerOrdersRequest request)
        {

            var orders = new List<Order>();
            var orderStatus = "";
            var orderType = "";
            var orderid = 0;

            if (request.CustomerID == 0)
            {
                throw new ArgumentException("CustomerID is required.");
            }
            // Apply the request variables
            if (request.OrderStatuses.Length > 0)
            {

                orderStatus = string.Join(",", request.OrderStatuses.ToList());
            }
            if (request.OrderTypes.Length > 0)
            {
                orderType = string.Join(",", request.OrderTypes.ToList());
            }
            if (request.OrderID != null)
            {
                orderid = (int)request.OrderID;
            }
            
            using (var contextsql = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetCustomerOrders {0}, {1}, {2}, {3}, '{4}', '{5}', '{6}'", 
                    request.CustomerID, orderid, request.Page, request.Take, orderStatus, orderType, request.StartDate);
                orders = contextsql.Query<Order>(sql).ToList();
            }
            // If we don't have any orders, stop here.
            if (orders.Count == 0) yield break;

            orders = orders.ToList().Select(s =>
            {
                var order = s;
                order.Customer = GetCustomer(s.CustomerID);
                return order;
            }).ToList();


            // Setup the base orders query
            if (request.IncludePayments)
            {
                orders = orders.ToList().Select(s =>
                {
                    var order = s;
                    order.Payments = GetOrderPayments(s.OrderID).ToList();
                    return order;
                }).ToList();
            }

            // Get the order details if applicable
            if (request.IncludeOrderDetails)
            {
                orders = orders.ToList().Select(s =>
                {
                    var order = s;
                    order.Details = GetOrderDetails(s.OrderID).ToList();
                    return order;
                }).ToList();

                //var itemIDs = orders.ToList().Select(.Select(c => c.ItemID).Distinct().ToList();

                // Get a unique list of item IDs in the orders
                var itemIDs = orders.ToList().Select(o =>
                
                    o.Details.Select(od =>
                    {
                        return od.ItemCode;
                    }).Distinct()
                ).ToList();

                var apiItems = new List<Item>();
                // Get the extra data we need for each detail
                if (itemIDs.Count > 0)
                {
                    foreach (var itemID in itemIDs)
                    {
                        foreach (var id in itemID)
                        {
                            apiItems.Add(GetItem(id));
                        }
                    }
                    
                }

                // Format the data to our models
                foreach (var order in orders)
                {
                    // Get the order details
                    var details = order.Details;
                    foreach (var detail in details)
                    {
                        var apiItem = apiItems.Where(c => c.ItemCode == detail.ItemCode).FirstOrDefault();
                        if (apiItem != null)
                        {
                            detail.ImageUrl = apiItem.SmallImageUrl;
                            detail.IsVirtual = apiItem.IsVirtual;
                        }
                    }
                    order.Details = details;
                }
            }


            // Format the data to our models
            foreach (var order in orders)
            {
                yield return order;
            }
        }

        private static List<Payment> GetOrderPayments(int orderID)
        {
            using (var contextsql = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetOrderPayments {0}",
                    orderID);
                return contextsql.Query<Payment>(sql).ToList();
            }
        }

        private static List<OrderDetail> GetOrderDetails(int orderID)
        {
            using (var contextsql = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetOrderDetails {0}",
                    orderID);
                return contextsql.Query<OrderDetail>(sql).ToList();
            }
        }

        //private static List<OrderDetail> OrderDetailModels { get; set; }
        //private static void context_ReadingEntity(object sender, ReadingWritingEntityEventArgs e)
        //{
        //    if (OrderDetailModels == null) OrderDetailModels = new List<OrderDetail>();

        //    var orderDetailModel = ((OrderDetail)((Common.Api.ExigoOData.OrderDetail)e.Entity));

        //    OrderDetailModels.Add(orderDetailModel);
        //}

        public static void CancelOrder(int orderID)
        {
            WebService().ChangeOrderStatus(new ChangeOrderStatusRequest
            {
                OrderID = orderID,
                OrderStatus = OrderStatusType.Canceled
            });
        }

        public static OrderCalculationResponse CalculateOrder(OrderCalculationRequest request)
        {
            var result = new OrderCalculationResponse();
            if (!request.Items.Any()) return result;
            if (request.Address == null) request.Address = GlobalSettings.Company.Address;
            if (request.ShipMethodID == 0) request.ShipMethodID = request.Configuration.DefaultShipMethodID;

            var apirequest = new CalculateOrderRequest
            {
                WarehouseID = request.Configuration.WarehouseID,
                CurrencyCode = request.Configuration.CurrencyCode,
                PriceType = request.Configuration.PriceTypeID,
                ShipMethodID = request.ShipMethodID,
                ReturnShipMethods = request.ReturnShipMethods,
                Address1 = request.Address.Address1,
                Address2 = request.Address.Address2,
                City = request.Address.City,
                State = request.Address.State,
                Zip = request.Address.Zip,
                Country = request.Address.Country,
                Details = request.Items.Select(c => new OrderDetailRequest(c)).ToArray()
            };



            var apiresponse = WebService().CalculateOrder(apirequest);

            result.Subtotal = apiresponse.SubTotal;
            result.Shipping = apiresponse.ShippingTotal;
            result.Tax      = apiresponse.TaxTotal;
            result.Discount = apiresponse.DiscountTotal;
            result.Total    = apiresponse.Total;


            // Assemble the ship methods
            var shipMethods = new List<ShipMethod>();
            if (apiresponse.ShipMethods != null && apiresponse.ShipMethods.Length > 0)
            {
                shipMethods.AddRange(apiresponse.ShipMethods.Select(shipMethod => (ShipMethod) shipMethod));

                // Ensure that at least one ship method is selected
                var shipMethodID = (request.ShipMethodID != 0) ? request.ShipMethodID : request.Configuration.DefaultShipMethodID;
                if (shipMethods.Any(c => c.ShipMethodID == shipMethodID))
                {
                    shipMethods.First(c => c.ShipMethodID == shipMethodID).Selected = true;
                }
                else
                {
                    shipMethods.First().Selected = true;
                }
            }
            result.ShipMethods = shipMethods.AsEnumerable();

            return result;
        }

        public static List<ShipMethodResponse> GetShipMethodsFromApi(GetShipMethodsRequest request)
        {
            var Api = Exigo.WebService();
            var response = Api.GetShipMethods(request);
            var shippingMethods = response.ShipMethods.ToList();
            shippingMethods.ForEach(s => { s.Price = s.ShippingAmount; s.ShipMethodDescription = s.Description; });
            return shippingMethods.ToList();
        }
        public static List<OrderStatus> GetOrderStatuses()
        {

            List<OrderStatus> Getorderstatus = new List<OrderStatus>();
            using (var contextsql = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetOrderStatuses");

                Getorderstatus = contextsql.Query<OrderStatus>(sql).ToList();
                return Getorderstatus;
            }

        }


          
    }
}