using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ServicesEx.Rewards
{
    public class NewStyleAmbassadorHalfOffReward : BaseStyleAmbassadorHalfOffReward
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
            get { return DiscountType.SAHalfOff; }
        }

        protected override int RewardItemCategory
        {
            get { return 40; }
        }

        protected override int? RewardPointsAccountId 
        { 
            get { return PointAccounts.StyleAmbassadorHalfOffRewards; }
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
                if (customer.CustomerTypeID == CustomerTypes.IndependentStyleAmbassador)
                {
                    // Subtracting a day to since Day 1 begins when the customer joins as a Style Ambassador (determined by Date1)
                    ExpirationDate = ThirtyDaysWhenCustomerBecameStyleAmbassador(customer);

                    bool first30Days = DateTime.Now.Date <= ExpirationDate;

                    var pointAccountResponse = Exigo.GetCustomerPointAccount(customer.CustomerID, RewardPointsAccountId.Value);

                    var creditsRemaining = (null != pointAccountResponse ? pointAccountResponse.Balance : 0M);

                    if (first30Days && creditsRemaining > 0.00M)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override void PopulateEligibleDiscounts(List<Product> products)
        {
            foreach (var product in products)
            {
                //if (product.EligibleDiscounts.Where(i => i.DiscountType.Equals(DiscountType.TenPersentPRV)).FirstOrDefault() != null) continue;
                var productDiscount = product.EligibleDiscounts.Where(i => i.DiscountType == DiscountType.SAHalfOff/*|| i.DiscountType == DiscountType.HalfOffCredits*/).FirstOrDefault();
                if (productDiscount == null) continue;
                product.ApplyDiscount(productDiscount);
                product.ApplyDiscountType = productDiscount.DiscountType;
            }
        }

        #endregion

    }
}