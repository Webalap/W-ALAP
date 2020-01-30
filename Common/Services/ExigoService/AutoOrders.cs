using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoOrderDetailOdata = Common.Api.ExigoOData.AutoOrderDetail;
using AutoOrderOdata = Common.Api.ExigoOData.AutoOrder;
namespace ExigoService
{
    public static partial class Exigo
    {
        public static IEnumerable<AutoOrder> GetCustomerAutoOrders(GetCustomerAutoOrdersRequest request)
        {
            //var context = Exigo.OData();

            //// Setup the base query
            //var basequery = context.AutoOrders;
            //if (request.IncludeDetails) basequery = basequery.Expand("Details");


            ////// Setup the query
            //var query = basequery.Where(c => c.CustomerID == request.CustomerID);


            ////// Filters
            //if (!request.IncludeCancelledAutoOrders)
            //{
            //    query = query.Where(c => c.AutoOrderStatusID == 0);
            //}


            ////// Get the data
            //var autoOrderData = query.Select(c => c);

            //var AutoOrders = new List<AutoOrder>();
            //foreach (var autoOrder in autoOrderData)
            //{
            //    AutoOrders.Add((AutoOrder)autoOrder);
            //}


            //get auto order list
            var AutoOrders = new List<AutoOrder>();
            using (var Context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetPaymentMethodAutoOrder {0}, {1}", request.CustomerID, request.IncludeCancelledAutoOrders);
                AutoOrders = Context.Query<AutoOrder>(sqlProcedure).ToList();
                string SPGetCustomerAutoOrderShippingAddress = string.Format("GetCustomerAutoOrderShippingAddress {0}", request.CustomerID);
                var reciept = Context.Query<ShippingAddress>(SPGetCustomerAutoOrderShippingAddress).FirstOrDefault();
                AutoOrders.ForEach(c => c.Recipient = reciept);
                if (request.IncludeDetails)
                {
                    string autoOrdersIds = string.Join(",", AutoOrders.Select(s => s.AutoOrderID));
                    string SPGetCustomerAutoOrderDetail = string.Format("GetCustomerAutoOrderDetail '{0}'", autoOrdersIds);
                    var detailsList = Context.Query<AutoOrderDetail>(SPGetCustomerAutoOrderDetail).ToList();
                    foreach (var item in AutoOrders)
                    {
                        var lstDetails = detailsList.Where(s => s.AutoOrderID == item.AutoOrderID);
                        item.Details = lstDetails.ToList();
                    }
                    
                }
            }

            // If we don't have any autoships, stop here.
            if (AutoOrders.Count == 0) yield break;


            // Get the payment methods if applicable
            if (request.IncludePaymentMethods)
            {
                var paymentMethods = GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
                {
                    CustomerID = request.CustomerID
                });
                foreach (var autoOrder in AutoOrders)
                {
                    IPaymentMethod paymentMethod;
                    switch (autoOrder.AutoOrderPaymentTypeID)
                    {
                        case 1: paymentMethod = paymentMethods.Where(c => c is CreditCard && ((CreditCard)c).Type == CreditCardType.Primary).FirstOrDefault(); break;
                        case 2: paymentMethod = paymentMethods.Where(c => c is CreditCard && ((CreditCard)c).Type == CreditCardType.Secondary).FirstOrDefault(); break;
                        case 3: paymentMethod = paymentMethods.Where(c => c is BankAccount && ((BankAccount)c).Type == ExigoService.BankAccountType.Primary).FirstOrDefault(); break;
                        default: paymentMethod = null; break;
                    }
                    autoOrder.PaymentMethod = paymentMethod;
                }
            }


            // Determine if each autoship contains only virtual items if applicable
            //if (request.IncludeDetails)
            //{
            //    var itemIDs = new List<int>();
            //    AutoOrders.ForEach(a =>
            //    {
            //        if (a.Details != null && a.Details.Count() > 0)
            //        {
            //            itemIDs.AddRange(a.Details.Select(c => c.ItemID));
            //        }
            //    });
            //    itemIDs = itemIDs.Distinct().ToList();

            //    // Determine which item IDs are virtual
            //    var virtualItems = context.Items
            //        .Where(c => c.IsVirtual)
            //        .Where(itemIDs.ToOrExpression<Common.Api.ExigoOData.Item, int>("ItemID"))
            //        .Select(c => new { c.ItemID })
            //        .ToList();
            //    var virtualItemIDs = virtualItems.Select(c => c.ItemID).ToList();

            //    // Loop through each auto order, setting the details
            //    foreach (var autoOrder in AutoOrders)
            //    {
            //        foreach (var detail in autoOrder.Details)
            //        {
            //            if (virtualItemIDs.Contains(detail.ItemID))
            //            {
            //                detail.IsVirtual = true;
            //            }
            //        }
            //    }
            //}


            // Get the images
            //if (request.IncludeDetails)
            //{
            //    var itemcodes = new List<string>();
            //    foreach (var order in AutoOrders)
            //    {
            //        itemcodes.AddRange(order.Details.Select(c => c.ItemCode));
            //    }

            //    var images = new List<Common.Api.ExigoOData.Item>();
            //    if (itemcodes.Count > 0)
            //    {
            //        images = context.Items
            //            .Where(itemcodes.ToOrExpression<Common.Api.ExigoOData.Item, string>("ItemCode"))
            //            .Select(c => new Common.Api.ExigoOData.Item
            //            {
            //                ItemCode = c.ItemCode,
            //                SmallImageUrl = c.SmallImageUrl
            //            })
            //            .ToList();
            //    }


            //    // Loop through each auto order, setting the details
            //    foreach (var autoOrder in AutoOrders)
            //    {
            //        foreach (var detail in autoOrder.Details)
            //        {
            //            var image = images.Where(c => c.ItemCode == detail.ItemCode).FirstOrDefault();
            //            if (image != null) detail.ImageUrl = image.SmallImageUrl;
            //        }
            //    }
            //}


            // Return the results
            foreach (var autoOrder in AutoOrders)
            {
                yield return autoOrder;
            }
        }
        public static AutoOrder GetCustomerAutoOrder(int customerID, int autoOrderID)
        {
            //var context = Exigo.OData();

            //// Setup the base query
            //var autoOrderData = context.AutoOrders.Expand("Details")
            //    .Where(c => c.CustomerID == customerID)
            //    .Where(c => c.AutoOrderID == autoOrderID)
            //    .FirstOrDefault();
            //if (autoOrderData == null) return null;

            //var autoOrder = (AutoOrder)autoOrderData;


            // Get the payment methods if applicable
            var paymentMethods = GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID = customerID
            });

            //get auto orders from sql using following stored procedures
            var autoOrder = new AutoOrder();
            using (var Context = Exigo.Sql())
            {
                string SPGetCustomerAutoOrders = string.Format("GetCustomerAutoOrders {0},{1}", customerID, autoOrderID);
                autoOrder = Context.Query<AutoOrder>(SPGetCustomerAutoOrders).FirstOrDefault();
                string SPGetCustomerAutoOrderDetail = string.Format("GetCustomerAutoOrderDetail {0}", autoOrderID.ToString());
                var detailsList = Context.Query<AutoOrderDetail>(SPGetCustomerAutoOrderDetail).ToList();
                autoOrder.Details = detailsList.ToList();
                string SPGetCustomerAutoOrderShippingAddress = string.Format("GetCustomerAutoOrderShippingAddress {0}", customerID);
                var reciept = Context.Query<ShippingAddress>(SPGetCustomerAutoOrderShippingAddress).FirstOrDefault();
                autoOrder.Recipient = reciept;
            }
            IPaymentMethod paymentMethod;
            switch (autoOrder.AutoOrderPaymentTypeID)
            {
                case 1: paymentMethod = paymentMethods.Where(c => c is CreditCard && ((CreditCard)c).Type == CreditCardType.Primary).FirstOrDefault(); break;
                case 2: paymentMethod = paymentMethods.Where(c => c is CreditCard && ((CreditCard)c).Type == CreditCardType.Secondary).FirstOrDefault(); break;
                case 3: paymentMethod = paymentMethods.Where(c => c is BankAccount && ((BankAccount)c).Type == ExigoService.BankAccountType.Primary).FirstOrDefault(); break;
                default: paymentMethod = null; break;
            }
            autoOrder.PaymentMethod = paymentMethod;


            // Determine if each autoship contains only virtual items if applicable
            //var itemIDs = new List<int>();
            //if (autoOrder.Details != null && autoOrder.Details.Count() > 0)
            //{
            //    itemIDs.AddRange(autoOrder.Details.Select(c => c.ItemID));
            //}
            //itemIDs = itemIDs.Distinct().ToList();

            //// Determine which item IDs are virtual
            //var virtualItems = context.Items
            //    .Where(c => c.IsVirtual)
            //    .Where(itemIDs.ToOrExpression<Common.Api.ExigoOData.Item, int>("ItemID"))
            //    .Select(c => new { c.ItemID })
            //    .ToList();
            //var virtualItemIDs = virtualItems.Select(c => c.ItemID).ToList();

            //// Loop through each auto order, setting the details
            //foreach (var detail in autoOrder.Details)
            //{
            //    if (virtualItemIDs.Contains(detail.ItemID))
            //    {
            //        detail.IsVirtual = true;
            //    }
            //}

            // Return the results
            return autoOrder;
        }

        public static void UpdateCustomerAutoOrderShippingAddress(int customerID, int autoOrderID, ShippingAddress address)
        {
            //var context = Exigo.OData();


            // Get the autoorder
            //var autoOrder = context.AutoOrders
            //    .Where(c => c.CustomerID == customerID)
            //    .Where(c => c.AutoOrderID == autoOrderID)
            //    .FirstOrDefault();
            //get auto orders from sql using following stored procedures
            AutoOrder autoOrder = new AutoOrder();
            using (var Context = Exigo.Sql())
            {
                string SPGetCustomerAutoOrders = string.Format("GetCustomerAutoOrders {0},{1}", customerID, autoOrderID);
                autoOrder = Context.Query<AutoOrder>(SPGetCustomerAutoOrders).FirstOrDefault();
            }
            if (autoOrder == null) return;


            // Re-create the autoorder
            var request       = GetCreateOverridenAutoOrderRequest(customerID, autoOrderID);
            request.FirstName = address.FirstName;
            request.LastName  = address.LastName;
            request.Company   = address.Company;
            request.Address1  = address.Address1;
            request.Address2  = address.Address2;
            request.City      = address.City;
            request.State     = address.State;
            request.Zip       = address.Zip;
            request.Country   = address.Country;
            request.Email     = address.Email;
            request.Phone     = address.Phone;


            // Update the autoorder
            var response = Exigo.WebService().CreateAutoOrder(request);
        }
        public static void UpdateCustomerAutoOrderPaymentMethod(int customerID, int autoOrderID, AutoOrderPaymentType type)
        {
            // Re-create the autoorder
            var request = GetCreateOverridenAutoOrderRequest(customerID, autoOrderID);
            if (request == null) return;

            request.PaymentType = type;


            // Update the autoorder
            var response = Exigo.WebService().CreateAutoOrder(request);
        }
        public static void DeleteCustomerAutoOrder(int customerID, int autoOrderID)
        {
            // Get the autoorder
            //var context = Exigo.OData();
            //var autoOrder = context.AutoOrders
            //    .Where(c => c.CustomerID == customerID)
            //    .Where(c => c.AutoOrderID == autoOrderID);
            AutoOrder autoOrder = new AutoOrder();
            using (var Context = Exigo.Sql())
            {
                string SPGetCustomerAutoOrders = string.Format("GetCustomerAutoOrders {0},{1}", customerID, autoOrderID);
                autoOrder = Context.Query<AutoOrder>(SPGetCustomerAutoOrders).FirstOrDefault();
            }
            if (autoOrder == null) return;


            // Cancel the autoorder
            var response = Exigo.WebService().ChangeAutoOrderStatus(new ChangeAutoOrderStatusRequest
            {
                AutoOrderID     = autoOrderID,
                AutoOrderStatus = AutoOrderStatusType.Deleted
            });
        }
        public static bool IsValidAutoOrderID(int customerID, int autoOrderID, bool includeCancelledAutoOrders = false)
        {
            // Get the autoorder
            //var context = Exigo.OData();
            //var query = context.AutoOrders
            //    .Where(c => c.CustomerID == customerID)
            //    .Where(c => c.AutoOrderID == autoOrderID);
            AutoOrder autoOrder = new AutoOrder();
            using (var Context = Exigo.Sql())
            {
                string SPGetCustomerAutoOrders = string.Format("GetCustomerAutoOrders {0},{1}", customerID, autoOrderID);
                autoOrder = Context.Query<AutoOrder>(SPGetCustomerAutoOrders).FirstOrDefault();
            }
            // Only pull active auto orders if applicable
            //if (!includeCancelledAutoOrders) query = query.Where(c => c.AutoOrderStatusID == 1);
            bool isAutoOrdercancelled=true;
            if (!includeCancelledAutoOrders)
            {
                isAutoOrdercancelled = autoOrder.AutoOrderStatusID == 1;
            }
            if (!isAutoOrdercancelled)
            {
                autoOrder = new AutoOrder();
            }
           // var autoOrder = query.FirstOrDefault();

            return autoOrder != null;
        }

        public static DateTime GetNewAutoOrderStartDate(FrequencyType frequency)
        {
            DateTime autoshipstartDate = new DateTime();
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            switch (frequency)
            {
                case FrequencyType.Weekly: autoshipstartDate = now.AddDays(7); break;
                case FrequencyType.BiWeekly: autoshipstartDate = now.AddDays(14); break;
                case FrequencyType.EveryFourWeeks: autoshipstartDate = now.AddDays(28); break;
                case FrequencyType.Monthly: autoshipstartDate = now.AddMonths(1); break;
                case FrequencyType.BiMonthly: autoshipstartDate = now.AddMonths(2); break;
                case FrequencyType.Quarterly: autoshipstartDate = now.AddMonths(3); break;
                case FrequencyType.SemiYearly: autoshipstartDate = now.AddMonths(6); break;
                case FrequencyType.Yearly: autoshipstartDate = now.AddYears(1); break;
                default: autoshipstartDate = now; break;
            }

            // Ensure we are not returning a day of 29, 30 or 31.
            autoshipstartDate = GetNextAvailableAutoOrderStartDate(autoshipstartDate);

            return autoshipstartDate;
        }
        public static DateTime GetNextAvailableAutoOrderStartDate(DateTime date)
        {
            // Ensure we are not returning a day of 29, 30 or 31.
            if (date.Day > 28)
            {
                date = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1).Date;
            }

            return date;
        }

        public static AutoOrderPaymentType GetAutoOrderPaymentType(int autoOrderPaymentTypeID)
        {
            switch (autoOrderPaymentTypeID)
            {
                case 1: return AutoOrderPaymentType.PrimaryCreditCard;
                case 2: return AutoOrderPaymentType.SecondaryCreditCard;
                case 3: return AutoOrderPaymentType.CheckingAccount;
                case 4: return AutoOrderPaymentType.WillSendPayment;
                case 6: return AutoOrderPaymentType.PrimaryWalletAccount;
                case 7: return AutoOrderPaymentType.SecondaryWalletAccount;

                default: return AutoOrderPaymentType.PrimaryCreditCard;
            }
        }
        public static AutoOrderPaymentType GetAutoOrderPaymentType(IPaymentMethod paymentMethod)
        {
            if (!(paymentMethod is IAutoOrderPaymentMethod)) throw new Exception("The provided payment method does not implement IAutoOrderPaymentMethod.");

            if (paymentMethod is CreditCard) return ((CreditCard)paymentMethod).AutoOrderPaymentType;
            if (paymentMethod is BankAccount) return ((BankAccount)paymentMethod).AutoOrderPaymentType;

            return AutoOrderPaymentType.WillSendPayment;
        }
        public static AutoOrderProcessType GetAutoOrderProcessType(int autoOrderProcessTypeID)
        {
            switch (autoOrderProcessTypeID)
            {
                case 1: return AutoOrderProcessType.AlwaysProcess;
                case 2: return AutoOrderProcessType.Conditional;

                default: return AutoOrderProcessType.AlwaysProcess;
            }
        }

        private static CreateAutoOrderRequest GetCreateOverridenAutoOrderRequest(int customerID, int autoOrderID)
        {
            // Get the autoorder
            //var context = Exigo.OData();
            //var autoOrder = context.AutoOrders.Expand("Details")
            //    .Where(c => c.CustomerID == customerID)
            //    .Where(c => c.AutoOrderID == autoOrderID)
            //    .FirstOrDefault();
            var autoOrder = new AutoOrderOdata();
            using (var Context = Exigo.Sql())
            {
                string SPGetCustomerAutoOrders = string.Format("GetCustomerAutoOrders {0},{1}", customerID, autoOrderID);
                autoOrder = Context.Query<AutoOrderOdata>(SPGetCustomerAutoOrders).FirstOrDefault();
                string SPGetCustomerAutoOrderDetail = string.Format("GetCustomerAutoOrderDetail {0}", autoOrderID.ToString());
                var detailsList = Context.Query<AutoOrderDetailOdata>(SPGetCustomerAutoOrderDetail).ToList();
                detailsList.ForEach(i => autoOrder.Details.Add(i));
            }
            
            // Re-create the autoorder
            return new CreateAutoOrderRequest(autoOrder);
        }
    }
}