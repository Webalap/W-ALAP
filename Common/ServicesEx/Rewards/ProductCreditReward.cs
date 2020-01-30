using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Common.Api.ExigoWebService;
using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using Ninject;
using IOrder = Common.ModelsEx.Shopping.IOrder;

namespace Common.ServicesEx.Rewards
{
    public class ProductCreditReward : BaseStyleAmbassadorHalfOffReward
    {

        #region Instance Properties

        public DateTime ExpirationDate { get; private set; }

        public int DaysLeft
        {
            get { return (int)(ExpirationDate - DateTime.Now.Date).TotalDays; }
        }

        #endregion

        #region Implemented Protected Properties

        protected override DiscountType RewardHalfOffDiscountType
        {
            get { return DiscountType.ProductCredit; }
        }

        protected override int RewardItemCategory
        {
            get { return 40; }
            // still using Style Amb Half off Reward till Exigo haven't created New Point Account
        }

        protected override int? RewardPointsAccountId
        {
            get { return PointAccounts.StyleAmbassadorHalfOffRewards; }
            // still using Style Amb Half off Reward till Exigo haven't created New Point Account
        }

        #endregion

        #region Implemented Protected Abstract Methods

        /// <summary>
        /// This method determines if all the eligiblity requirements are met for this reward.
        /// </summary>
        protected override bool EligibilityRequirements(Customer customer, string siteType)
        {
            // Style Ambassador that joined within 30 days (determined by Date1).  Replicated site only (not applicable for event).
            // Added back office so we can show messaging to the customer when this reward is active

            if (siteType == "rep" || siteType == "backOffice")
            {
                //comenting this because no restriction for independent style ambassador
                //if (customer.CustomerTypeID == CustomerTypes.IndependentStyleAmbassador)
                //{
                    // Subtracting a day to since Day 1 begins when the customer joins as a Style Ambassador (determined by Date1)
                return true;
                //}
            }

            return false;
        }

        public override void PopulateEligibleDiscounts(List<Product> products)
        {
            foreach (var product in products)
            {
                //if (product.EligibleDiscounts.Where(i => i.DiscountType.Equals(DiscountType.TenPersentPRV)).FirstOrDefault() != null) continue;
                var productDiscount = product.EligibleDiscounts.Where(i => i.DiscountType == DiscountType.ProductCredit).FirstOrDefault();
                if (productDiscount == null) return;
                product.ApplyDiscount(productDiscount);
                product.ApplyDiscountType = productDiscount.DiscountType;
            }
        }
        #endregion
    }
}