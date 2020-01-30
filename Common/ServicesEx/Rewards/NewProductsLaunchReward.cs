using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Customer = ExigoService.Customer;
using CustomerExtendedGroup = Common.ModelsEx.Shopping.CustomerExtendedGroup;

namespace Common.ServicesEx.Rewards
{
    public class NewProductsLaunchReward : BaseNewProductsLaunchReward
    {
        #region Implemented Protected Properties

        protected override DiscountType RewardNewProductsLaunchDiscountType 
        {
            get { return DiscountType.NewProductsLaunchReward; }
        }

        protected override int RewardItemCategory
        {
            get { return 51; }
        }

        protected override int? RewardPointsAccountId 
        {
            get { return null; }
        }

        #endregion

        #region Implemented Protected Abstract Methods

        /// <summary>
        /// This method determines if all the eligiblity requirements are met for this reward.
        /// </summary>
        protected override bool EligibilityRequirements(Customer customer, string siteType)
        {
            var newSA = false;
            if (siteType != "backOffice") return false;
            if (customer.CustomerTypeID != CustomerTypes.IndependentStyleAmbassador) return false;

            if (customer.Date1.HasValue && customer.Date1.Value > new DateTime(2016, 08, 08, 2, 0, 0)) return false;
            
            if (customer.Date1.HasValue && customer.Date1.Value >= new DateTime(2016, 06, 01, 0, 0, 0) &&
                customer.Date1.Value <= new DateTime(2016, 08, 08, 2, 0, 0))
            {
                newSA = true;
            }

            using (var context = Exigo.Sql())
            {
                try { 
                    var sqlProcedure = string.Format(@"GetSumOfVolume12 {0},{1}", customer.CustomerID, PeriodTypes.Monthly);
                    var eligiblePRV = newSA ? 0 : context.Query<decimal>(sqlProcedure).FirstOrDefault();
                    var reward = GetActiveNewProductsLaunchRewards(customer, eligiblePRV);
                    var creditsRemaining = CreditsRemaining(customer.CustomerID, reward);
                    if (reward == null || creditsRemaining <= 0) return false;
                    EligibleReward = reward;
                    AvailableCredits = creditsRemaining;
                    return true;    
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        protected override int CreditsRemaining(int customerID, Api.ExigoOData.Rewards.NewProductsLaunchReward reward)
        {
            if (reward == null) return 0;
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("CreditsRemaining {0},{1},'{2}'", (int)CustomerExtendedGroup.NewProductsLaunchPurchase, customerID, reward.StartDate);
                var count = context.Query<int>(SqlProcedure).FirstOrDefault();

                return reward.Credits - count;
                
            }

            
            
        }

        public override void PopulateEligibleDiscounts(List<Product> products)
        {
            foreach (var product in products)
            {
                //if (product.EligibleDiscounts.Where(i => i.DiscountType.Equals(DiscountType.TenPersentPRV)).FirstOrDefault() != null) continue;
                var productDiscount = product.EligibleDiscounts.Where(i => i.DiscountType == DiscountType.NewProductsLaunchReward).FirstOrDefault();
                if (productDiscount == null) return;
                product.ApplyDiscount(productDiscount);
                product.ApplyDiscountType = productDiscount.DiscountType;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method returns the New Products Launch % off rewards that are currently active
        /// </summary>
        private Api.ExigoOData.Rewards.NewProductsLaunchReward GetActiveNewProductsLaunchRewards(Customer customer, decimal eligiblePRV)
        {
           //var context = Exigo.CreateODataContext<RewardsContext>(GlobalSettings.Exigo.Api.SandboxID);
            var customerStartDate = SAStartDate(customer);
   
            //var newSAReward = context.NewProductsLaunchRewards.Where(c => c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now && c.Credits == 998 && c.PRVThresholdMin == 0).FirstOrDefault();
         Common.Api.ExigoOData.Rewards.NewProductsLaunchReward  newSAReward = null;

         try
         {
             using (var context = Exigo.Sql())
             {
                 var SqlProcedure = string.Format("newSAReward '{0}',{1},{2}", DateTime.Now, 998, 0);
                 newSAReward = context.Query<Common.Api.ExigoOData.Rewards.NewProductsLaunchReward>(SqlProcedure).FirstOrDefault();

             }

             var eligibleAsNewSA = newSAReward != null && ((customerStartDate >= newSAReward.ThresholdPeriodStart &&
                                                            customerStartDate <= newSAReward.ThresholdPeriodEnd) && newSAReward.Credits == 998 && newSAReward.PRVThresholdMin == 0); // 998 is when you are eligible for all products as a new SA
             //var availableRewrads = context.NewProductsLaunchRewards.Where(c => c.Credits != 998 && c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now && (c.PRVThresholdMin <= eligiblePRV && eligiblePRV <= c.PRVThresholdMax)).FirstOrDefault();
             Common.Api.ExigoOData.Rewards.NewProductsLaunchReward availableRewrads = null;

             using (var context = Exigo.Sql())
             {
                 var SqlProcedure = string.Format("availableRewrads '{0}',{1},{2}", DateTime.Now, 998, eligiblePRV);
                 availableRewrads = context.Query<Common.Api.ExigoOData.Rewards.NewProductsLaunchReward>(SqlProcedure).FirstOrDefault();

             }
             return eligibleAsNewSA ? newSAReward : availableRewrads;
         }
         catch (Exception ex)
         {
             
             throw;
         }
           
        }



        #endregion
    }
}