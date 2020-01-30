using AutoMapper;
using Common;
using Common.Api.ExigoOData.Rewards;
using Common.Api.ExigoWebService;
using Common.ModelsEx.DashBoard;
using Common.ModelsEx.Reward;
using Common.ModelsEx.SavedCart;
using Common.ModelsEx.Shopping;
using Common.Providers;
using Common.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
//using Common.Api.ExigoOData.Rewards;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
//using ODataCustomer = Common.Api.ExigoOData.Customer;
using System.Text.RegularExpressions;
using System.Web;

namespace ExigoService
{
    public static partial class Exigo
    {
        private static Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static int CreateCustomer(Customer customer, bool canLogin = false)
        {
            var api = WebService();

            var createCustomerRequest = new CreateCustomerRequest(customer) { CanLogin = canLogin };


            var response = api.CreateCustomer(createCustomerRequest);

            if (ResultStatus.Success.Equals(response.Result.Status))
            {
                return response.CustomerID;
            }

            throw new ApplicationException("Create Customer Failed.");
        }
        public static int UpdateCustomer(Customer customer)
        {
            var api = WebService();

            var updateCustomerRequest = new UpdateCustomerRequest(customer)
            {
                CustomerID = customer.CustomerID
            };

            var response = api.UpdateCustomer(updateCustomerRequest);

            if (ResultStatus.Success.Equals(response.Result.Status))
            {
                return customer.CustomerID;
            }

            throw new ApplicationException("Update Customer Failed.");
        }
        public static string LoadFromHtmlTemplate(string filePath, string htmlFileName, object liveObject, string start, string end)
        {
            using (WebClient webClient = new WebClient())
            {
                string url = (HttpRuntime.AppDomainAppPath + filePath + htmlFileName).Replace('/', '\\');

                using (Stream stream = webClient.OpenRead(url))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string emailBody = reader.ReadToEnd();
                        Match match = Regex.Match(emailBody, string.Format(@"({0})([A-Z])\w+({1})", start, end));
                        List<string> matches = new List<string>();
                        while (match.Success)
                        {
                            if (!matches.Contains(match.Value))
                            {
                                matches.Add(match.Value);
                            }
                            match = match.NextMatch();
                        }
                        Dictionary<string, string> replacements = GetDictionary(liveObject, matches);

                        if (null != replacements)
                        {
                            foreach (var item in replacements)
                            {
                                emailBody = emailBody.Replace(item.Key, item.Value);
                            }
                        }

                        return emailBody;
                    }
                }
            }

        }
        private static Dictionary<string, string> GetDictionary(object liveobject, List<string> fields)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            var _objectdict = liveobject.ToDictionary();
            decimal number = 0;
            foreach (var field in fields)
            {
                var value = _objectdict.Where(i => i.Key.Equals(field.Replace(@"<%", string.Empty).Replace(@"%>", string.Empty))).FirstOrDefault().Value;
                if (decimal.TryParse(Convert.ToString(value), out number))
                {
                    _dict.Add(field, Math.Floor(number).ToString());
                }
                else
                {
                    _dict.Add(field, Convert.ToString(value));
                }
            }

            return _dict;
        }

        public static bool IsCurrentMethod(CheckLogicResult check, string actionName)
        {
            return ((System.Web.Mvc.RedirectToRouteResult)check.NextAction).RouteValues["action"].Equals(actionName);
        }

        //public static string GetSavedCartByCartID(int cartID)
        //{
        //    var Datalist = new List<SavedCart>();
        //    using (var contextsql = Exigo.Sql())
        //    {
        //        var sql = string.Format(@"Exec GetSpecificSavedCart {0}", cartID);
        //        Datalist = contextsql.Query<SavedCart>(sql).ToList();
        //    }
        //    return Datalist.FirstOrDefault().PropertyBag;
        //}

        //public static BasePropertyBag GetSavedCartByCartID(int cartID, string shoppingCartName)
        //{
        //    var Datalist = new List<SavedCart>();
        //    using (var contextsql = Exigo.Sql())
        //    {
        //        var sql = string.Format(@"Exec GetSpecificSavedCart {0}", cartID);
        //        Datalist = contextsql.Query<SavedCart>(sql).ToList();
        //    }
        //    IList<BasePropertyBag> lstPropertyBag = new List<BasePropertyBag>();
        //    foreach (var item in Datalist)
        //    {
        //        if (shoppingCartName.Contains("BackofficeShopping"))
        //            lstPropertyBag.Add(
        //                JsonConvert.DeserializeObject<PersonalShopppingCart>(
        //                item.PropertyBag
        //                )
        //            );
        //        if (shoppingCartName.Contains("RetailCustomerOrderPropertyBag"))
        //            lstPropertyBag.Add(
        //                JsonConvert.DeserializeObject<RetailCustomerOrderPropertyBag>(
        //                item.PropertyBag
        //                )
        //            );
        //        if (shoppingCartName.Contains("EventCustomerOrderPropertyBag"))
        //            lstPropertyBag.Add(
        //                JsonConvert.DeserializeObject<EventCustomerOrderPropertyBag>(
        //                item.PropertyBag
        //                )
        //            );
        //    }
        //    return lstPropertyBag.FirstOrDefault();
        //}

        public static List<SavedCartViewModel> GetSavedCart(int customerID)
        {
            List<SavedCartViewModel> lstCarts = new List<SavedCartViewModel>();
            List<SavedCart> Datalist = new List<SavedCart>();
            using (var contextsql = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetSavedCart {0}", customerID);
                Datalist = contextsql.Query<SavedCart>(sql).ToList();
            }

            IList<BasePropertyBag> lstPropertyBag = new List<BasePropertyBag>();
            foreach (var item in Datalist)
            {
                if (item.PropertyBag.Contains("BackofficeShopping"))
                {
                    var personalOrder = JsonConvert.DeserializeObject<PersonalShopppingCart>(item.PropertyBag);
                    lstCarts.Add(new SavedCartViewModel
                    {
                        Id = item.ID,
                        CartName = "Personal Order - ",
                        UrlLink = string.Format("store/checkout/payment"),
                        PropertyBag = personalOrder,
                        ImageLink = @"/Content/images/DashBoardIcons/personal.jpg",
                        DisplayText = string.Format(@"You have {0:N0} products totaling {1:c} in your saved cart.", personalOrder.ShoppingCartItemsPropertyBag.Items.Where(s => s.ParentItemCode == null).Sum(i => i.Quantity), personalOrder.ShoppingCartItemsPropertyBag.Items.Sum(i => i.Price * i.Quantity))
                    });
                }
                if (item.PropertyBag.Contains("RetailCustomerOrderPropertyBag"))
                {
                    var RetailOrder = JsonConvert.DeserializeObject<RetailCustomerOrderPropertyBag>(item.PropertyBag);
                    if (RetailOrder.Customer.CustomerID != 0)
                    {
                        lstCarts.Add(new SavedCartViewModel
                        {
                            Id = item.ID,
                            CartName = "Retail Order - ",
                            UrlLink = RetailOrder.ShippingMethodId == 0 ? @"retailshopping/shipping" : @"retailshopping/payment",
                            PropertyBag = RetailOrder,
                            ImageLink = @"/Content/images/DashBoardIcons/retailorderico.png",
                            DisplayText = string.Format(@"You have a saved cart for {0} {1} totaling {2:c}.", RetailOrder.Customer.FirstName, RetailOrder.Customer.LastName, RetailOrder.Order.Subtotal)
                        });
                    }

                }
                if (item.PropertyBag.Contains("EventCustomerOrderPropertyBag"))
                {
                    var eventOrder = JsonConvert.DeserializeObject<EventCustomerOrderPropertyBag>(item.PropertyBag);
                    if (eventOrder.EventId != 0)
                    {
                        lstCarts.Add(new SavedCartViewModel
                        {
                            Id = item.ID,
                            CartName = "Get Together Order - ",
                            UrlLink = eventOrder.ShippingMethodId == 0 ? string.Format("events/{0}/shipping", eventOrder.EventId) : string.Format("events/{0}/payment", eventOrder.EventId),
                            PropertyBag = eventOrder,
                            ImageLink = @"/Content/images/DashBoardIcons/gtorder.png",
                            DisplayText = string.Format(@"You have a saved cart for {0} {1} totaling  {2:c}.", eventOrder.Customer.FirstName, eventOrder.Customer.LastName, eventOrder.Order.Subtotal)
                        });
                    }

                }
            }
            return lstCarts;
        }


        public static List<DisplaySaveCart> GetCustomerWishList(int customerID, string siteType)
        {
            // List<SavedCartViewModel> lstCarts = new List<SavedCartViewModel>();
            List<DisplaySaveCart> Datalist = new List<DisplaySaveCart>();
            using (var contextsql = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetSavedCartBySiteType {0},'{1}'", customerID, siteType.Replace("'", "''"));
                //Datalist = contextsql.Query<SavedCart>(sql).ToList();
                Datalist = contextsql.Query<DisplaySaveCart>(sql).ToList();
            }

            // IList<BasePropertyBag> lstPropertyBag = new List<BasePropertyBag>();
            // string savedCartsString = "You have following saved carts";

            //foreach (var item in Datalist)
            //{
            //    if (item.PropertyBag.Contains("ReplicatedShopping"))
            //    {
            //        var personalOrder = JsonConvert.DeserializeObject<Common.ModelsEx.Replicated.ShoppingCartCheckoutPropertyBag>(item.PropertyBag);
            //        lstCarts.Add(new SavedCartViewModel
            //        {
            //            Id = item.ID,
            //            CartName = "Replicated Site Order - ",
            //            UrlLink = string.Format("{0}rep/india/shopping/payment", ConfigurationManager.AppSettings["ReplicatedSiteDomain"]),
            //            PropertyBag = personalOrder,
            //            ImageLink = @"/Content/images/DashBoardIcons/personal.jpg",
            //            DisplayText = string.Format(@"You have {0:N0} products total {1:c} in your saved cart.", personalOrder.ShoppingCart.Products.Sum(i => i.Quantity), personalOrder.ShoppingCart.Products.Sum(i => i.Subtotal))
            //        });
            //    }


            //}
            return Datalist;
        }
        

        public static bool DeleteCart(int cartId)
        {
            using (var contextsql = Exigo.Sql())
            {

                var sql = string.Format(@"Exec DeleteCart {0}", cartId);
                var list = contextsql.Query<SavedCart>(sql).ToList();
                return list.Count == 0 ? true : false;
            }
        }

        public static bool ConvertSavedCart(int cartId)
        {
            using (var contextsql = Exigo.Sql())
            {

                var sql = string.Format(@"Exec Convert_EventCart_RetailCart {0}", cartId);
                var list = contextsql.Query<SavedCart>(sql).ToList();
                return list.Count == 0 ? true : false;
            }
        }
        public static IsCartEventClosed CheckGTSavedCartEvent(int cartId)
        {
            using (var contextsql = Exigo.Sql())
            {

                var sql = string.Format(@"Exec IsEventClose {0}", cartId);
                return contextsql.Query<IsCartEventClosed>(sql).FirstOrDefault();
            }
        }     
        public static bool SaveCart(int customerID, string data, string shoppingCartName, string siteType = "", int? itemCount = 0, decimal? total = 0, string customerName = "")

        {
            try
            {
                using (var contextsql = Exigo.Sql())
                {
                    int cartID = 0;
                    data = data.Replace("'", "''");
                    var sql = string.Format(@"Exec Insert_Update_SavedCart {0} , '{1}',{2},{3},'{4}','{5}',{6},'{7}'", customerID, data, itemCount, total, shoppingCartName.Replace("'", "''"), siteType, cartID, customerName.Replace("'", "''"));
                    var list = contextsql.Query<SavedCart>(sql).ToList();
                    return list.Count > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public static bool UpdateCart(string data, int CartID, int? itemCount = 0, decimal? total = 0, string customerName = "")
        {
            try
            {
                using (var contextsql = Exigo.Sql())
                {
                    data = data.Replace("'", "''");
                    //var sql = string.Format(@"Exec UpdateSavedCart '{0}',{1}", data, CartID);
                    var sql = string.Format(@"Exec Insert_Update_SavedCart {0} , '{1}',{2},{3},'{4}','{5}',{6},'{7}'", 0, data, itemCount, total, "", "", CartID, customerName.Replace("'", "''"));
                    var list = contextsql.Query<SavedCart>(sql).ToList();
                    return list.Count > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        
        public static List<ShipMethod> GetShipMethods(decimal total=0, int WarehouseID=Warehouses.DefaultUSA)
        {
            var NewList = new List<ShipMethod>();

            try
            {
                using (var Context = Sql())
                {
                    Context.Open();
                    //Getting shipment method list
                    string sqlProcedure = string.Format("GetShipMethods {0},{1}", total, WarehouseID);
                    NewList = Context.Query<ShipMethod>(sqlProcedure).ToList();
                    Context.Close();
                }
                return NewList;
            }
            catch (Exception ex)
            {
                // ignored
            }
            return NewList;
        }

        public static int CreatePartyGuest(string data, int currentid)
        {
            var AttributeList = new List<string>();
            var atribute = new StringBuilder();
            try
            {
                foreach (var item in data)
                {
                    if (item.CompareTo('`') == 0)
                    {
                        AttributeList.Add(atribute.ToString());
                        atribute.Clear();
                    }
                    else
                    {
                        atribute.Append(item);
                    }
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
            try
            {
                if (AttributeList[0] == string.Empty) return 0;
                var custToInsert = new CreateCustomerRequest
                {
                    FirstName = AttributeList[0],
                    LastName = AttributeList[1],
                    Phone = AttributeList[2],
                    Email = AttributeList[3],
                    MainAddress1 = AttributeList[4],
                    MainAddress2 = AttributeList[5],
                    MainCity = AttributeList[6],
                    MainState = AttributeList[7],
                    MainZip = AttributeList[8],
                    MailAddress1 = AttributeList[9],
                    MailAddress2 = AttributeList[10],
                    MailCity = AttributeList[11],
                    MailState = AttributeList[12],
                    MailZip = AttributeList[13],
                    MainCountry = AttributeList[14],
                    MailCountry = AttributeList[15],
                    EnrollerID = currentid,
                    SponsorID = currentid,
                    CustomerType = CustomerTypes.PartyGuest,
                    CustomerStatus = CustomerStatuses.Active,
                    InsertEnrollerTree = true,
                    InsertUnilevelTree = true,
                    EntryDate = DateTime.Now
                };

                var customerPrevious = GetCustomer(AttributeList[3]);
                if (customerPrevious.Count == 0)
                {
                    var response = WebService().CreateCustomer(custToInsert);

                    return response.Result.Status == ResultStatus.Success ? response.CustomerID : 0;
                }

                var firstOrDefault = customerPrevious.FirstOrDefault(c => c.FirstName == AttributeList[0] && c.LastName == AttributeList[1] && c.Email == AttributeList[3] && c.EnrollerID == currentid);
                if (firstOrDefault != null)
                {
                    var custid = firstOrDefault.CustomerID;
                    return custid > 0 ? custid : 0;
                }
                var resp = WebService().CreateCustomer(custToInsert);
                return resp.Result.Status == ResultStatus.Success ? resp.CustomerID : 0;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static int CreateRetailCustomer(string data, int currentid)
        {
            var attributeList = new List<string>();
            var atribute = new StringBuilder();
            foreach (var item in data)
            {
                if (item.CompareTo('`') == 0)
                {
                    attributeList.Add(atribute.ToString());
                    atribute.Clear();
                }
                else
                {
                    atribute.Append(item);
                }
            }
            try
            {
                var custToInsert = new CreateCustomerRequest
                {
                    FirstName = attributeList[0],
                    LastName = attributeList[1],
                    Phone = attributeList[2],
                    Email = attributeList[3],
                    MainAddress1 = attributeList[4],
                    MainAddress2 = attributeList[5],
                    MainCity = attributeList[6],
                    MainState = attributeList[7],
                    MainZip = attributeList[8],
                    MailAddress1 = attributeList[9],
                    MailAddress2 = attributeList[10],
                    MailCity = attributeList[11],
                    MailState = attributeList[12],
                    MailZip = attributeList[13],
                    MainCountry = attributeList[14],
                    MailCountry = attributeList[15],
                    EnrollerID = currentid,
                    SponsorID = currentid,
                    CustomerType = CustomerTypes.RetailCustomer,
                    CustomerStatus = CustomerStatuses.Active,
                    InsertEnrollerTree = true,
                    InsertUnilevelTree = true,
                    EntryDate = DateTime.Now
                };

                var customerPrevious = GetCustomer(attributeList[3]);
                if (customerPrevious.Count == 0)
                {
                    var response = WebService().CreateCustomer(custToInsert);

                    return response.Result.Status == ResultStatus.Success ? response.CustomerID : 0;
                }

                var firstOrDefault = customerPrevious.FirstOrDefault(c => c.FirstName == attributeList[0] && c.LastName == attributeList[1] && c.Email == attributeList[3] && c.EnrollerID == currentid);
                if (firstOrDefault != null)
                {
                    var custid = firstOrDefault.CustomerID;
                    return custid > 0 ? custid : 0;
                }
                var resp = WebService().CreateCustomer(custToInsert);
                return resp.Result.Status == ResultStatus.Success ? resp.CustomerID : 0;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public static List<CustomerSite> GetCustomerByWebalias(string webalias)
        {

            using (var context = Sql())
            {
                var sql = string.Format(@"Exec GetCustomerByWebalias '{0}'", webalias);
                return context.Query<CustomerSite>(sql).ToList();
            }
            throw new ApplicationException("Get Customer Webalias Failed.");
        }

        public static List<Customer> GetCustomerByFirstName(string firstName)
        {

            using (var context = Sql())
            {
                var sql = string.Format(@"Exec GetCustomerByFirstName '{0}'", firstName.Replace("'", "''"));
                return context.Query<Customer>(sql).ToList();
            }
            throw new ApplicationException("Get Customer Failed.");
        }

        public static Customer GetCustomerByIDandType(int customerID, int CustomerTypeID)
        {

            using (var context = Sql())
            {
                var sql = string.Format(@"Exec GetCustomerByIDandType {0},{1}", customerID, CustomerTypeID);
                return context.Query<Customer>(sql).FirstOrDefault();
            }
            throw new ApplicationException("Get Customer Failed.");
        }

        public static Customer GetCustomerByTypeID(int customerID, int CustomerTypeID)
        {

            using (var context = Sql())
            {
                var sql = string.Format(@"Exec GetCustomerByTypeID {0},{1}", customerID, CustomerTypeID);
                return context.Query<Customer>(sql).FirstOrDefault();
            }
            throw new ApplicationException("Get Customer Failed.");
        }

        public static CustomerType GetCustomerByTypes(int CustomerTypeID)
        {

            using (var context = Sql())
            {
                var sql = string.Format(@"Exec GetCustomerByTypes {0}", CustomerTypeID);
                return context.Query<CustomerType>(sql).FirstOrDefault();
            }
            throw new ApplicationException("Get Customer Failed.");
        }
        public static Customer GetCustomer(int customerID)
        {
           
            try
            {
               return PopulateCustomer(customerID);
            }
            catch
            {
                //throw new ApplicationException("Get Customer Failed.");
                //geting from Exigo when there delay occurs for DB sync
                //--------Previous Code-----------
                return PopulateCustomer(customerID);
            }
        }
        private static Customer PopulateCustomer(int customerID) {
            Customer customer = new Customer();
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetRetailCustomer {0}", customerID);
                customer = context.Query<Customer>(sql).FirstOrDefault();
            }
            if (customer != null)
            {
                customer.MainAddress.Address1 = customer.MainAddress1;
                customer.MainAddress.Address2 = customer.MainAddress2;
                customer.MainAddress.City = customer.MainCity;
                customer.MainAddress.Country = customer.MainCountry;
                customer.MainAddress.Zip = customer.MainZip;
                customer.MainAddress.State = customer.MainState;
                customer.MailingAddress.Address1 = customer.MailAddress1;
                customer.MailingAddress.Address2 = customer.MailAddress2;
                customer.MailingAddress.City = customer.MailCity;
                customer.MailingAddress.Country = customer.MailCountry;
                customer.MailingAddress.Zip = customer.MailZip;
                customer.MailingAddress.State = customer.MailState;
            }
            else
            {
                if (customer == null)
                    customer = new Customer();

                var api = WebService();
                var response = api.GetCustomers(new GetCustomersRequest()
                {
                    CustomerID = customerID
                });
                var ApiCustomer = response.Customers.FirstOrDefault();
                customer = (Customer)ApiCustomer;
           
                customer.MainAddress.Address1 = ApiCustomer.MainAddress1;
                customer.MainAddress.Address2 = ApiCustomer.MainAddress2;
                customer.MainAddress.City = ApiCustomer.MainCity;
                customer.MainAddress.Country = ApiCustomer.MainCountry;
                customer.MainAddress.Zip = ApiCustomer.MainZip;
                customer.MainAddress.State = ApiCustomer.MainState;
                customer.MailingAddress.Address1 = ApiCustomer.MailAddress1;
                customer.MailingAddress.Address2 = ApiCustomer.MailAddress2;
                customer.MailingAddress.City = ApiCustomer.MailCity;
                customer.MailingAddress.Country = ApiCustomer.MailCountry;
                customer.MailingAddress.Zip = ApiCustomer.MailZip;
                customer.MailingAddress.State = ApiCustomer.MailState;

            }
            //calling to get highestAchievedRank
            Rank highestAchievedRank = GetHighestAchievedRank(customerID);
            customer.HighestAchievedRank = highestAchievedRank;
            customer.HighestAchievedRankId = (null != highestAchievedRank ? highestAchievedRank.RankID : 0);
            return customer;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static Rank GetHighestAchievedRank(int CustomerID)
        {

            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetHighestAchievedRank {0}", CustomerID);
                return context.Query<Rank>(sql).FirstOrDefault();
            }
            throw new ApplicationException("Get Rank Failed.");
        }

        public static Customer GetCustomersEnroller(int customerID)
        {
            var api = WebService();
            var response = api.GetCustomers(new GetCustomersRequest()
            {
                CustomerID = customerID
            });

            if (!ResultStatus.Success.Equals(response.Result.Status))
                throw new ApplicationException("Get Customer Failed.");
            if (response.Customers != null)
            {
                response = api.GetCustomers(new GetCustomersRequest()
                {
                    CustomerID = response.Customers[0].EnrollerID
                });
                return (Customer)response.Customers[0];
            }

            throw new ApplicationException("Get Customer Failed.");
        }


        public static List<Customer> GetCustomer(string emailAddress)
        {
            //--------Previous Code-----------
            //var api = WebService();
            //var response = api.GetCustomers(new GetCustomersRequest()
            //{
            //    Email = emailAddress
            //});

            //if (ResultStatus.Success.Equals(response.Result.Status))
            //{
            //    return response.Customers.Select(Mapper.Map<Customer>).ToList();
            //}
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetCustomerByEmail '{0}'", emailAddress);
                return context.Query<Customer>(sql).ToList();
            }
            throw new ApplicationException("Get Customer Failed.");
        }

        public static List<Customer> GetCustomersExpiringCreditsTopTen()
        {

            var context = Sql();
            var currentPeriod = Exigo.GetCurrentPeriod(PeriodTypes.Monthly);

            string sql = string.Format(@"Select Top(10) c.CustomerID
                , c.FirstName
                , c.LastName
                ,c.CanLogin
                , pv.Volume12 As PRVBalance
                , pv.Volume14 As Active
                , cpa.PointBalance AS 'PointBalance (Running Balance)'
                FROM Customers c
                INNER JOIN PeriodVolumes pv
                ON
                c.CustomerID = pv.CustomerID
                INNER JOIN CustomerPointAccounts cpa
                ON
                c.CustomerID = cpa.CustomerID
                WHERE
                c.CustomerTypeID = 3
                AND
                c.CustomerID > 1000
                And
                pv.PeriodTypeID = 2
                AND
                cpa.PointAccountID = 12
                AND
                pv.PeriodID = {0}
                And
                pv.Volume14 = 0
                And
                pv.Volume12 < 500
                And
                cpa.PointBalance > 0
                And
                --c.CustomerID > 40000
                --And
                c.CustomerTypeID = 3
                And
                c.CanLogin = 1
                ORDER BY c.CustomerID", currentPeriod.PeriodID.ToString());

            
            string sqlProcedure = string.Format("GetCustomer");
            var results = context.Query<Common.Api.ExigoOData.Customer>(sql);
            return results.Select(Mapper.Map<Customer>).ToList();

        }

        public static List<Customer> GetCustomer()
        {
            var context = Sql();
            string sqlProcedure = string.Format("GetCustomer");
            return context.Query<Customer>(sqlProcedure).ToList();
        }

        public static List<Customer> GetEnrollerDownline(int EnrollerID, int RankID, int CustomerTypeID, int CustomerStatusID, string searchClause, string OrderbyClause)
        {
            var context = Sql();

            return context.Query<Customer>(string.Format(@"exec GetEnrollersDownlineCustomers {0},{1},{2},{3},'{4}','{5}'", EnrollerID, RankID, CustomerTypeID, CustomerStatusID, searchClause, OrderbyClause)).ToList();
        }
        public static Customer GetCustomerByLoginName(string loginName)
        {
            var customer = new Customer();
            var webalias = "";
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetCustomerByLoginName '{0}'", loginName);
                customer = context.Query<Customer>(sql).FirstOrDefault();
            }
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetCustomerWebAlias {0}", customer.CustomerID);
                webalias = context.Query<string>(sql).FirstOrDefault();
            }
            customer.WebAlias = webalias;
            return customer;
        }
        public static Customer GetCustomerByEmail(string email)
        {
            var customer = new Customer();
            var webalias = "";
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetCustomerByEmail '{0}'", email);
                customer = context.Query<Customer>(sql).FirstOrDefault();
            }
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetCustomerWebAlias {0}", customer.CustomerID);
                webalias = context.Query<string>(sql).FirstOrDefault();
            }
            customer.WebAlias = webalias;
            return customer;
        }
        public static Customer GetEventHost(int eventid)
        {
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetEventHost {0}", eventid);
                return context.Query<Customer>(sql).FirstOrDefault();
            }
        }
        public static CustomerSite GetEventSite(int eventid)
        {
            using (var context = Sql())
            {
                var sql = string.Format(@"Exec GetCustomerSite {0}", eventid);
                return context.Query<CustomerSite>(sql).FirstOrDefault();
            }
        }
        public static IEnumerable<CustomerExtendedDetails> GetCustomerBonusRewardProducts(int customerID)
        {
            return GetCustomerExtendedDetails(customerId: customerID, extendedGroupId: (int)CustomerExtendedGroup.BonusKicker).ToList();
        }
        public static IEnumerable<CustomerExtendedDetails> GetCustomerExtendedDetails(int customerId, int extendedGroupId)
        {
            // get the detail from db with sql proc with mapped field instead of using mapper 
            List<CustomerExtendedDetails> lstItems = new List<CustomerExtendedDetails>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetCustomerExtendedDetail {0},{1}", customerId, extendedGroupId);
                lstItems = context.Query<CustomerExtendedDetails>(sqlProcedure).ToList();
            }
            var items = new List<CustomerExtendedDetails>();

            if (lstItems.Count() > 0)
            {
                if (extendedGroupId == (int)CustomerExtendedGroup.EbRewards) // a temporary fix until I can get the data fixed on EBRewards rewards
                {
                    items = lstItems.Where(s => s.Field4.Equals("100.0000") || s.Field4 == string.Empty).ToList();
                }
                else
                {
                    items = lstItems;
                }
            }

            return items;
        }
        public static IList<LimitedEditionProducts> GetLimitedEditionProducts(DateTime PresentDate)
        {
            #region SQL Call
            var Result = new List<LimitedEditionProducts>();
            using (var Context = Exigo.Sql())
            {

                string SqlProcedure = string.Format("dbo.GetLimitedEditionProducts '{0}'", PresentDate);
                Result = Context.Query<LimitedEditionProducts>(SqlProcedure).ToList();

            }
            return Result;
            #endregion
            #region ODATA call
            //var context = Exigo.CreateODataContext<RewardsContext>(GlobalSettings.Exigo.Api.SandboxID);
            //var limitedEditionProduct = context.LimitedEditionProducts.Where(i => i.StartDate <= PresentDate && i.EndDate >= PresentDate).ToList();
            //return limitedEditionProduct;
            #endregion
        }
        public static bool IsValidCustomer(int customerID, int customerTypeId)
        {
            var count = 0;
            using (var Context = Exigo.Sql())
            {

                string SqlProcedure = string.Format("dbo.GetCustomerByIDandType {0}, {1}", customerID, customerTypeId);
                count = Context.Query(SqlProcedure).Count();
            }
            return (count == 1);
        }
        #region SearchCustomer


        public static IEnumerable<Customer> SearchCustomer(
                                                    String criteria,
                                                    int take = 10,
                                                    int? customerTypeId = null,
                                                    int? customerStatusId = null,
                                                    int? enrollerId = null
        )
        {

            return InternalSearchCustomer(

                                    criteria,
                                    take,

                                    customerTypeId.HasValue ? new int[] { customerTypeId.Value } : null,
                                    customerStatusId.HasValue ? new int[] { customerStatusId.Value } : null,
                                    enrollerId.HasValue ? new int[] { enrollerId.Value } : null

            );

        }


        public static IEnumerable<Customer> SearchCustomer(
                                                    String criteria,
                                                    int take = 10,
                                                    IEnumerable<int> customerTypeId = null,
                                                    IEnumerable<int> customerStatusId = null,
                                                    IEnumerable<int> enrollerId = null
        )
        {

            return InternalSearchCustomer(

                                    criteria,
                                    take,

                                    (customerTypeId == null) ? null : customerTypeId.ToArray(),
                                    (customerStatusId == null) ? null : customerStatusId.ToArray(),
                                    (enrollerId == null) ? null : enrollerId.ToArray()

            );

        }


        private static IEnumerable<Customer> InternalSearchCustomer(

                                                            String criteria,
                                                            int take,

                                                            int[] customerTypeId = null,
                                                            int[] customerStatusId = null,
                                                            int[] enrollerId = null

        )
        {

            var sql = new StringBuilder();

            sql.AppendLine("SELECT TOP ( @top ) c.* ");

            sql.AppendLine("FROM dbo.Customers c ");

            sql.AppendLine("WHERE ");

            if (customerTypeId != null)
            {

                sql.AppendLine("c.CustomerTypeID IN @customerTypeId ");

            }

            if (customerStatusId != null)
            {

                sql.AppendLine("AND c.CustomerStatusID IN @customerStatusId ");

            }

            if (customerStatusId != null)
            {

                sql.AppendLine("AND c.EnrollerID IN @enrollerId ");

            }


            sql.AppendLine("AND     ( ");
            sql.AppendLine("            c.FirstName + ' ' + c.LastName LIKE '%' + @criteria + '%' ");
            sql.AppendLine("            OR ( ");
            sql.AppendLine("                CHARINDEX( '@', @criteria ) > 0 ");
            sql.AppendLine("                AND ");
            sql.AppendLine("                c.Email LIKE '%' + @criteria + '%' ");
            sql.AppendLine("            ) ");
            sql.AppendLine("        ); ");


            using (SqlConnection context = Sql())
            {

                IEnumerable<Customer> results = context.Query<Customer>(
                                                                        sql.ToString(),
                                                                        new
                                                                        {
                                                                            top = take,
                                                                            customerTypeId,
                                                                            customerStatusId,
                                                                            enrollerId,
                                                                            criteria
                                                                        }
                );

                //                IEnumerable<Customer> customers = results.Select(Mapper.Map<Customer>);

                return Array.AsReadOnly(results.ToArray());

            }

        }


        #endregion
        public static List<Customer> SearchCustomer(string criteria, int take)
        {
            var context = Sql();
            string sqlProcedure = string.Format("SearchCustomerByCriteria {0},{1} ", take, criteria);
            return context.Query<Customer>(sqlProcedure).ToList();
            //            return results.Select(Mapper.Map<Customer>).ToList();
        }
        public static List<Customer> SearchCustomer(string criteria, string zipCodeDistance)
        {
            var context = Sql();
            string SqlQuery = string.Format(@"DECLARE @ZipCodeList TABLE (ZipCode varchar(15),Distance float)
                                            INSERT INTO @ZipCodeList(ZipCode,Distance)
                                            SELECT * From (Select {0}) t(col,col2)
                                            SELECT c.* from @ZipCodeList join Customers c on c.MainZip=ZipCode
                                            where c.CustomerTypeID={1} AND c.{2}
                                            order by Distance", zipCodeDistance, CustomerTypes.IndependentStyleAmbassador.ToString(), criteria);
            return context.Query<Customer>(SqlQuery).ToList();
            //            return results.Select(Mapper.Map<Customer>).ToList();
        }
        public static ZipCodesList GetCustomersNearByZip(string zip)
        {
            string link = ConfigurationManager.AppSettings["NearByZipCodes"].ToString();
            int distance = int.Parse(ConfigurationManager.AppSettings["NearByZipCodesDistance"].ToString());
            link = string.Format(link, zip, distance.ToString());
            WebRequest request = WebRequest.Create(
              link);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            responseFromServer = responseFromServer.Replace("zip_code", "ZipCode");
            responseFromServer = responseFromServer.Replace("distance", "Distance");
            responseFromServer = responseFromServer.Replace("state", "State");
            responseFromServer = responseFromServer.Replace("city", "City");
            ZipCodesList listZipCodes = Newtonsoft.Json.JsonConvert.DeserializeObject<ZipCodesList>(responseFromServer);
            listZipCodes.ZipCodes = listZipCodes.ZipCodes.OrderBy(i => i.Distance).ToList();
            // Clean up the streams and the response.
            reader.Close();
            response.Close();
            return listZipCodes;
        }

        public static List<Customer> SearchCustomer(string criteria, int take, int customerTypeId)
        {
            var context = Sql();
            string sqlProcedure = string.Format("SearchCustomerBycustomerTypeId {0},{1},'{2}'", take, customerTypeId, criteria);
            return context.Query<Customer>(sqlProcedure).ToList();
            //           return results.Select(Mapper.Map<Customer>).ToList();
        }

        public static List<Customer> SearchDownline(string criteria, int enrollerId, int take)
        {
            var context = Sql();
            string sqlProcedure = string.Format("SearchDownline {0},{1} ,'{2}'", enrollerId.ToString(), take, criteria);
            var customer = context.Query<Customer>(sqlProcedure).ToList();
            customer.ForEach(s => { s.MainAddress.Address1 = s.MainAddress1;
            s.MainAddress.Address2 = s.MainAddress2;
            s.MainAddress.City = s.MainCity;
            s.MainAddress.Country = s.MainCountry;
            s.MainAddress.Zip = s.MainZip;
            s.MainAddress.State = s.MainState;
            s.MailingAddress.Address1 = s.MailAddress1;
            s.MailingAddress.Address2 = s.MailAddress2;
            s.MailingAddress.City = s.MailCity;
            s.MailingAddress.Country = s.MailCountry;
            s.MailingAddress.Zip = s.MailZip;
            s.MailingAddress.State = s.MailState;
            });
            return customer;
            //           return results.Select(Mapper.Map<Customer>).ToList();

        }

        public static List<CustomerWallItem> GetCustomerRecentActivity(GetCustomerRecentActivityRequest request)
        {
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetCustomerRecentActivity {0},{1},{2}", request.CustomerID, request.Skip, request.Take);
                var wallItems = context.Query<CustomerWallItem>(sqlProcedure).ToList();
                return wallItems;
            }
        }

        public static CustomerStatus GetCustomerStatus(int customerStatusID)
        {
            CustomerStatus customerStatus = new CustomerStatus();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetCustomerStatusByStatusID {0}", customerStatusID);
                customerStatus = context.Query<CustomerStatus>(sqlProcedure).FirstOrDefault();
            }

            if (customerStatus == null) return null;
            return customerStatus;
        }

        public static CustomerType GetCustomerType(int customerTypeID)
        {
            var customerType = GetCustomerByTypes(customerTypeID);
            if (customerType == null) return null;

            return customerType;
        }

        public static void SetCustomerPreferredLanguage(int customerID, int languageID)
        {
            WebService().UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = customerID,
                LanguageID = languageID
            });

            var language = GlobalSettings.Globalization.AvailableLanguages.Where(c => c.LanguageID == languageID).FirstOrDefault();
            if (language != null)
            {
                GlobalUtilities.SetCurrentUICulture(language.CultureCode);
            }
        }

        public static bool IsEmailAvailable(int customerID, string email)
        {
            // Validate the email address
            //return Exigo.OData().Customers
            //    .Where(c => c.CustomerID != customerID)
            //    .Where(c => c.Email == email)
            //    .Count() == 0; 
            var emailCount = new List<int>();
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("EmailValidate {0},'{1}'", customerID, email);
                emailCount = context.Query<int>(sqlProcedure).ToList();
            }
            if (emailCount[0].Equals(0))
                return true;
            else
                return false;

        }

        public static bool IsLoginNameAvailable(int customerID, string loginname)
        {
            // Get the current login name to see if it matches what we passed. If so, it's still valid.
            var currentLoginName = GetCustomer(customerID).LoginName;
            if (loginname.Equals(currentLoginName, StringComparison.InvariantCultureIgnoreCase)) return true;


            // Validate the login name
            return WebService().Validate(new IsLoginNameAvailableValidateRequest
            {
                LoginName = loginname
            }).IsValid;
        }

        public static bool IsLoginNameAvailable(string loginname)
        {
            // Validate the login name
            return WebService().Validate(new IsLoginNameAvailableValidateRequest
            {
                LoginName = loginname
            }).IsValid;
        }

        public static void SendEmailVerification(int customerID, string email)
        {
            // Create the publicly-accessible verification link
            var sep = "&";
            if (!GlobalSettings.Emails.VerifyEmailUrl.Contains("?")) sep = "?";

            var encryptedValues = Security.UrlEncrypt(string.Format("{0}|{1}|{2}",
                customerID,
                email,
                DateTime.Now), GlobalSettings.Encryptions.General.Key, GlobalSettings.Encryptions.General.IV);

            var verifyEmailUrl = GlobalSettings.Emails.VerifyEmailUrl + sep + "token=" + encryptedValues;


            // Send the email
            SendEmail(new SendEmailRequest
            {
                To = new[] { email },
                From = GlobalSettings.Emails.NoReplyEmail,
                ReplyTo = new[] { GlobalSettings.Emails.NoReplyEmail },
                SMTPConfiguration = GlobalSettings.Emails.SMTPConfigurations.Default,
                Subject = "{0} - Verify your email".FormatWith(GlobalSettings.Company.Name),
                Body = @"
                    <p>
                        {1} has received a request to enable this email account to receive email notifications from {1} and your upline.
                    </p>

                    <p> 
                        To confirm this email account, please click the following link:<br />
                        <a href='{0}'>{0}</a>
                    </p>

                    <p>
                        If you did not request email notifications from {1}, or believe you have received this email in error, please contact {1} customer service.
                    </p>

                    <p>
                        Sincerely, <br />
                        {1} Customer Service
                    </p>"
                    .FormatWith(verifyEmailUrl, GlobalSettings.Company.Name)
            });
        }

        public static void OptInCustomer(string token)
        {
            var decryptedToken = Security.Decrypt(token, GlobalSettings.Encryptions.General.Key, GlobalSettings.Encryptions.General.IV);
            var tokenParts = decryptedToken.Split('|');

            var customerID = Convert.ToInt32(tokenParts[0]);
            var email = tokenParts[1];

            OptInCustomer(customerID, email);
        }

        public static void OptInCustomer(int customerID, string email)
        {
            WebService().UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = customerID,
                Email = email,
                SubscribeToBroadcasts = true,
                SubscribeFromIPAddress = GlobalUtilities.GetClientIP()
            });
        }

        public static void OptOutCustomer(int customerID)
        {
            WebService().UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = customerID,
                SubscribeToBroadcasts = false
            });
        }
    }
}