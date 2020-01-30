using Common.Api.ExigoOData.Rewards;
using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Common.ServicesEx.Rewards
{
    public class OngoingStyleAmbassadorHalfOffReward : BaseStyleAmbassadorHalfOffReward
    {
        #region Implemented Protected Properties

        protected override DiscountType RewardHalfOffDiscountType 
        {
            get { return DiscountType.SAHalfOffOngoing; }
        }

        protected override int RewardItemCategory
        {
            get { return 34; }
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
            // Must be a Style Ambassador that meets one of the following conditions:
            // (1) joined within the past 60 days 
            // (2) personal retail volumes for both the current and previous months must exceed a configurable threshold.  
            // Reward is for the replicated site only (not applicable for event).  Included back office so we can show messaging to the customer when 
            // this reward is active

            
            var reward = GetActiveStyleAmbassadorReward();
            
            if (reward == null) return false;
            if (siteType != "rep" && siteType != "backOffice") return false;
            if (customer.CustomerTypeID != CustomerTypes.IndependentStyleAmbassador) return false;
            if (DateTime.Now.Date > reward.EndDate) return false;

            var customerFirst60Days = ThirtyDaysWhenCustomerBecameStyleAmbassador(customer).AddDays(30);
            var first60Days = DateTime.Now.Date <= customerFirst60Days;
            if (first60Days)
            {
                return true;
            }

   
            //var query = Exigo.OData().PeriodVolumes
            //    .Where(c => c.CustomerID == customer.CustomerID && c.PeriodTypeID == PeriodTypes.Monthly && c.Period.StartDate <= DateTime.Now)
            //    .OrderByDescending(c => c.Period.StartDate)
            //    .Select(p => new {p.Volume12, p.Period, PeriodId = p.PeriodID } )
            //    .Take(3).ToList();
            List<VolumeCollection> query = null;
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("GetPeriodVolumes {0},{1},'{2}'", customer.CustomerID, PeriodTypes.Monthly, DateTime.Now);
                query = context.Query<VolumeCollection>(SqlProcedure).ToList();

            }
            var threshold = reward.PersonalRetailVolumeThreshold;

            switch (query.Count())
            {
                case 1:
                {
                    var currentPeriodSingle = query[0];

                    if (currentPeriodSingle.Volume12 >= threshold)
                    {
                        return true;
                    }
                }
                    break;
                case 2:
                {
                    var currentPeriod = query[0];
                    var previousPeriod = query[1];
                    var prvTotal = currentPeriod.Volume12 + previousPeriod.Volume12;

                    if (prvTotal >= threshold)
                    {
                        return true;
                    }
                }
                    break;
                case 3:
                {
                    var currentPeriodSingle = query[0];

                    if (currentPeriodSingle.Volume12 >= threshold)
                    {
                        return true;
                    }

                    var previousPeriod = query[1];
                    var period3 = query[2];
                    var prvTotal = previousPeriod.Volume12 + period3.Volume12;

                    if (prvTotal >= threshold)
                    {
                        return true;
                    }
                }
                    break;
            }

            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method returns the StyleAmbassador 50% off reward that is currently active
        /// </summary>
        private StyleAmbassadorReward GetActiveStyleAmbassadorReward()
        {
            //var context = Exigo.CreateODataContext<RewardsContext>(GlobalSettings.Exigo.Api.SandboxID);

            //var styleAmbassadorRewardSettings = context.StyleAmbassadorRewards.Where(c => c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now).FirstOrDefault();
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("StyleAmbassadorRewards '{0}'",DateTime.Now);
                var styleAmbassadorRewardSettings = context.Query<Common.Api.ExigoOData.Rewards.StyleAmbassadorReward>(SqlProcedure).FirstOrDefault();
              return styleAmbassadorRewardSettings;
            }
          
        }

        #endregion
    }
}