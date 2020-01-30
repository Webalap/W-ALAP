using AutoMapper;
using Common.Api.ExigoOData.Rewards;
using Common.Api.ExigoWebService;
using Common.ModelsEx.Base;
using Common.ModelsEx.Email;
using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using Common.Services;
using Common.ServicesEx.Base;
using ExigoService;
using Ninject;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using IOrder = Common.ModelsEx.Shopping.IOrder;

namespace Common.ServicesEx
{

    /// <summary>
    /// The default implementation of <see cref="IOrderService"/>.
    /// </summary>
    public class OrderService : ServiceBase, IOrderService
    {

        public const decimal DefaultProductCreditPercentage = 10.0M;



        private readonly TraceSource _log = new TraceSource("IhBackoffice", SourceLevels.All);
        private readonly Logger _nlog = LogManager.GetCurrentClassLogger();


        #region Dependencies

        [Inject]
        public IShippingService ShippingService { get; set; }

        [Inject]
        public IEventService EventService { get; set; }

        #endregion

        #region Calculate Order

        /// <summary>
        /// Calculates the order totals for shipping, tax, and totals.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="shipping">Output of shipping.</param>
        /// <param name="tax">Output of tax.</param>
        /// <param name="total">Output of total.</param>
        void IOrderService.CalculateTotals(IOrder order, out decimal subTotal, out decimal shipping, out decimal tax, out decimal total)
        {
            subTotal = 0m;
            shipping = 0m;
            tax = 0m;
            total = 0m;

            // Build the request
            var request = Mapper.Map<CalculateOrderRequest>(order);
            //var request = new CalculateOrderRequest();
            if (order.DiscountsApplied)
            {
                request.Details = applyProductDicounts(order);
            }

            try
            {
                // Get the response.
                var response = Api.CalculateOrder(request);

                if (response == null)
                    return;

                // Set totals.
                shipping = response.ShippingTotal;
                tax = response.TaxTotal;
                total = response.Total;
                subTotal = response.SubTotal;
            }
            catch
            {
                // ignored
            }
        }
        void IOrderService.InternationalShippingCalculateTotals(InternationalShippingRequestModel Shipping, IOrder order, out decimal subTotal, out decimal shipping, out decimal tax, out decimal total)
        {
            subTotal = 0m;
            shipping = 0m;
            tax = 0m;
            total = 0m; 
            InternationalShippingRequestModel requestModel = new InternationalShippingRequestModel();
            requestModel.carrier = Shipping.carrier;
            requestModel.shippingMethod = Shipping.shippingMethod;
            requestModel.securityKey = GlobalSettings.LandedCost.LandedCostKey;
            requestModel.shippingCostTotal = Shipping.shippingCostTotal;// "00.00";
            requestModel.sourceCurrencyCode = "USD";
            requestModel.targetCurrencyCode = Shipping.targetCurrencyCode;
            requestModel.discountTotal = CalculateDisountTotal(order.Products).ToString();//Shipping.discountTotal;// "0.00";
            requestModel.additionalInsuranceTotal = Shipping.additionalInsuranceTotal;// "0.00";
            requestModel.languageCode = "en-us";
            List<LandedCostAddressModel> addressList = new List<LandedCostAddressModel>();
            addressList.Add(new LandedCostAddressModel()
            {
                firstName = "india" ,
                lastName = "Hicks",
                address1 = "123 Vine Street",
                address2 = "",
                city = "Lemoore",
                regionCode = "CA",
                countryCode = "US",
                postalCode = "93245",
                emailAddress = "Account@indiahicks.com",
                nationalIdentificationNumber = "",
                addressType = "shipFrom"

            });
            addressList.Add(new LandedCostAddressModel()
            {
              firstName = order.ShippingAddress.FirstName,
              lastName = order.ShippingAddress.LastName,
              address1 = order.ShippingAddress.Address1,
              address2 = order.ShippingAddress.Address2,
              city = order.ShippingAddress.City,
              regionCode = order.ShippingAddress.State,
              countryCode =order.ShippingAddress.Country,
              postalCode =order.ShippingAddress.Zip,
              emailAddress = order.ShippingAddress.Email,
              nationalIdentificationNumber = "",
              addressType = "ShipTo" 

            });
            requestModel.addresses = addressList;
            List<LandedCostItemModel> itemList = new List<LandedCostItemModel>();
            foreach (Product product in order.Products)
            {
                itemList.Add(new LandedCostItemModel()
                {
                    sku = product.ItemCode,
                    description = product.Description,
                    name = product.Description,
                    price = product.Price.ToString(),
                    quantity = int.Parse(product.Quantity.ToString()),
                    category = product.CategoryName,
                    hsCode = product.Field2,
                    weight = 1,
                    uom = "lbs",
                    countryOfOrigin = product.Field1,
                    autoClassify = "false",

                });
            }
            requestModel.items = itemList;
            InternationalShippingResponseModel response = LandedCostApiCall(requestModel);
            if (response.code != "200")
            {
                return;
            }
            //end code for International Shipping  
            // Set totals.
            shipping = Convert.ToDecimal(response.shippingCostTotal);
            tax = Convert.ToDecimal(response.landedCostTotal);
            total = Convert.ToDecimal(response.grandTotal);
            subTotal = Convert.ToDecimal(response.subTotal)- Convert.ToDecimal(string.IsNullOrEmpty(response.discountTotal)?"0": response.discountTotal);

        }
        InternationalShippingResponseModel IOrderService.InternationalShippingCalculateTotals(decimal ShippingTotal,ShippingAddress ShippingAddress, IEnumerable<IShoppingCartItem> Items) {
            InternationalShippingRequestModel requestModel = new InternationalShippingRequestModel();
            requestModel.carrier = "UPS";
            requestModel.shippingMethod = "2DAY";
            requestModel.securityKey = GlobalSettings.LandedCost.LandedCostKey;
            requestModel.shippingCostTotal = ShippingTotal.ToString();// "00.00";
            requestModel.sourceCurrencyCode = "USD";
            requestModel.targetCurrencyCode = "USD";
            requestModel.discountTotal = CalculateDisountTotal(Items).ToString();// "0.00";
            requestModel.additionalInsuranceTotal =  "0.00";
            requestModel.languageCode = "en-us";
            List<LandedCostAddressModel> addressList = new List<LandedCostAddressModel>();
            addressList.Add(new LandedCostAddressModel()
            {
                firstName = "india",
                lastName = "Hicks",
                address1 = "123 Vine Street",
                address2 = "",
                city = "Lemoore",
                regionCode = "CA",
                countryCode = "US",
                postalCode = "93245",
                emailAddress = "Account@indiahicks.com",
                nationalIdentificationNumber = "",
                addressType = "shipFrom"

            });
            addressList.Add(new LandedCostAddressModel()
            {
                firstName =ShippingAddress.FirstName,
                lastName = ShippingAddress.LastName,
                address1 = ShippingAddress.Address1,
                address2 =ShippingAddress.Address2,
                city = ShippingAddress.City,
                regionCode = ShippingAddress.State,
                countryCode = ShippingAddress.Country,
                postalCode = ShippingAddress.Zip,
                emailAddress = ShippingAddress.Email,
                nationalIdentificationNumber = "",
                addressType = "ShipTo"

            });
            requestModel.addresses = addressList;
            List<LandedCostItemModel> itemList = new List<LandedCostItemModel>();
            foreach (IShoppingCartItem product in Items)
            {
                itemList.Add(new LandedCostItemModel()
                {
                    sku = product.ItemCode,
                    description = product.ItemDescription,
                    name = product.ItemDescription,
                    price = product.Price.ToString(),
                    quantity = int.Parse(product.Quantity.ToString()),
                    category = product.Category,
                    hsCode = product.Field2,
                    weight = 1,
                    uom = "lbs",
                    countryOfOrigin = product.Field1,
                    autoClassify = "false",

                });
            }
            requestModel.items = itemList;
            return LandedCostApiCall(requestModel);
        }
        private InternationalShippingResponseModel LandedCostApiCall(InternationalShippingRequestModel request) {

            InternationalShippingResponseModel response = new InternationalShippingResponseModel();
            try
            {
                var LandedCostjson = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request);
                string url = GlobalSettings.LandedCost.LandedCostUrl + "calculator";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json; charset=utf-8";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(LandedCostjson);
                    streamWriter.Flush();
                    streamWriter.Close();
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        response = (InternationalShippingResponseModel)Newtonsoft.Json.JsonConvert.DeserializeObject(result, typeof(InternationalShippingResponseModel));
                    }
                }
            }
            catch (Exception ex)
            {
                       
            }

            return response;
        }
        private decimal CalculateDisountTotal(IEnumerable<IShoppingCartItem> items)
        {
            decimal discountTotal = 0;
            foreach (IShoppingCartItem item in items.Where(i=>i.Discounts.Count>0))
            {
                discountTotal = discountTotal + (decimal)((item.Price - (item.PriceEachOverride == null ? 0 : item.PriceEachOverride)) * item.Quantity);
            }

            return discountTotal;

        }
        private decimal CalculateDisountTotal(List<Product> items)
        {
             decimal discountTotal= 0;
            foreach (Product item in items.Where(i => i.Discounts.Count > 0))
            {
                discountTotal = discountTotal + (decimal)((item.Price - (item.PriceEachOverride == null ? 0 : item.PriceEachOverride)) * item.Quantity);
            }
            return discountTotal;
        }          


        #endregion

        #region Process Shop Order

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        OrderConfirmation IOrderService.PlaceOrder(Shop shop, string giftMessage, int customerID)
        {
            // Products inventory Status 
            var productAvailable = shop.Order.Products.ToList();
            var productUnAvailable = new List<Product>();

            // comment split order modcle 
            //   var productAvailable = shop.Order.Products.Where(i => string.IsNullOrEmpty(i.InventoryStatus) || (!string.IsNullOrEmpty(i.InventoryStatus) && i.InventoryStatus.Equals(InventoryStatusType.Available.ToString()))).ToList();
            //   var productUnAvailable = shop.Order.Products.Where(i => (!string.IsNullOrEmpty(i.InventoryStatus) && !i.InventoryStatus.Equals(InventoryStatusType.Available.ToString()))).ToList();

            //  Place Order for Available Items
            // false=simple order 
            shop.Order.Products = productAvailable;
            List<CreditCard> lstCreditCard = new List<CreditCard>();
            List<int> deletedCards = new List<int>();
            if (shop.CreditCardList.Count > 1 && productUnAvailable.Count > 0)
            {
                decimal total = 0M;
             //public IOrderConfiguration CustomerOrderConfiguration = Identity.Current.Market.Configuration.CustomerOrders;
             var response = new OrderCalculationResponse();
                if (!shop.IsInternationalShipping)
                {
                    response = Exigo.CalculateOrder(new OrderCalculationRequest
                    {
                        Configuration = shop.Order.Configuration,
                        Items = productAvailable.Select(s => new ShoppingCartItem(s)).ToList(),
                        Address = shop.Customer.MailingAddress,
                        ShipMethodID = shop.ShipMethodID,
                        ReturnShipMethods = false
                    });
                    total= response.Total;
                }
                else
                {
                    total = shop.Total;
                } 
                 
                decimal remainingAmount = 0;

                int counter = 0;
                foreach (var item in shop.CreditCardList)
                {
                    if (total == item.Amount)
                    {
                        lstCreditCard.Add(item);
                        deletedCards.Add(item.Id);
                        //shop.CreditCardList.Remove(item);
                        break;
                    }
                    else if (total < item.Amount)
                    {
                        item.Amount = item.Amount - total;
                        var card = new CreditCard
                        {
                            CardNumber = item.CardNumber,
                            NameOnCard = item.NameOnCard,
                            Amount = total,
                            BillingAddress = item.BillingAddress,
                            CVV = item.CVV,
                            ExpirationMonth = item.ExpirationMonth,
                            ExpirationYear = item.ExpirationYear,
                        };
                        lstCreditCard.Add(card);
                        break;
                    }
                    else
                    {
                        if (counter > 0)
                        {
                            if (remainingAmount < item.Amount)
                            {
                                item.Amount = item.Amount - remainingAmount;
                                var card = new CreditCard
                                {
                                    CardNumber = item.CardNumber,
                                    NameOnCard = item.NameOnCard,
                                    Amount = remainingAmount,
                                    BillingAddress = item.BillingAddress,
                                    CVV = item.CVV,
                                    ExpirationMonth = item.ExpirationMonth,
                                    ExpirationYear = item.ExpirationYear,
                                };
                                lstCreditCard.Add(card);
                                break;
                            }
                            else
                            {
                                remainingAmount = remainingAmount - item.Amount;
                                lstCreditCard.Add(item);
                                deletedCards.Add(item.Id);
                                //shop.CreditCardList.Remove(item);
                            }
                        }
                        else
                        {
                            remainingAmount = total - item.Amount;
                            lstCreditCard.Add(item);
                            deletedCards.Add(item.Id);
                            //shop.CreditCardList.Remove(item);
                        }
                    }

                    counter++;
                }
            }
            shop.CreditCardList.RemoveAll(s => deletedCards.Contains(s.Id));
            List<CreditCard> lstRemainingCard = shop.CreditCardList;
            if (lstCreditCard.Count > 0)
            {
                shop.CreditCardList = lstCreditCard;
            }
            OrderConfirmation PlaceOrders = SplitOrders(shop, giftMessage, false, customerID);

            // Place order for Unavailable items With SHippping amount zero
            // true=back  order 
            if (productUnAvailable.Count > 0)
            {
                shop.CreditCardList = lstRemainingCard;
                shop.Order.Products = productUnAvailable;
                OrderConfirmation backOrder = SplitOrders(shop, giftMessage, true, customerID);

                PlaceOrders.Order.Details = PlaceOrders.Order.Details.Union(backOrder.Order.Details);
                PlaceOrders.Order.Total = PlaceOrders.Order.Total + backOrder.Order.Total;
                PlaceOrders.Order.TaxTotal = PlaceOrders.Order.TaxTotal + backOrder.Order.TaxTotal;
                PlaceOrders.Order.Subtotal = PlaceOrders.Order.Subtotal + backOrder.Order.Subtotal;

            }


            return PlaceOrders;

        }

        /// <returns></returns>
        private OrderConfirmation SplitOrders(Shop shop, string giftMessage, bool IsSplited, int custID)
        {

            if ((shop.Order.Products == null) || (shop.Order.Products.Count < 1))
            {

                _log.TraceEvent(TraceEventType.Warning, 1, "IOrderService.PlaceOrder( Shop, String ): Entering method and shop.Order.Products is {0}.", (shop.Order.Products == null) ? "null" : "empty");

            }
            else
            {

                _log.TraceEvent(TraceEventType.Information, 1, "IOrderService.PlaceOrder( Shop, String ): Entering method with {0} products.", shop.Order.Products.Count);

            }


            string orderType = string.Empty;
            //TODO: Build the capabilty to award 10% PRV for each Retail, Personal or GT Order.
            bool nonRewardOrder = true;
            bool isCommissionableOrder = true;

            var confirmation = new OrderConfirmation();

            UpdateCustomerResponse resultLoginUpdate = null;

            var updateCustomerLoginRequest = new UpdateCustomerRequest();

            TransactionalResponse result;
            CreditCard firstCard = shop.CreditCardList.FirstOrDefault();
            try
            {
                var requests = new List<ApiRequest>();
                var requestsUpdateLogin = new List<ApiRequest>();

                // TODO: Need to figure out the flow for checking out as guest.
                // Check for duplicate customers
                //Customer _customer = null;

                // _customer = Exigo.GetCustomerByEmail(shop.Customer.Email);


                if (shop.Customer.CustomerID == 0)
                {
                    var createCustomerRequest = new CreateCustomerRequest(shop.Customer)
                    {
                        DefaultWarehouseID =shop.Order.Configuration.WarehouseID,
                        PayableType = PayableType.PaymentCard
                    };
                    if (string.IsNullOrEmpty(shop.Customer.Password))
                        createCustomerRequest.LoginName = string.Empty;
                    else
                        createCustomerRequest.CanLogin = true;

                    requests.Add(createCustomerRequest);
                }
                //else
                //{
                //    // Update customer
                //    //shop.Customer.CustomerID = _customer.CustomerID;
                //    //custID = _customer.CustomerID;
                //    var UpdateCustomerRequest = new UpdateCustomerRequest(shop.Customer)
                //    {
                //        DefaultWarehouseID = shop.Order.Configuration.WarehouseID,
                //        PayableType = PayableType.PaymentCard
                //    };
                //    if (string.IsNullOrEmpty(shop.Customer.Password))
                //        UpdateCustomerRequest.LoginName = string.Empty;
                //    else {
                //        UpdateCustomerRequest.CanLogin = true;
                //        //UpdateCustomerRequest.LoginName = shop.Customer.LoginName;
                //    }


                //    requests.Add(UpdateCustomerRequest);

                //}

                var shippingMethodId = shop.Order.ShippingMethodId != 0 ?
                    shop.Order.ShippingMethodId :
                    shop.Order.Configuration.DefaultShipMethodID;


                var customerID = shop.Customer.CustomerID; //assigns an order from a non-Host SA to the Host

                // Override the customerID if SA shopping on behalf of the Host
                if (shop.Order.ShopperIsEventSa && !shop.Order.ShopperIsHost)
                {
                    //Get the customer Extended 2 of shop.Order.EventId.Value; 
                    customerID = EventService.GetEventDetails((int)shop.Order.EventId).Host.CustomerID;
                }

                var createOrderRequest = new CreateOrderRequest(
                    customerID,
                    shop.Order.Configuration,
                    giftMessage,
                    shippingMethodId,
                    shop.Order);


                //if Back Order Than Shipping Amout Must b Zero
                if (IsSplited)
                {
                    createOrderRequest.ShippingAmountOverride = 0;

                }
                if (shop.IsInternationalShipping)
                {  
                    createOrderRequest.ShippingAmountOverride = shop.InternationalShipping;
                    decimal productsubtotal = shop.Order.Products.Sum(product => (product.Price * product.Quantity)) - CalculateDisountTotal(shop.Order.Products);
                    createOrderRequest.TaxRateOverride = (shop.InternationalShippingTax/productsubtotal)*100; //%age of subtotal      
                    createOrderRequest.WarehouseID = Warehouses.Canada;
                }
                if (shop.Order.EventId.HasValue)
                {
                    // This is required for placing an order under an event.
                    orderType = "Get Together";
                    createOrderRequest.Other14 = shop.Order.EventId.Value.ToString();

                    try
                    {
                        // here add code to check event is charitable or not 
                        CharityService charity = new CharityService();
                        List<CharityModel> charityDetail = charity.GetCharityName(int.Parse(shop.Order.EventId.Value.ToString())).ToList();
                        if (charityDetail.Count > 0)
                        {
                            //   decimal chatitycontribution = ((decimal.Parse(string.IsNullOrEmpty(charityDetail.FirstOrDefault().CharityContribution) ? "5" : charityDetail.FirstOrDefault().CharityContribution)) + (decimal.Parse(string.IsNullOrEmpty(charityDetail.FirstOrDefault().IHContribution) ? "0" : charityDetail.FirstOrDefault().IHContribution)));
                            decimal chatitycontribution = decimal.Parse(string.IsNullOrEmpty(charityDetail.FirstOrDefault().CharityContribution) ? "5" : charityDetail.FirstOrDefault().CharityContribution);
                            createOrderRequest.Other17 = chatitycontribution == 0 ? "5" : chatitycontribution.ToString();
                        }
                        //this needs to stay commenetd. It is a bug that creates wrong charity contribution.
                        //else
                        //{
                        //    //// here to check charity flag from my local database if query response late 
                        //    bool IsCharity = false;
                        //    IsCharity = charity.CheckCharityEvent(int.Parse(shop.Order.EventId.Value.ToString()));
                        //    if (IsCharity)
                        //    {
                        //        createOrderRequest.Other17 = "5";
                        //    }
                        //}
                        //// end 
                    }
                    catch (Exception)
                    {
                    }

                    if (shop.Order.DiscountsApplied && shop.Order.ActiveAwards != null && shop.Order.ActiveAwards.Count > 0)
                    {
                        createOrderRequest.Details = applyProductDicounts(shop.Order);
                        //Host Orders are not commissionable; Exigo created a customer field set when Other15 is set to 1, for "Not Commissionsable"
                        createOrderRequest.Other15 = "1";
                        isCommissionableOrder = false;

                    }
                    else if (shop.Order.DiscountsApplied)
                    {
                        if (shop.Order.ShopperIsHost)
                        {
                            //Host Orders are not commissionable; Exigo created a customer field set when Other15 is set to 1, for "Not Commissionsable"
                            createOrderRequest.Other15 = "1";
                            isCommissionableOrder = false;
                        }
                        createOrderRequest.Details = applyProductDicounts(shop.Order);
                    }

                }
                else if (shop.Order.DiscountsApplied && shop.Order.ActiveAwards != null && shop.Order.ActiveAwards.Count > 0)
                {

                    createOrderRequest.Details = applyProductDicounts(shop.Order);

                    //Host Orders are not commissionable; Exigo created a customer field set when Other15 is set to 1, for "Not Commissionsable"
                    createOrderRequest.Other15 = "1";
                    nonRewardOrder = false;
                    isCommissionableOrder = false;

                }
                else if (shop.Order.DiscountsApplied)
                {

                    createOrderRequest.Details = applyProductDicounts(shop.Order);

                }

                if (!shop.Order.EventId.HasValue)
                {
                    orderType = "Retail";
                    //Marking the order with the ID of the SA on whose replicated site the order was placed
                    //createOrderRequest.Other16 = shop.Customer.CustomerTypeID.Equals(CustomerTypes.IndependentStyleAmbassador) ? shop.Customer.CustomerID.ToString() : shop.Customer.EnrollerID.ToString();
                    createOrderRequest.Other16 = shop.CorporateSiteOwner.Length > 0 ? shop.CorporateSiteOwner :
                        shop.Customer.CustomerTypeID.Equals(CustomerTypes.IndependentStyleAmbassador) ? shop.Customer.CustomerID.ToString() : shop.Customer.EnrollerID.ToString();
                }

                // promocode orders are commissionable 
                //if (shop.Order.DiscountsApplied)
                //{
                //    foreach (var discount in from product in shop.Order.Products from discount in product.Discounts where discount.DiscountType == DiscountType.PromoCode select discount)
                //    {
                //        createOrderRequest.Other15 = "1";
                //        isCommissionableOrder = false;
                //    }
                //}
                requests.Add(createOrderRequest);
                //if (shop.CreditCardList.Count == 1 && shop.IsShippingPaidByAmbassador != true)
                //{
                var creditCard = shop.CreditCardList.FirstOrDefault();
                creditCard.IsShippingCard = true;
                creditCard.Amount = (shop.CreditCardList.Count == 1 && shop.IsShippingPaidByAmbassador != true) ? shop.Total : creditCard.Amount;
                    //shop.CreditCardList.FirstOrDefault().Amount = shop.Total;
                    var chargeCcTokenRequest = new ChargeCreditCardTokenRequest(creditCard);
                chargeCcTokenRequest.MaxAmount = creditCard.Amount;
                    requests.Add(chargeCcTokenRequest);
                //After processing first card with order request deleting it from the list.
                    
                //var chargeCcTokenRequest = new ChargeCreditCardTokenRequest(shop.CreditCard);
                    //requests.Add(chargeCcTokenRequest);

                    //Only store Credit Card Details for non event purchasing
                    if (shop.Order.EventId == null)
                    {
                        var setAccountCcTokenRequest = new SetAccountCreditCardTokenRequest(
                            shop.CreditCardList.FirstOrDefault(),
                            chargeCcTokenRequest.CreditCardToken,
                            shop.Customer.CustomerID);
                        requests.Add(setAccountCcTokenRequest);
                    }
                //}
                


                if (shop.Order.EventId.HasValue &&
                    shop.Order.DiscountsApplied)
                {
                    var rewardAccounts = this.CalculateHostPointAccounts(shop.Order);
                    if (rewardAccounts != null)
                    {
                        if (rewardAccounts.Any(rewardAccount => !string.IsNullOrWhiteSpace(rewardAccount.AppliedItemCode) &&
                                                                rewardAccount.Balance < 0))
                        {
                            throw new ApplicationException("You've exceeded your rewards balance");
                        }
                    }

                    //var hostSpecial = (from p in shop.Order.Products
                    //                   from d in p.Discounts
                    //                   where d.DiscountType == DiscountType.HostSpecial
                    //                   select d as HostSpecialDiscount).FirstOrDefault();

                    //if (null != hostSpecial)
                    //{
                    //    var eventId = shop.Order.EventId.Value;

                    //    // Retrieving the Party Details as a quick fix to discover the CustomerExtendedID - a more efficient way most likely exists...

                    //    var partyDetailsRequest = Api.GetCustomerExtended(new GetCustomerExtendedRequest
                    //    {
                    //        CustomerID = eventId,
                    //        ExtendedGroupID = (int)CustomerExtendedGroup.PartyDetails, // Party Details
                    //    });

                    //    var customerExtendedId = partyDetailsRequest.Items[0].CustomerExtendedID;

                    //    var hostSpecialRequst = new UpdateCustomerExtendedRequest
                    //    {
                    //        CustomerID = eventId,
                    //        CustomerExtendedID = customerExtendedId,
                    //        ExtendedGroupID = (int)CustomerExtendedGroup.PartyDetails, // Party Details
                    //        Field10 = 1.ToString() // True
                    //    };

                    //    requests.Add(hostSpecialRequst);
                    //}
                }

                if (shop.Order.ActiveAwards != null)
                {
                    foreach (var reward in shop.Order.ActiveAwards)
                    {
                        reward.VerifyOrderIsValid(shop.Order);
                    }
                }

                result = Api.ProcessTransaction(new TransactionalRequest
                {
                    TransactionRequests = requests.ToArray()
                });

                if (custID == 0)
                {
                    CreateCustomerResponse custResponse = result.TransactionResponses.FirstOrDefault(s => s is CreateCustomerResponse) as CreateCustomerResponse;
                    custID = custResponse.CustomerID;
                }

                CreateOrderResponse response = result.TransactionResponses.FirstOrDefault(tr => tr is CreateOrderResponse) as CreateOrderResponse;
                int declineOrderAmountID = 0;
                bool isValid = true;
                string orderStatus = string.Empty;
                StringBuilder emailContent = new StringBuilder();
                // Charging shipping amount from Ambassador card if selected.
                int counter = 0;
                if (shop.IsShippingPaidByAmbassador)
                {
                    Api.ChangeOrderStatus(new ChangeOrderStatusRequest()
                    {
                        OrderID = response.OrderID,
                        OrderStatus = OrderStatusType.Pending

                    });
                    
                    try
                    {
                        var card = shop.ShippingCard;
                        card.Amount = shop.ShippingAmount;
                        ChargeCreditCardTokenRequest req = new ChargeCreditCardTokenRequest(card);
                        req.MaxAmount = shop.ShippingAmount;
                        req.OrderID = response.OrderID;
                        ChargeCreditCardResponse res = Api.ChargeCreditCardToken(req);
                        orderStatus = res.Result.Status == ResultStatus.Success ? OrderStatusType.Accepted.ToString() : OrderStatusType.Pending.ToString();
                    }
                    catch (Exception ex)
                    {
                        if (counter == 0)
                        {
                            confirmation.Result.Errors.Add("Your order is pending due to following invalid card(s).");
                        }

                        string cardNumber = string.Format("***********{0}", shop.ShippingCard.CardNumber.Substring(shop.ShippingCard.CardNumber.Length - 4));

                        emailContent.Append("<p style='font-size:15px; padding: 0px 10px; height: auto;'>");
                        emailContent.Append(string.Format("<strong style='color: green'>${0:0.00}</strong> has been declined due to this {1} invalid card.", shop.ShippingCard.Amount, cardNumber));
                        emailContent.Append("</p><hr style='background: #ccc;  height: 1px;  border: none; display:block;' />");

                        using (var contextsql = Exigo.Sql())
                        {
                            var sql = string.Format(@"Exec InsertDeclineOrderAmount {0},{1},'{2}',{3},'{4}','{5}','{6}'", response.OrderID, custID, cardNumber, shop.ShippingCard.Amount, shop.SiteType, orderType, shop.ShippingCard.NameOnCard);
                            var Id = contextsql.Query<SavedCart>(sql).ToList();

                            declineOrderAmountID = Convert.ToInt32(Id.FirstOrDefault().ID);
                        }
                        confirmation.Result.Errors.Add("The credit card ending in " + cardNumber + " being used to cover shipping is invalid please <a id='btnAddNewPayment' class='DeclineCardLi' data-paymentId=" + declineOrderAmountID + " data-OrderId=" + response.OrderID + " data-amount=" + shop.ShippingCard.Amount + " data-customerId=" + custID + " onclick='PayWithNewShippingCard(this)'>click here</a> to correct.");
                        counter++;
                        orderStatus = OrderStatusType.Pending.ToString();
                    }
                    
                }

                if (shop.CreditCardList.Count > 1)
                {
                    if (response.Result.Status == ResultStatus.Success)
                    {
                        int paymentID = 0;
                        
                        //foreach (var item in shop.CreditCardList)
                        //{
                        var secondCard = shop.CreditCardList.FirstOrDefault(s => s.IsShippingCard == false);
                        if (secondCard != null)
                        {
                            Api.ChangeOrderStatus(new ChangeOrderStatusRequest()
                            {
                                OrderID = response.OrderID,
                                OrderStatus = OrderStatusType.Pending
                            });
                            try
                            {
                                var chargeCardTokenRequest = new ChargeCreditCardTokenRequest(secondCard);
                                chargeCardTokenRequest.OrderID = response.OrderID;
                                chargeCardTokenRequest.MaxAmount = secondCard.Amount;
                                ChargeCreditCardResponse res = Api.ChargeCreditCardToken(chargeCardTokenRequest);
                                paymentID = res.PaymentID;
                            }
                            catch (Exception ex)
                            {
                                isValid = false;
                                paymentID = 0;
                                if (counter == 0)
                                {
                                    confirmation.Result.Errors.Add("Your order is pending due to following invalid card(s).");
                                }

                                string cardNumber = string.Format("***********{0}", secondCard.CardNumber.Substring(secondCard.CardNumber.Length - 4));

                                emailContent.Append("<p style='font-size:15px; padding: 0px 10px; height: auto;'>");
                                emailContent.Append(string.Format("<strong style='color: green'>${0:0.00}</strong> has been declined due to this {1} invalid card.", secondCard.Amount, cardNumber));
                                emailContent.Append("</p><hr style='background: #ccc;  height: 1px;  border: none; display:block;' />");

                                using (var contextsql = Exigo.Sql())
                                {
                                    var sql = string.Format(@"Exec InsertDeclineOrderAmount {0},{1},'{2}',{3},'{4}','{5}','{6}'", response.OrderID, custID, cardNumber, secondCard.Amount, shop.SiteType, orderType, secondCard.NameOnCard);
                                    var Id = contextsql.Query<SavedCart>(sql).ToList();

                                    declineOrderAmountID = Convert.ToInt32(Id.FirstOrDefault().ID);
                                }
                                confirmation.Result.Errors.Add(string.Format("The credit card ending in " + cardNumber + " being used is invalid. Please click <a id='btnAddNewPayment' class='DeclineCardLi' data-paymentId=" + declineOrderAmountID + " data-OrderId=" + response.OrderID + " data-amount=" + secondCard.Amount + " data-customerId=" + custID + " onclick='PayWithNewCard(this)'>here</a> to correct.", secondCard.Amount.ToString("c"), cardNumber));
                                //confirmation.Result.Errors.Add(string.Format("{0} payment for following card {1} is declined due to invalid card details. <br /><a id='btnAddNewPayment' class='DeclineCardLi' data-paymentId=" + declineOrderAmountID + " data-OrderId=" + response.OrderID + " data-amount=" + secondCard.Amount + " data-customerId=" + custID + " onclick='PayWithNewCard(this)'>Click here to pay this amount with other card.</a>", secondCard.Amount.ToString("c"), cardNumber));
                                counter++;
                            }
                            confirmation.lstPayment.Add(new Payment()
                            {
                                CreditCardNumber = secondCard.CardNumber,
                                ExpirationMonth = secondCard.ExpirationMonth,
                                ExpirationYear = secondCard.ExpirationYear,
                                CreditCardTypeID = (int)secondCard.TypeOfCard,
                                AuthorizationCode = secondCard.CVV,
                                Amount = secondCard.Amount,
                                PaymentID = paymentID,
                                BillingAddress = secondCard.BillingAddress,
                                BillingName = secondCard.NameOnCard,
                                DeclineOrderAmountID = isValid ? 0 : declineOrderAmountID
                            });

                        }

                        //}
                        if (confirmation.lstPayment.Any(s => s.DeclineOrderAmountID > 0) || orderStatus == "Pending")
                        {
                            //email to ambassador
                            var email = new PendingOrderNotificationEmail();
                            email.Subject = "Pending Order Notification";
                            email.CardDetails = emailContent.ToString();
                            email.PendingOrderUrl = shop.SiteUrl;
                            email.EmailHeading = string.Format("Dear {0}, <br /> Order # {1} for {2} {3} is pending due to the following payment failures.", shop.AmbassadorName, response.OrderID, shop.Customer.FirstName, shop.Customer.LastName);

                            string EmailTemplate = @"/Content/templates/IH_Training-Email_1 pendingOrder.html";
                            EmailService.SendEmail(email: shop.AmbassadorEmail, subject: email.Subject, templateName: EmailTemplate, liveObject: email);

                            //email to customer
                            var email2customer = new PendingOrderNotificationEmail();
                            email2customer.Subject = "Pending Order Notification";
                            email2customer.CardDetails = emailContent.ToString();
                            email2customer.PendingOrderUrl = shop.SiteUrl;
                            email2customer.EmailHeading = string.Format("Dear {0}, <br /> Order # {1} is pending due to the following payment failures.", shop.Customer.FirstName, response.OrderID);

                            string EmailTemplate2Customer = @"/Content/templates/IH_Training-Email_1 pendingOrder.html";
                            EmailService.SendEmail(email: shop.Customer.Email, subject: email2customer.Subject, templateName: EmailTemplate2Customer, liveObject: email2customer);

                        }

                        Api.ChangeOrderStatus(new ChangeOrderStatusRequest()
                        {
                            OrderID = response.OrderID,
                            OrderStatus = (isValid == false || orderStatus == "Pending") ? OrderStatusType.Pending : OrderStatusType.Accepted
                        });
                    }
                }
                if (response == null)
                {

                    _log.TraceEvent(TraceEventType.Error, 1, "There was no CreateOrderResponse.");

                }
                else
                {

                    _log.TraceEvent(TraceEventType.Information, 1, "CreateOrderResponse.Result.TransactionKey = {0}", response.Result.TransactionKey);
                    _log.TraceEvent(TraceEventType.Information, 1, "CreateOrderResponse.Result.Reponse = {0}", response.Result.Status);

                    if ((response.Result.Errors != null) && (response.Result.Errors.Length > 0))
                    {

                        _log.TraceEvent(TraceEventType.Error, 1, "CreateOrderResponse.Result.Errors.Length = {0}", response.Result.Errors.Length);

                        for (int i = 0; i < response.Result.Errors.Length; i++)
                        {

                            _log.TraceEvent(TraceEventType.Error, 1, "CreateOrderResponse.Result.Errors[{0}] = {1}", i, response.Result.Errors[i]);

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid TERMINALID field\n" || ex.Message == "Decline\n" || ex.Message == "An error occurred during processing. Call Mer...\n" || ex.Message == "Invalid or Misconfigured Terminal.\n" || ex.Message == "The transaction has been declined because of ...\n" || ex.Message == "This transaction has been declined.\n" || ex.Message == "The credit card number is invalid.\n")
                {
                    string cardNumber = string.Format("***********{0}", firstCard.CardNumber.Substring(firstCard.CardNumber.Length - 4));
                    confirmation.Result.Errors.Add("Unable to process order, the credit card ending in " + cardNumber + " being used is invalid. Please click <a onclick='GoToPayment()'>here</a> to correct.");
                }
                //else if (ex.Message == "The transaction has been declined because of ...\n")
                //{
                    
                //    confirmation.Result.Errors.Add(string.Format("The credit card ending in " + cardNumber + " being used is invalid. Please click <a onclick='GoToPayment()'>here</a> to correct."));
                //    //confirmation.Result.Errors.Add("The transaction has been declined due to invalid card details.");
                //}
                else
                {
                    confirmation.Result.Errors.Add(ex.Message);
                }


                confirmation.Result.Status = Status.Failure;

                return confirmation;

            }

            try
            {
                if (!string.IsNullOrEmpty(shop.Customer.Password) && shop.Customer.CustomerID != 0)
                {
                    updateCustomerLoginRequest.LoginPassword = shop.Customer.Password;
                    updateCustomerLoginRequest.CanLogin = true;
                    updateCustomerLoginRequest.LoginName = shop.Customer.LoginName;
                    updateCustomerLoginRequest.CustomerID = shop.Customer.CustomerID;
                    resultLoginUpdate = Api.UpdateCustomer(updateCustomerLoginRequest);
                }
            }
            catch (Exception ex)
            {
                confirmation.Result.Errors.Add("A problem occured when creating your login credentials.  Please call customer service.");
                confirmation.Result.Status = Status.Failure;
                return confirmation;
            }

            if (shop.Order.EventId.HasValue && (shop.Order.ShopperIsHost || shop.Order.ShopperIsEventSa))
            {
                ProcessPointAccountTransactions(shop, confirmation);
            }

            if (shop.Order.ActiveAwards != null)
            {
                foreach (var reward in shop.Order.ActiveAwards)
                {
                    reward.OrderPostProcessing(shop.Order);
                }
            }

            ProcessShopResponse(result, shop, confirmation);

            int newOrderId = confirmation.Order.OrderID;
            // Send Email to Customer and Enroller
            GenrateAutoEmails(shop, newOrderId);

            if (!nonRewardOrder || (result.Result.Status != ResultStatus.Success) || !isCommissionableOrder)
            {
                return confirmation;
            }

            int? ambassadorId = GetAmbassadorId(shop, newOrderId);
            
            if (ambassadorId < 999)
            {
                return confirmation; // don't want to assign any product credits for corporate accounts
            }


            bool isActive = Exigo.IsAmbassadorActive(ambassadorId.Value);

            decimal orderTotal = 0;
            //if (shop.Order.Products.Any(s => s.BusinessVolumeEachOverride != null))
            //    orderTotal = shop.Order.Products.Sum(product => (product.BusinessVolumeEachOverride.Value * product.Quantity));
            //else
            // BV include sum of Quantity*BvEach
           // orderTotal = confirmation.Order.Details.Sum(product => (product.BV * product.Quantity));
            orderTotal = confirmation.Order.Details.Sum(product => (product.BV));
            decimal creditAmount = orderTotal * (DefaultProductCreditPercentage / 100M) /* e.g. 20% / 100 = 0.20 */;

            var resultPointAccount = Api.CreatePointTransaction(
                new CreatePointTransactionRequest
                {
                    CustomerID = ambassadorId.Value,
                    PointAccountID = isActive ? PointAccounts.ProductCredit : PointAccounts.PendingProductCredit,
                    TransactionType = PointTransactionType.Adjustment,
                    Reference = "For Order: " + newOrderId,
                    Amount = creditAmount
                }
            );
            return confirmation;
        }

        private static void GenrateAutoEmails(Shop shop, int newOrderId)
        {
            string CustomerEmail = string.Empty;
            var ordersDetails = new InvoiceDetail(getOrderDetail(newOrderId).Orders.FirstOrDefault());
            if (shop.Order.EventId.HasValue)
            {
                // mechanism for event creater id or event enrollerid 
                var eventSponsor = Exigo.GetCustomersEnroller(int.Parse(shop.Order.EventId.ToString()));
                CustomerEmail = eventSponsor.Email;                
            }
            else
            {
                // mechanism for other16
                CustomerEmail = ordersDetails.Other16 != string.Empty ? Exigo.GetCustomer(int.Parse(ordersDetails.Other16)).Email : string.Empty;
            }

            string subject = string.Empty;
            string EmailTemplate = string.Empty;
            string emailRecipient = string.Empty;
            if (!string.IsNullOrEmpty(CustomerEmail) && (CustomerEmail != "contact@indiahicks.com"))
            {
                subject = "You have a new customer order";
                EmailTemplate = @"/Content/templates/IH_Training-Email_InvoiceModal.html";
                emailRecipient = CustomerEmail;
                EmailService.SendEmail(email: emailRecipient, subject: subject, templateName: EmailTemplate, liveObject: ordersDetails);
            }
            //Order email
            if (shop.Customer != null)
            {
                subject = "Thank you for your India Hicks order";
                EmailTemplate = @"/Content/templates/IH_Training-Email_InvoiceModal-Customer.html";
                emailRecipient = shop.Customer.Email;
                EmailService.SendEmail(email: emailRecipient, subject: subject, templateName: EmailTemplate, liveObject: ordersDetails);
            }

        }

        protected void ProcessPointAccountTransactions(Shop shop, OrderConfirmation confirmation)
        {
            try
            {
                var pointAccounts = CalculateHostPointAccounts(shop.Order);

                foreach (var pointAccount in pointAccounts)
                {
                    var createPointTransactionRequest = new CreatePointTransactionRequest
                    {
                        CustomerID = shop.Order.EventId.Value,
                        PointAccountID = pointAccount.PointAccountID,
                        // Should be Redemption?  Using Adjustment per Tom.
                        TransactionType = PointTransactionType.Adjustment,
                        // Amount needs to be negative here.
                        Amount = -pointAccount.AppliedAmount,
                        Reference = string.Format("Applied Item Code: {0}", pointAccount.AppliedItemCode)
                    };

                    var response = Api.CreatePointTransaction(createPointTransactionRequest);

                    if (!ResultStatus.Failure.Equals(response.Result.Status))
                    {
                        ProcessErrors(response, confirmation);
                    }
                }
            }
            catch (Exception ex)
            {
                confirmation.Result.Errors.Add(ex.Message);
            }
        }

        protected void ProcessShopResponse(TransactionalResponse result, Shop shop, OrderConfirmation confirmation)
        {
            try
            {
                if (IsSuccess(result) &&
                    result.TransactionResponses != null &&
                    result.TransactionResponses.Length != 0)
                {
                    confirmation.Result.Status = Status.Success;

                    var responses = result.TransactionResponses;

                    if (shop.Customer.CustomerID == 0)
                        ProcessCreateCustomerResponse(responses, confirmation);
                    else
                        ProcessUpdateCustomerResponse(responses, confirmation);

                    ProcessCreateOrderResponse(responses, confirmation, shop.Order);
                    ProcessChargeCreditCardTokenResponse(responses, confirmation, shop.CreditCardList);
                    ProcessSetAccountResponse(responses, confirmation, shop.Order, shop.CreditCardList.FirstOrDefault());
                }
                else
                {
                    confirmation.Result.Status = Status.Failure;
                    ProcessErrors(result, confirmation);
                }
            }
            catch (Exception ex)
            {
                if (Status.Success.Equals(confirmation.Result.Status))
                    confirmation.Result.Errors.Add("Your order was placed but an unexpected error was encountered: " + ex.Message);
                else
                    confirmation.Result.Errors.Add("There was an unexpected error while processing your order: " + ex.Message);
            }
        }

        #endregion

        #region Process Enrollment Order

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <param name="giftMessage"></param>
        /// <returns></returns>
        OrderConfirmation IOrderService.PlaceEnrollmentOrder(Enrollment enrollment, string giftMessage)
        {
            var confirmation = new OrderConfirmation();
            TransactionalResponse result = null;
            UpdateCustomerResponse resultLoginUpdate = null;
            SetCustomerSiteResponse resultSiteSetup = null;
            var updateCustomerLoginRequest = new UpdateCustomerRequest();

            try
            {
                var requests = new List<ApiRequest>();
                var requestsUpdateLogin = new List<ApiRequest>();

                // Need to create customer if new customer
                if (enrollment.Customer.CustomerID == 0)
                {
                    var createCustomerRequest = new CreateCustomerRequest(enrollment.Customer)
                    {
                        DefaultWarehouseID = enrollment.Order.Configuration.WarehouseID,
                        CanLogin = true,
                        CustomerType = CustomerTypes.IndependentStyleAmbassador,
                        MailAddress1 = enrollment.Order.ShippingAddress.Address1,
                        MailAddress2 = enrollment.Order.ShippingAddress.Address2,
                        MailCity = enrollment.Order.ShippingAddress.City,
                        MailState = enrollment.Order.ShippingAddress.State,
                        MailZip = enrollment.Order.ShippingAddress.Zip,
                        Date1 = DateTime.Now
                    };

                    requests.Add(createCustomerRequest);
                }
                else
                {
                    // Upgrade existing customer to Independent Style Ambassador.  
                    // Need clarification which fields they can be updated
                    var updateCustomerRequest = new UpdateCustomerRequest
                    {
                        CustomerID = enrollment.Customer.CustomerID,
                        CustomerType = CustomerTypes.IndependentStyleAmbassador,
                        FirstName = enrollment.Customer.FirstName,
                        LastName = enrollment.Customer.LastName,
                        MiddleName = enrollment.Customer.MiddleName,
                        BirthDate = enrollment.Customer.BirthDate,
                        MainAddress1 = enrollment.Customer.MainAddress.Address1,
                        MainAddress2 = enrollment.Customer.MainAddress.Address2,
                        MainCity = enrollment.Customer.MainAddress.City,
                        MainState = enrollment.Customer.MainAddress.State,
                        MainZip = enrollment.Customer.MainAddress.Zip,
                        MainCountry = enrollment.Customer.MainAddress.Country,
                        MailAddress1 = enrollment.Order.ShippingAddress.Address1,
                        MailAddress2 = enrollment.Order.ShippingAddress.Address2,
                        MailCity = enrollment.Order.ShippingAddress.City,
                        MailState = enrollment.Order.ShippingAddress.State,
                        MailZip = enrollment.Order.ShippingAddress.Zip,
                        MailCountry = enrollment.Order.ShippingAddress.Country,
                        Phone = enrollment.Customer.PrimaryPhone,
                        Phone2 = enrollment.Customer.SecondaryPhone,
                        MobilePhone = enrollment.Customer.MobilePhone,
                        TaxID = enrollment.Customer.TaxID,
                        Date1 = DateTime.Now
                    };

                    requests.Add(updateCustomerRequest);

                    if (CustomerTypes.Lead.Equals(enrollment.Customer.CustomerTypeID) ||
                        CustomerTypes.RetailCustomer.Equals(enrollment.Customer.CustomerTypeID) ||
                        CustomerTypes.PartyGuest.Equals(enrollment.Customer.CustomerTypeID))
                    {
                        updateCustomerLoginRequest.CustomerID = enrollment.Customer.CustomerID;
                        updateCustomerLoginRequest.LoginName = enrollment.Customer.Email;
                        updateCustomerLoginRequest.LoginPassword = enrollment.Customer.Password;
                        updateCustomerLoginRequest.CanLogin = true;
                        requestsUpdateLogin.Add(updateCustomerLoginRequest);
                    }

                }

                // TODO: Field mapping for custom fields
                var createCustomerExRequest = new CreateCustomerExtendedRequest
                {
                    CustomerID = enrollment.Customer.CustomerID,
                    ExtendedGroupID = enrollment.CustomerEx.CustomerExtendedGroupID,
                    Field1 = enrollment.CustomerEx.Field1, //PrimaryContact
                    Field2 = enrollment.CustomerEx.Field2, //PrimaryContactTitle
                    Field3 = enrollment.CustomerEx.Field3, //BusinessPhone
                    Field4 = enrollment.CustomerEx.Field4, //BusinessCellPhone
                    Field5 = enrollment.CustomerEx.Field5, //BusinessEmail
                    Field6 = enrollment.CustomerEx.Field6, //BusinessType
                    Field7 = enrollment.CustomerEx.Field7, //BusinessOwners1
                };

                requests.Add(createCustomerExRequest);

                var createCustomerExAgreementRequest = new CreateCustomerExtendedRequest
                {
                    CustomerID = enrollment.Customer.CustomerID,
                    ExtendedGroupID = enrollment.CustomerExAgreements.CustomerExtendedGroupID,
                    Field1 = enrollment.CustomerExAgreements.Field1,    // Agreed to SA - T&Cs
                    Field4 = enrollment.CustomerExAgreements.Field4,    // Agreed to P&Ps
                    Field5 = enrollment.CustomerExAgreements.Field5     // Agreed to eSign
                };

                requests.Add(createCustomerExAgreementRequest);

                // Setup of a new site can be in the transaction if for a new customer
                // For Leads and Retail customers, they are added after the transaction
                if (enrollment.Customer.CustomerID == 0)
                {
                    var setCustomerSiteRequest = new SetCustomerSiteRequest(enrollment.CustomerSite);
                    requests.Add(setCustomerSiteRequest);
                }

                int shippingMethodId = enrollment.Order.ShippingMethodId != 0 ?
                    enrollment.Order.ShippingMethodId :
                    enrollment.Order.Configuration.DefaultShipMethodID;

                var createOrderRequest = new CreateOrderRequest(
                    enrollment.Customer.CustomerID,
                    enrollment.Order.Configuration,
                    giftMessage,
                    shippingMethodId,
                    enrollment.Order);
                // Extension for new Enrollment Pages
                createOrderRequest.FirstName = createOrderRequest.FirstName == null ? enrollment.Customer.FirstName : createOrderRequest.FirstName;
                createOrderRequest.LastName = createOrderRequest.LastName == null ? enrollment.Customer.LastName : createOrderRequest.LastName;
                createOrderRequest.MiddleName = createOrderRequest.MiddleName == null ? enrollment.Customer.MiddleName : createOrderRequest.MiddleName;
                createOrderRequest.Email = createOrderRequest.Email == null ? enrollment.Customer.Email : createOrderRequest.Email;
                createOrderRequest.Phone = createOrderRequest.Phone == null ? enrollment.Customer.MobilePhone : createOrderRequest.Phone;

                //// Shipping and Tax override for new Enrollemnt Pages
                //createOrderRequest.ShippingAmountOverride = 0M;
                //createOrderRequest.TaxRateOverride = 0M;

                createOrderRequest.WarehouseID =
                    enrollment.Order.Configuration.StarterKitSubscriptionWarehouseID;

                requests.Add(createOrderRequest);

                var chargeCcTokenRequest = new ChargeCreditCardTokenRequest(enrollment.CreditCard);
                requests.Add(chargeCcTokenRequest);

                var setAccountCcTokenRequest = new SetAccountCreditCardTokenRequest(
                    enrollment.CreditCard,
                    chargeCcTokenRequest.CreditCardToken,
                    enrollment.Customer.CustomerID);
                requests.Add(setAccountCcTokenRequest);

                result = Api.ProcessTransaction(new TransactionalRequest
                {
                    TransactionRequests = requests.ToArray()
                });

            }
            catch (Exception ex)
            {
                confirmation.Result.Errors.Add(ex.Message);
                confirmation.Result.Status = Status.Failure;

                return confirmation;
            }

            //
            // LEAD and RETAIL existing customers; Update LOGIN info
            try
            {
                if (CustomerTypes.Lead.Equals(enrollment.Customer.CustomerTypeID) ||
                    CustomerTypes.RetailCustomer.Equals(enrollment.Customer.CustomerTypeID) ||
                    CustomerTypes.PartyGuest.Equals(enrollment.Customer.CustomerTypeID))
                {
                    resultLoginUpdate = Api.UpdateCustomer(updateCustomerLoginRequest);
                }
            }
            catch (Exception ex)
            {
                confirmation.Result.Errors.Add("A problem occured when creating your login credentials.  Please call customer service.");
                confirmation.Result.Status = Status.Failure;
                return confirmation;
            }

            //
            // Customer Site setup is done here when the customer already exists; another site asignment is above and placed
            // in the transaction when part of a new customer.
            try
            {
                if (enrollment.Customer.CustomerID != 0)
                {

                    var setCustomerSiteRequest = new SetCustomerSiteRequest(enrollment.CustomerSite);
                    setCustomerSiteRequest.CustomerID = enrollment.Customer.CustomerID;
                    resultSiteSetup = Api.SetCustomerSite(setCustomerSiteRequest);
                }
            }
            catch (Exception ex)
            {
                confirmation.Result.Errors.Add("A problem occured while setting up your replicated site.  Please call customer service.");
                confirmation.Result.Status = Status.Failure;
                return confirmation;
            }

            //
            // Initialize the point accounts for the new Style Ambassador
            try
            {
                int customerId = enrollment.Customer.CustomerID;

                if (customerId == 0)
                {
                    CreateCustomerResponse createCustomerResponse = result.TransactionResponses.FirstOrDefault(s => s is CreateCustomerResponse) as CreateCustomerResponse;
                    customerId = createCustomerResponse.CustomerID;
                }

                //InitializeStyleAmbassadorPointAccounts(customerId);
            }
            catch (Exception ex)
            {
                confirmation.Result.Errors.Add("A problem occured while initializing your point accounts.  Please call customer service.");
                confirmation.Result.Status = Status.Failure;
                return confirmation;
            }

            // 
            //
            ProcessEnrollmentOrderResponse(result, enrollment, confirmation);

            return confirmation;
        }

        protected void ProcessEnrollmentOrderResponse(TransactionalResponse result, Enrollment enrollment, OrderConfirmation confirmation)
        {
            try
            {
                if (IsSuccess(result) &&
                    result.TransactionResponses != null &&
                    result.TransactionResponses.Length != 0)
                {
                    confirmation.Result.Status = Status.Success;

                    var responses = result.TransactionResponses;

                    if (enrollment.Customer.CustomerID == 0)
                        ProcessCreateCustomerResponse(responses, confirmation);
                    else
                        ProcessUpdateCustomerResponse(responses, confirmation);

                    ProcessSetCustomerSiteResponse(responses, confirmation);
                    ProcessCreateOrderResponse(responses, confirmation, enrollment.Order);
                    List<CreditCard> lstCard = new List<CreditCard>();
                    lstCard.Add(enrollment.CreditCard);
                    ProcessChargeCreditCardTokenResponse(responses, confirmation, lstCard);
                    ProcessSetAccountResponse(responses, confirmation, enrollment.Order, enrollment.CreditCard);
                }
                else
                {
                    confirmation.Result.Status = Status.Failure;
                    ProcessErrors(result, confirmation);
                }
            }
            catch (Exception ex)
            {
                if (Status.Success.Equals(confirmation.Result.Status))
                    confirmation.Result.Errors.Add("Your order was placed but an unexpected error was encountered: " + ex.Message);
                else
                    confirmation.Result.Errors.Add("There was an unexpected error while processing your order: " + ex.Message);
            }
        }

        #endregion

        #region Process Event Order

        /// <summary>
        /// This method is not implemented here due to time constraints.
        /// For now, IOrderService.PlaceOrder will handle Event and host
        /// shopping.
        /// </summary>
        /// <param name="shopEvent"></param>
        /// <returns></returns>
        OrderConfirmation IOrderService.PlaceEventOrder(ShopEvent shopEvent)
        {
            throw new NotImplementedException("PlaceEventOrder has not been implemented.  Please use PlaceOrder instead.");
        }

        protected void ProcessEventShopResponse(TransactionalResponse result, ShopEvent shopEvent, OrderConfirmation confirmation)
        {
            throw new NotImplementedException("ProcessEventShopResponse will be implemented along with PlaceEventOrder.");
        }

        #endregion

        #region API Response Processing Methods

        protected void ProcessCreateCustomerResponse(ApiResponse[] responses, OrderConfirmation confirmation)
        {
            var createCustomerResponse = responses
                .FirstOrDefault(s => s is CreateCustomerResponse)
                as CreateCustomerResponse;

            if (IsSuccess(createCustomerResponse))
            {
                confirmation.CustomerID = createCustomerResponse.CustomerID;

                // TODO: Any other post-order processing
                return;
            }
            else
            {
                ProcessErrors(createCustomerResponse, confirmation);
            }
        }

        protected void ProcessUpdateCustomerResponse(ApiResponse[] responses, OrderConfirmation confirmation)
        {
            var updateCustomerResponse = responses
                .FirstOrDefault(s => s is UpdateCustomerResponse)
                as UpdateCustomerResponse;

            if (IsSuccess(updateCustomerResponse))
            {
                // TODO: Any other post-order processing
                return;
            }
            else
            {
                ProcessErrors(updateCustomerResponse, confirmation);
            }
        }

        protected void ProcessSetCustomerSiteResponse(ApiResponse[] responses, OrderConfirmation confirmation)
        {
            var setCustomerSiteResponse = responses
                .FirstOrDefault(s => s is SetCustomerSiteResponse)
                as SetCustomerSiteResponse;

            if (IsSuccess(setCustomerSiteResponse))
            {
                // TODO: Any other post-order processing
                return;
            }
            else
            {
                ProcessErrors(setCustomerSiteResponse, confirmation);
            }
        }

        protected void ProcessChargeCreditCardTokenResponse(ApiResponse[] responses, OrderConfirmation confirmation, List<CreditCard> lstCreditCard)
        {
            var chargeCcResponse = responses
                .FirstOrDefault(r => r is ChargeCreditCardResponse)
                as ChargeCreditCardResponse;

            if (IsSuccess(chargeCcResponse))
            {
                foreach (var item in lstCreditCard)
                {
                    if (!confirmation.lstPayment.Any(s => s.CreditCardNumber == item.CardNumber && s.Amount == item.Amount && s.ExpirationMonth == item.ExpirationMonth && s.ExpirationYear == item.ExpirationYear))
                    {
                        confirmation.lstPayment.Add(new Payment()
                        {
                            CreditCardNumber = item.CardNumber,
                            ExpirationMonth = item.ExpirationMonth,
                            ExpirationYear = item.ExpirationYear,
                            CreditCardTypeID = (int)item.TypeOfCard,
                            AuthorizationCode = chargeCcResponse.AuthorizationCode,
                            Amount = chargeCcResponse.Amount,
                            PaymentID = chargeCcResponse.PaymentID,
                            BillingAddress = item.BillingAddress,
                            BillingName = item.NameOnCard
                        });
                    }
                }
                // TODO: Any other post-order processing
            }
            else
            {
                ProcessErrors(chargeCcResponse, confirmation);
            }
        }

        protected void ProcessSetAccountResponse(ApiResponse[] responses, OrderConfirmation confirmation, IOrder order, CreditCard card)
        {
            var setAccountResponse = responses
                .FirstOrDefault(r => r is SetAccountResponse)
                as SetAccountResponse;

            if (IsSuccess(setAccountResponse) || order.EventId.HasValue)
            {
                confirmation.Payment.BillingName = card.NameOnCard;
                confirmation.Payment.BillingAddress = card.BillingAddress;

                // TODO: Any other post-order processing
            }
            else
            {
                ProcessErrors(setAccountResponse, confirmation);
            }
        }

        protected void ProcessCreateOrderResponse(ApiResponse[] responses, OrderConfirmation confirmation, IOrder order)
        {
            var createOrderResponse = responses
                .FirstOrDefault(r => r is CreateOrderResponse)
                as CreateOrderResponse;

            if (IsSuccess(createOrderResponse))
            {
                confirmation.Order = Mapper.Map<Order>(createOrderResponse);
                confirmation.Order.ShipMethodDescription = ShippingService.GetShippingMethodDescription(order.ShippingMethodId);
                confirmation.Order.Recipient = order.ShippingAddress;

                // This is assuming that warnings would only be available upon
                // successful order placement.
                ProcessWarnings(createOrderResponse, confirmation);
            }
            else
            {
                ProcessErrors(createOrderResponse, confirmation);
            }

        }

        #endregion

        #region Process Discount Methods

        /// <summary>
        /// Separates items based on applied discounts into individual line items.
        /// This implemention will apply only one discount per item.  The discounted
        /// item will have a price override for the discounted amount and no Business 
        /// Volume (BV) or Commission Volume (CV) will be applied, overridden as 0.  
        /// The reamaining quantity of the the same item will be a separate line item
        /// with the actual price, BV, and CV.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private OrderDetailRequest[] applyProductDicounts(IOrder order)
        {
            var orderDetails = new List<OrderDetailRequest>();
            OrderDetailRequest orderDetail = null;

            foreach (Product product in order.Products)
            {
                if (product.Quantity < product.Discounts.Count)
                {
                    throw new ApplicationException("Number of discounts applied cannot exceed the number of products.");
                }

                var quantity = product.Quantity;
                foreach (var discount in product.Discounts)
                {
                    orderDetail = new OrderDetailRequest
                    {
                        ItemCode = product.ItemCode,
                        PriceEachOverride = discount.Apply(product),
                    };

                    if (discount.DiscountType == DiscountType.RetailPromoFixed ||
                             discount.DiscountType == DiscountType.RetailPromoPercent || discount.DiscountType == DiscountType.PromoCode)
                    {
                        orderDetail.Other10EachOverride = 1M;
                        orderDetail.Quantity = quantity;
                        quantity = quantity - quantity;

                        orderDetail.BusinessVolumeEachOverride = discount.OverrideBVAmount;
                        orderDetail.CommissionableVolumeEachOverride = discount.OverrideCVAmount;
                    }
                    else
                    {
                        orderDetail.Quantity = 1M;
                        quantity--;
                        orderDetail.BusinessVolumeEachOverride = 0M;
                        orderDetail.CommissionableVolumeEachOverride = 0M;
                    }

                    if (discount.OverrideTaxableAmount)
                    {
                        orderDetail.TaxableEachOverride = orderDetail.PriceEachOverride;
                    }

                    orderDetails.Add(orderDetail);
                    // quantity--;
                }

                if (quantity != 0)
                {
                    orderDetail = new OrderDetailRequest
                    {
                        ItemCode = product.ItemCode,
                        Quantity = quantity,
                    };
                    orderDetails.Add(orderDetail);
                }
            }

            return orderDetails.ToArray();
        }

        /// <summary>
        /// Retrieves the host' point accounts and updates the rewards spent.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public RewardsAccount[] CalculateHostPointAccounts(IOrder order)
        {
            if (!order.EventId.HasValue) return null;
            var pointAccounts = EventService.GetHostRewardPointAccounts(order.EventId.Value);

            var halfOffCredits = (from pa in pointAccounts
                                  where PointAccounts.Host12offcredits.Equals(pa.PointAccountID)
                                  select pa).FirstOrDefault();

            var rewardsCash = (from pa in pointAccounts
                               where PointAccounts.HostRewardsCash.Equals(pa.PointAccountID)
                               select pa).FirstOrDefault();

            //var bookingRewardsCash = (from pa in pointAccounts
            //                          where PointAccounts.BookingsRewardsCash.Equals(pa.PointAccountID)
            //                          select pa).FirstOrDefault();

            // WIll never happen as Recuritng Reward is not associated with an Event (get together)
            //var recruitungRewards = (from pa in pointAccounts
            //    where PointAccounts.RecruitingRewards.Equals(pa.PointAccountID)
            //    select pa).FirstOrDefault();


            foreach (var product in order.Products)
            {
                if (product.Quantity < product.Discounts.Count)
                {
                    throw new ApplicationException("Number of discounts applied cannot exceed the number of products.");
                }

                foreach (var discount in product.Discounts)
                {
                    switch (discount.DiscountType)
                    {
                        case DiscountType.RewardsCash:
                            rewardsCash.AppliedAmount += discount.AppliedAmount;
                            rewardsCash.AppliedItemCode = product.ItemCode;
                            break;
                        case DiscountType.HalfOffCredits:
                            halfOffCredits.AppliedAmount++;
                            halfOffCredits.AppliedItemCode = product.ItemCode;
                            break;
                        //case DiscountType.BookingRewards:
                        //    bookingRewardsCash.AppliedAmount += discount.AppliedAmount;
                        //    bookingRewardsCash.AppliedItemCode = product.ItemCode;
                        //    break;
                        //case DiscountType.RecruitingReward:
                        //    recruitungRewards.AppliedAmount += discount.AppliedAmount;
                        //    recruitungRewards.AppliedItemCode = product.ItemCode;
                        //    break;
                        case DiscountType.HostSpecial:

                            break;
                    }
                }
            }

            return pointAccounts;
        }

        #endregion




        #region Private Methods

        private static int? GetAmbassadorId(Shop shop, int newOrderId)
        {
            if (shop.Order.EventId.HasValue)
            {
                // mechanism for event creater id or event enrollerid 
                var eventSponsor = Exigo.GetCustomersEnroller(int.Parse(shop.Order.EventId.ToString()));
                return eventSponsor.CustomerID;
            }
            
            var ordersDetails = new InvoiceDetail(getOrderDetail(newOrderId).Orders.FirstOrDefault());
            return ordersDetails.Other16 != string.Empty ? Exigo.GetCustomer(int.Parse(ordersDetails.Other16)).CustomerID : shop.Customer.EnrollerID;
        }

        #endregion

        #region OrderDetails
        public static GetOrdersResponse getOrderDetail(int OrderID)
        {
            ExigoApi api = Exigo.WebService();
            GetOrdersResponse response = new GetOrdersResponse();
            response = api.GetOrders(new GetOrdersRequest()
            {
                OrderID = OrderID,
            });

            return response;
        }

        #endregion

    }

}
