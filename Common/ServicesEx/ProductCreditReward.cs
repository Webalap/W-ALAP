using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

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
            //Only available in BACKOFFice

            if (siteType == "backOffice")
            {
                return true;
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