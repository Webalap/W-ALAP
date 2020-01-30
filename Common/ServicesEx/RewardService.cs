using Common.Api.ExigoOData.Rewards;
using Common.Api.ExigoWebService;
using Common.ModelsEx.Reward;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using Common.ServicesEx.Rewards;
using ExigoService;
using Ninject;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Common.ServicesEx
{
    public class RewardService : IRewardService
    {
        private readonly TraceSource _log = new TraceSource("IhBackoffice", SourceLevels.All);
        private readonly Logger _nlog = LogManager.GetCurrentClassLogger();

        #region Dependencies

        [Inject]
        public ExigoApi Api { get; set; }
        public ShoppingCartItemsPropertyBag ShoppingCart = new ShoppingCartItemsPropertyBag();
        public IAutoOrderConfiguration AutoOrderConfiguration = new UnitedStatesConfiguration.AutoOrderConfiguration
        {
            DefaultFrequencyType = FrequencyType.Monthly
        };
        #endregion

        #region For Product Spinner on Replicated
        public IList<IIndividualReward> GetActiveRewards(string siteType, int customerId)
        {
            var activeAwards = new List<IIndividualReward>();

            if (customerId <= 999) return activeAwards; //Don't want any rewards returning for corporarte accounts.

            if (siteType == "backOffice")
            {
                var customer = Exigo.GetCustomer(customerId);

                var activeBOAwards = new List<IIndividualReward>
                {
                    ServicesModule.Instance.Kernel.Get<NewEbReward>(),
                    ServicesModule.Instance.Kernel.Get<RecruitingReward>(),
                    ServicesModule.Instance.Kernel.Get<ProductCreditReward>(),
                    ServicesModule.Instance.Kernel.Get<NewStyleAmbassadorHalfOffReward>(),
                    //ServicesModule.Instance.Kernel.Get<OngoingStyleAmbassadorHalfOffReward>(),
                    //ServicesModule.Instance.Kernel.Get<NewProductsLaunchReward>()
                };
                activeAwards.AddRange(activeBOAwards.Where(reward => reward.IsEligible(customer, siteType)));
                return activeAwards;
            }
            // was being used for REP site...no longer needed for now. - Azam
            //var allRewards = AllRewards();
            //activeAwards.AddRange(allRewards.Where(reward => reward.IsEligible(customer, siteType)));

            return activeAwards;
        }


        public IList<Product> GetRewardProducts(IList<IIndividualReward> rewards, IList<Product> productsInShoppingCart)
        {
            IList<Product> rewardProducts = null;
            if (rewards != null)
            {
                rewardProducts = rewards.SelectMany(r => r.GetActiveRewardProducts(productsInShoppingCart)).ToList();

                // Specialized logic - If both New Style Ambassador and Ongoing Style Ambassador rewards are active (will be the first 30 days a Style Ambassador joins), 
                // remove any duplicate reward products the New Style Ambassador reward offers that the Ongoing Style Ambassador reward also offers.
                if (rewards.Any(r => r is NewStyleAmbassadorHalfOffReward)) /*&& (rewards.Any(r => r is NewProductsLaunchReward) || rewards.Any(r => r is OngoingStyleAmbassadorHalfOffReward)))*/
                {
                    IList<string> rewardItemCodes = rewardProducts.Where(p => p.EligibleDiscounts.Any(d => d.DiscountType == DiscountType.SAHalfOffOngoing) || p.EligibleDiscounts.Any(d => d.DiscountType == DiscountType.NewProductsLaunchReward)).Select(p => p.ItemCode).ToList();

                    rewardProducts = rewardProducts.Where(p => !(p.EligibleDiscounts.Any(d => d.DiscountType == DiscountType.SAHalfOff) && rewardItemCodes.Contains(p.ItemCode))).ToList();
                }
            }
            return rewardProducts;
        }

        private static IEnumerable<IIndividualReward> AllRewards()
        {
            var allAwards = new List<IIndividualReward>
            {
                //                ServicesModule.Instance.Kernel.Get<NewEbReward>(),
                //                ServicesModule.Instance.Kernel.Get<NewStyleAmbassadorHalfOffReward>(),
                //                ServicesModule.Instance.Kernel.Get<OngoingStyleAmbassadorHalfOffReward>(),
                //                ServicesModule.Instance.Kernel.Get<RecruitingReward>(),
                //                ServicesModule.Instance.Kernel.Get<EnrolleeReward>(),
                //                ServicesModule.Instance.Kernel.Get<NewProductsLaunchReward>()
            };

            return allAwards;
        }

        #endregion

        #region HostSpecialDiscount

        public HostSpecialDiscount GetHostSpecialReward(DateTime eventDate)
        {
            //var context = Exigo.CreateODataContext<RewardsContext>(GlobalSettings.Exigo.Api.SandboxID);

            //var hostSpecial = (from hs in context.HostSpecials
            //                   where hs.StartDate <= eventDate && eventDate <= hs.EndDate
            //                   select hs).FirstOrDefault();

            //return (HostSpecialDiscount)hostSpecial;

            try
            {
                using (var context = Exigo.Sql())
                {
                    var SqlProcedure = string.Format("GetHostSpecialReward '{0}'", eventDate);
                    var hostSpecial = context.Query<HostSpecial>(SqlProcedure).FirstOrDefault();
                    return (HostSpecialDiscount)hostSpecial;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           
         
        }

        public HostSpecialDiscount GetHostSpecialReward(int eventId)
        {
            var details = Exigo.GetCustomerExtendedDetails(eventId, (int)CustomerExtendedGroup.PartyDetails);

            var hostSpecialReward = (from d in details
                                     select new HostSpecialDiscount
                                     {
                                         CustomerExtendedDetailId = d.CustomerExtendedDetailID,
                                         ItemCode = d.Field8 == string.Empty ? string.Empty : d.Field8,
                                         DiscountAmount = string.IsNullOrEmpty(d.Field9) ? 0M : decimal.Parse(d.Field9),
                                         HasBeenRedeemed = !string.IsNullOrEmpty(d.Field10) && d.Field10 != "FALSE" && int.Parse(d.Field10) == 1,
                                         // d.Field10 != string.Empty && Convert.ToBoolean(d.Field10)),
                                         SalesThreshold = string.IsNullOrEmpty(d.Field11) ? 0M : decimal.Parse(d.Field11),
                                     }).FirstOrDefault();

            return hostSpecialReward;
        }
        #endregion

        #region Extraordinary Beginnings

        public bool BecameSABetweenMay17AndAug232015(int customerId)
        {

            var customer = Exigo.GetCustomer(customerId);
            var dateBecameSa = customer.Date1 ?? DateTime.Now;
            var startDate = new DateTime(2015, 5, 17);
            var endDate = new DateTime(2015, 8, 23);
            return (dateBecameSa >= startDate && dateBecameSa <= endDate);
        }

        //This will get the sales threshold for a reward given a phase
        public decimal GetSalesThreshold(int phaseNum)
        {
            var salesThreshhold = 0M;
            var reward = GetEbRewardDiscount(phaseNum);
            if (reward != null)
            {
                salesThreshhold = reward.SalesThreshhold;
            }
            return salesThreshhold;
        }

        //This will get the total amount of days the customer has been a Style Ambassador
        public int GetTotalSaDays(int customerId)
        {

            if (BecameSABetweenMay17AndAug232015(customerId))
                return 0;
            var customer = Exigo.GetCustomer(customerId);
            var dateBecameSa = customer.Date1 ?? DateTime.Now;
            var diff = (DateTime.Now.Date - dateBecameSa.Date);
            return diff.Days;
        }

        //This will get the active phase for a given amount of days the customer has been a Style Ambassador
        public int GetActivePhase(int totalSaDays)
        {

            var phaseNum = 0;
            if (totalSaDays >= 1 && totalSaDays <= 40)
            {
                phaseNum = 1;
            }

            else if (totalSaDays >= 41 && totalSaDays <= 70)
            {
                phaseNum = 2;
            }

            else if (totalSaDays >= 71 && totalSaDays <= 100)
            {
                phaseNum = 3;
            }

            return phaseNum;
        }

        //This will get the active phases for a given amount of days the customer has been a Style Ambassador and total sales
        public List<int> GetActivePhases(int totalSaDays, decimal totalSales, int customerId)
        {
            var phaseNums = new List<int>();

            if (totalSaDays >= 1 && totalSaDays <= 100 && totalSales >= (decimal)PRVThresholds.Phase1)
            {
                phaseNums.Add(1);
            }

            if (totalSaDays >= 41 && totalSaDays <= 100 && totalSales >= (decimal)PRVThresholds.Phase2)
            {
                phaseNums.Add(2);
            }

            if (totalSaDays >= 71 && totalSaDays <= 100 && totalSales >= (decimal)PRVThresholds.Phase3)
            {
                phaseNums.Add(3);
            }

            return phaseNums;
        }

        //This will get the EB Volumes
        public VolumeCollection GetEbRewardVolumes(int customerId)
        {
            // Get the volumes used for the customers sales amounts for the appropriate months
            var volumes = Exigo.GetCustomerVolumes(new GetCustomerVolumesRequest
            {
                CustomerID = customerId,
                PeriodTypeID = PeriodTypes.Monthly,
                VolumeIDs = new[] { 16, 17, 18, 22 },
            });

            //For Testing
            //var volumes = new VolumeCollection
            //{
            //    Volume16 = 1501,
            //    Volume17 = 1500,
            //    Volume18 = 1600,
            //    Volume22 = 4601
            //};

            return volumes;
        }

        //This will get the Phase Status for a given customer and phase
        public RewardPhase GetPhaseStatus(int customerId, int phaseNum)
        {

            var rewardStatus = new RewardPhase();
            var totalSaDays = GetTotalSaDays(customerId);
            if (totalSaDays == 0)
                return rewardStatus;
            // Get the volumes used for the customers sales amounts for the appropriate months
            var volumes = GetEbRewardVolumes(customerId);
            if (volumes == null) return rewardStatus;
            var salesThreshhold = GetSalesThreshold(phaseNum);
            decimal percentCompleted;
            switch (phaseNum)
            {
                case 1:
                    percentCompleted = Math.Round(((volumes.Volume16 / salesThreshhold) * 100));
                    rewardStatus.PhaseNum = 1;
                    // change 37 to 40 for phase1
                    rewardStatus.TotalDays = Math.Min(totalSaDays, 40);  //40
                    rewardStatus.TotalDaysEnd = 40; //40
                    rewardStatus.AmountCompleted = Math.Round(volumes.Volume16, 2);
                    rewardStatus.PercentCompleted = Math.Min(percentCompleted, 100);
                    rewardStatus.CurrentVolume = Math.Round(volumes.Volume16, 2);
                    break;
                case 2:
                    percentCompleted = Math.Round(((volumes.Volume17 / salesThreshhold) * 100));
                    rewardStatus.PhaseNum = 2;
                    // change 67 to 70 for phase2
                    rewardStatus.TotalDays = Math.Min(totalSaDays, 70); //70
                    rewardStatus.TotalDaysEnd = 70; //70
                    rewardStatus.AmountCompleted = Math.Round(volumes.Volume17, 2);
                    rewardStatus.PercentCompleted = Math.Min(percentCompleted, 100);
                    rewardStatus.CurrentVolume = Math.Round(volumes.Volume17, 2);
                    break;
                case 3:
                    percentCompleted = Math.Round(((volumes.Volume18 / salesThreshhold) * 100));
                    rewardStatus.PhaseNum = 3;
                    // change 97 to 100 for phase3
                    rewardStatus.TotalDays = Math.Min(totalSaDays, 100); //100
                    rewardStatus.TotalDaysEnd = 100; //100
                    rewardStatus.AmountCompleted = Math.Round(volumes.Volume18, 2);
                    rewardStatus.PercentCompleted = Math.Min(percentCompleted, 100);
                    rewardStatus.CurrentVolume = Math.Round(volumes.Volume18, 2);
                    break;
            }

            rewardStatus.SalesThreshold = salesThreshhold;
            if (rewardStatus.PercentCompleted >= 100)
            {
                rewardStatus.Reward = GetCustomerEbRewardDiscount(customerId, phaseNum);
            }

            return rewardStatus;
        }

        
        public List<Item> GetBonusKickerAvailableReward(int custId)
        {
            var customerEx =
                Exigo.GetCustomerExtendedDetails(custId,
                    (int)CustomerExtendedGroup.BonusKicker)
                    .Where(x => x.Field5 != "1");
            var rewardItems = customerEx.Select(custEx => Exigo.GetItem(custEx.Field3)).ToList();
            return rewardItems;
        }

        //This will get an Extended EB Reward Configuration for a given phase
        private static EBReward GetEbRewardDiscount(int phaseNum)
        {
            //var context = Exigo.CreateODataContext<RewardsContext>(GlobalSettings.Exigo.Api.SandboxID);
            //var ebReward = (from ebr in context.EBRewards
            //                where ebr.Phase == phaseNum
            //                select ebr).FirstOrDefault();

            //return ebReward;
            try
            {
                using (var context = Exigo.Sql())
                {
                    var SqlProcedure = string.Format("GetebReward {0}", phaseNum);
                    var ebReward = context.Query<EBReward>(SqlProcedure).FirstOrDefault();
                    return ebReward;
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
           
        }

        //This will get the Customer Extended Reward Configuration for a given customer and phase
        public EBRewardDiscount GetCustomerEbRewardDiscount(int customerId, int phase)
        {
            int _phase = 0;
            var details = Exigo.GetCustomerExtendedDetails(customerId, (int)CustomerExtendedGroup.EbRewards /* EB Rewards Extended Group ID */);
            var ebReward = (from d in details
                            where true == int.TryParse(d.Field1, out _phase) //Field1 is the Phase Number
                            select new EBRewardDiscount()
                            {
                                PhaseNumber = int.Parse(d.Field1),
                                CompletionDate = DateTime.Parse(d.Field2)
                            }).FirstOrDefault(i => i.PhaseNumber.Equals(phase));

            return ebReward;
        }

        //This will get all the Customer Extended Reward Configuration for a given customer
        public IList<EBRewardDiscount> GetCustomerEbRewardDiscounts(int customerId)
        {
            var details = Exigo.GetCustomerExtendedDetails(customerId, (int)CustomerExtendedGroup.EbRewards /* EB Rewards Extended Group ID */);

            var ebReward = (from d in details
                            select new EBRewardDiscount()
                            {
                                CustomerExtendedDetailId = d.CustomerExtendedDetailID,
                                PhaseNumber = int.Parse(d.Field1),
                                CompletionDate = DateTime.Parse(d.Field2)
                            }
             );

            return ebReward.ToList();
        }

        #endregion Extraordinary Beginnings

        #region Recruiting-Reward
        ////Not being used anymore...just here for reference
        //public void CreateCustomerRecruitingRewards(int customerId)
        //{

        //    var api = Exigo.WebService();
        //    var reward = new RecruitingRewardDiscount();

        //    //Set start and expiry dates.
        //    var startDate = DateTime.Now;
        //    reward.CompletionDate = startDate.AddDays(60);

        //    //customerId = 1022;
        //    const int extendedGroupId = (int)CustomerExtendedGroup.RecruitingRewards;

        //    //Get Discount amount and check validity.
        //    reward.RewardAmount = Convert.ToDecimal(GetRecruitingReward(customerId));
        //    if (reward.DiscountAmount <= 0) return;

        //    //Check if the reward has already been added for the customer
        //    var customerEx = Exigo.GetCustomerExtendedDetails(customerId, extendedGroupId).Where(x => Convert.ToDateTime(x.Field2) == reward.CompletionDate);
        //    if (customerEx.Any()) return;

        //    var response = api.CreateCustomerExtended(new CreateCustomerExtendedRequest()
        //    {
        //        CustomerID = customerId,
        //        ExtendedGroupID = extendedGroupId,
        //        Field1 = reward.ItemCode, //Item Code
        //        Field2 = reward.CompletionDate.ToString(CultureInfo.InvariantCulture), //Completion Date
        //        Field4 = Convert.ToString(reward.RewardAmount, CultureInfo.InvariantCulture), //RewardAmount
        //        Field3 = 0.ToString(CultureInfo.InvariantCulture) //Redeemed

        //    });
        //}

        ////Also not being use at this time. 
        //public decimal GetRecruitingReward(int customerId)
        //{

        //    var request = new GetPointAccountRequest()
        //    {
        //        CustomerID = customerId,
        //        PointAccountID = PointAccounts.RecruitingRewards
        //    };
        //    var rewardAmount = Exigo.GetRecruitingReward(request);

        //    return rewardAmount;
        //}

        //public IList<RecruitingRewardDiscount> GetCustomerRecruitingRewardDiscounts(int customerId)
        //{
        //    var details = Exigo.GetCustomerExtendedDetails(customerId, (int)CustomerExtendedGroup.RecruitingRewards /* Recruiting Rewards Extended Group ID */);

        //    var recruitingReward = (from d in details
        //                            select new RecruitingRewardDiscount()
        //                    {
        //                        CustomerExtendedDetailId = d.CustomerExtendedDetailID,
        //                        CompletionDate = DateTime.Parse(d.Field2),
        //                        ItemCode = d.Field1,
        //                        HasBeenRedeemed = (d.Field3 == "1"),
        //                        RewardAmount = Convert.ToDecimal(d.Field4)
        //                    }
        //     );

        //    return recruitingReward.ToList();

        //}

        #endregion Recruiting-Reward

        #region Enrollee-Reward

        //public void CreateCustomerEnrolleeRewards(int customerId)
        //{

        //    var api = Exigo.WebService();
        //    var reward = new EnrolleeRewardDiscount();

        //    //Set start and expiry dates.
        //    var startDate = DateTime.Now;
        //    reward.CompletionDate = startDate.AddDays(60);

        //    //customerId = 1022;
        //    const int extendedGroupId = (int)CustomerExtendedGroup.EnrolleeRewards;

        //    //Get Discount amount and check validity.
        //    reward.RewardAmount = Convert.ToDecimal(GetEnrolleeReward(customerId));
        //    if (reward.DiscountAmount <= 0) return;

        //    //Check if the reward has already been added for the customer
        //    var customerEx = Exigo.GetCustomerExtendedDetails(customerId, extendedGroupId).Where(x => Convert.ToDateTime(x.Field2) == reward.CompletionDate);
        //    if (customerEx.Any()) return;

        //    var response = api.CreateCustomerExtended(new CreateCustomerExtendedRequest()
        //    {
        //        CustomerID = customerId,
        //        ExtendedGroupID = extendedGroupId,
        //        Field1 = reward.CompletionDate.ToString(CultureInfo.InvariantCulture), //Completion Date
        //        Field2 = Convert.ToString(reward.RewardAmount), //RewardAmount
        //        Field3 = 0.ToString(CultureInfo.InvariantCulture) //Redeemed

        //    });
        //}

        //public decimal GetEnrolleeReward(int customerId)
        //{

        //    var request = new GetPointAccountRequest()
        //    {
        //        CustomerID = customerId,
        //        PointAccountID = PointAccounts.EnrolleeRewards
        //    };
        //    var rewardAmount = Exigo.GetEnrolleeReward(request);

        //    return rewardAmount;
        //}

        //public IList<EnrolleeRewardDiscount> GetCustomerEnrolleeRewardDiscounts(int customerId)
        //{
        //    var details = Exigo.GetCustomerExtendedDetails(customerId, (int)CustomerExtendedGroup.EnrolleeRewards /* Recruiting Rewards Extended Group ID */);

        //    var enrolleeReward = (from d in details
        //                          select new EnrolleeRewardDiscount()
        //                            {
        //                                CustomerExtendedDetailId = d.CustomerExtendedDetailID,
        //                                CompletionDate = DateTime.Parse(d.Field2),
        //                                ItemCode = d.Field3,
        //                                HasBeenRedeemed = (d.Field5 == "1")
        //                            }
        //     );

        //    return enrolleeReward.ToList();

        //}

        #endregion Enrollee-Reward


        public int CreateRewardsAutoOrder(string itemCode, int customerId, int ExtendedGroupID, int phase)
        {
            ShoppingCart.Items.Clear();
            try
            {
                Api = Exigo.WebService();

                CustomerExtendedDetails customerEx = Exigo.GetCustomerExtendedDetails(customerId, ExtendedGroupID).FirstOrDefault(x => x.Field3 == itemCode && x.Field1 == phase.ToString());
                if (customerEx != null)
                {
                    return 0;
                }
                var customer = Exigo.GetCustomer(customerId);
                // Start creating the API requests
                var details = new List<ApiRequest>();
                // Create the order request, if applicable
                ShoppingCartItem cartItem = ShoppingCartItem.Create();

                cartItem.ItemCode = itemCode;
                cartItem.Quantity = 1;
                cartItem.PriceEachOverride = 0;
                cartItem.PriceTypeID = PriceTypes.Wholesale;
                cartItem.Type = ShoppingCartItemType.Order;

                ShoppingCart.Items.Add(cartItem);
                if (string.IsNullOrEmpty(itemCode))
                {
                    _log.TraceEvent(TraceEventType.Warning, 1, "IRewardService.CreateBonusKickerAutoOrder( String, int ): Entering method and ShoppingCart.Items is {0}.", (ShoppingCart.Items == null) ? "null" : "empty");
                    _nlog.Warn("IRewardService.CreateBonusKickerAutoOrder( String, int ): Entering method and ShoppingCart.Items is {0}.", (ShoppingCart.Items == null) ? "null" : "empty");
                }
                else
                {
                    _log.TraceEvent(TraceEventType.Information, 1, "IRewardService.CreateBonusKickerAutoOrder( String, int ): Entering method and ShoppingCart.Items is {0}.", ShoppingCart.Items.Count);
                    _nlog.Info("IOrderService.PlaceOrder( Shop, String ): Entering method with {0} products.", ShoppingCart.Items.Count);
                }
                // Create the autoorder request, if applicable
                var autoOrderRequest = new CreateOrderRequest(
                    AutoOrderConfiguration,
                    (int)ShipMethods.Standard,
                    string.Empty,
                    ShoppingCart.Items,
                    new ShippingAddress
                    {
                        Address1 = string.IsNullOrEmpty(customer.MainAddress.Address1) ? customer.MailingAddress.Address1 : customer.MainAddress.Address1,
                        Address2 = string.IsNullOrEmpty(customer.MainAddress.Address2) ? customer.MailingAddress.Address2 : customer.MainAddress.Address2,
                        AddressType = AddressType.Mailing,
                        City = string.IsNullOrEmpty(customer.MainAddress.City) ? customer.MailingAddress.City : customer.MainAddress.City,
                        Country = string.IsNullOrEmpty(customer.MainAddress.Country) ? customer.MailingAddress.Country : customer.MainAddress.Country,
                        CustomerID = customer.CustomerID,
                        Email = customer.Email,
                        LastName = customer.LastName,
                        FirstName = customer.FirstName,
                        Phone = customer.MobilePhone,
                        State = string.IsNullOrEmpty(customer.MainAddress.State) ? customer.MailingAddress.State : customer.MainAddress.State,
                        Zip = string.IsNullOrEmpty(customer.MainAddress.Zip) ? customer.MailingAddress.Zip : customer.MainAddress.Zip,
                    },
                    shippingOverride: 0,
                    taxOverride: 0);

                autoOrderRequest.TaxRateOverride = 0M;
                autoOrderRequest.ShippingAmountOverride = 0M;
                autoOrderRequest.CustomerID = customerId;
                autoOrderRequest.Other15 = "1";

                details.Add(autoOrderRequest);

                // Process the transaction
                var transactionRequest = new TransactionalRequest { TransactionRequests = details.ToArray() };
                var transactionResponse = Exigo.WebService().ProcessTransaction(transactionRequest);
                if (transactionResponse == null)
                {
                    _log.TraceEvent(TraceEventType.Error, 1, "There was no CreateOrderResponse.");
                }
                else
                {
                    _log.TraceEvent(TraceEventType.Information, 1, "ProcessTransaction.Result.TransactionKey = {0}", transactionResponse.Result.TransactionKey);
                    _log.TraceEvent(TraceEventType.Information, 1, "ProcessTransaction.Result.Reponse = {0}", transactionResponse.Result.Status);

                    if ((transactionResponse.Result.Errors != null) && (transactionResponse.Result.Errors.Length > 0))
                    {
                        _log.TraceEvent(TraceEventType.Error, 1, "ProcessTransaction.Result.Errors.Length = {0}", transactionResponse.Result.Errors.Length);
                        for (int i = 0; i < transactionResponse.Result.Errors.Length; i++)
                        {
                            _log.TraceEvent(TraceEventType.Error, 1, "ProcessTransaction.Result.Errors[{0}] = {1}", i, transactionResponse.Result.Errors[i]);
                        }
                    }
                }
                var newOrderId = 0;

                //process the response
                if (transactionResponse.Result.Status == ResultStatus.Success)
                {
                    foreach (var orderResponse in transactionResponse.TransactionResponses.OfType<CreateOrderResponse>())
                    {
                        newOrderId = orderResponse.OrderID;

                        var request = Api.CreateCustomerExtended(new CreateCustomerExtendedRequest()
                        {
                            CustomerID = customer.CustomerID,
                            ExtendedGroupID = ExtendedGroupID,
                            Field1 = phase.ToString(), // phase # for bonus kicker or module for training
                            Field2 = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                            Field3 = itemCode, //ItemCode
                            Field4 = 100.ToString(), //Discount Percent
                            Field5 = 1.ToString(CultureInfo.InvariantCulture), //Redeemed
                            Field6 = newOrderId.ToString() // orderid 
                        });
                    }

                    var updateOrder = Api.ChangeOrderStatus(new ChangeOrderStatusRequest()
                    {
                        OrderID = newOrderId,
                        OrderStatus = OrderStatusType.Accepted
                    });

                }
                return newOrderId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }



        #region Qualifying Recruit
        public void CreateQualifiedRecruitReward(int customerId)
        {
            var api = Exigo.WebService();

            var lst = GetQualifiedRecruits(customerId);
            if (lst.Count <= 0) return;
            var previousAwardedRecruit = Exigo.GetCustomerExtendedDetails(customerId, (int)CustomerExtendedGroup.QualifiedRecruit).ToList();


            foreach (var item in lst.Where(item => previousAwardedRecruit.Where(i => i.Field1.Equals(item.RecruitID.ToString())).ToList().Count == 0))
            {
                decimal qualifiedRecruitCredit;
                var enrollerEBProgramEndDate = item.EnrollerStartDate.AddDays(100);
                var recruitedInEBProgram = item.RecruitStartDate >= item.EnrollerStartDate && item.RecruitStartDate <= enrollerEBProgramEndDate;

                // we give $200 here and $200 comes from the Exigo recruiting reward at the beginning of the next month
                if (!recruitedInEBProgram) continue;
                qualifiedRecruitCredit = (decimal)QualifiedRecruitThreshold.Phase2;

                var responseQualifiedRecruit = api.CreateCustomerExtended(new CreateCustomerExtendedRequest()
                {
                    CustomerID = customerId, //Customer ID
                    ExtendedGroupID = (int)CustomerExtendedGroup.QualifiedRecruit,
                    Field1 = item.RecruitID.ToString(), // Qualified Recruit Customer ID
                    Field2 = item.RecruitStartDate.ToString(CultureInfo.InvariantCulture), // Recruit Start Date
                    Field3 = qualifiedRecruitCredit.ToString(CultureInfo.InvariantCulture), // Amount Awarded
                    Field4 = DateTime.Now.ToString(CultureInfo.InvariantCulture) // creation date and time
                });
                
                var responsePointAccount = api.CreatePointTransaction(new CreatePointTransactionRequest
                {
                    Amount = qualifiedRecruitCredit,
                    CustomerID = customerId,
                    PointAccountID = PointAccounts.QualifiedRecruit,
                    TransactionType = PointTransactionType.Adjustment
                });
            }
        }

        public List<QualifiedRecruit> GetQualifiedRecruits(int customerId)
        {
            string sqlQuery = string.Format("GetQualifiedRecruits {0},{1},{2},{3}",PeriodTypes.Monthly, CustomerTypes.IndependentStyleAmbassador, CustomerStatuses.Active, customerId);
            return Exigo.Sql().Query<QualifiedRecruit>(sqlQuery).ToList();
        }
        #endregion

    }
}