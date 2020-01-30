using Common.Api.ExigoWebService;
using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using Ninject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IOrder = Common.ModelsEx.Shopping.IOrder;

namespace Common.ServicesEx.Rewards
{
    public class NewEbReward : IIndividualReward
    {
        #region Dependencies

        [Inject]
        public ExigoApi Api { get; set; }

        [Inject]
        public IRewardService RewardService { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        #endregion

        #region Instance Variables

        // Need to research this, required to be a public property at the moment I believe for deserialization 
        public int CustomerId { get; set; }

        #endregion

        #region Private Instance Variables

        private bool isEligible = false;

        #endregion

        #region Protected Methods
        /// <summary>
        /// This method returns the reward products found in the passed in list of products.
        /// </summary>
        protected IList<Product> GetRewardProducts(IList<Product> products)
        {
            return products.Where(p => p.Discounts.Any(d => d.DiscountType == DiscountType.EBRewards)).ToList();
        }

        /// <summary>
        /// This method returns the list of item codes which the customer has already purchased.
        /// </summary>
        protected IList<string> RedeemedandExpiredItemCodes()
        {
            List<string> itemCodes = new List<string>();
            var rewards = RewardService.GetCustomerEbRewardDiscounts(CustomerId);

            foreach (var reward in rewards)
            { 
                string sku = reward.ItemCode;

                // There shouldn't be any duplicates but it is better to perform the check just in case versus having the application crash...
                // AzamNote: This is where we let the reward be redeemable for 30 days after completion!!!
                if (!itemCodes.Contains(sku) && (reward.HasBeenRedeemed || DateTime.Now >= reward.CompletionDate))
                {
                    itemCodes.Add(sku);
                }
            }

            return itemCodes;
        }

        protected bool EligibilityRequirements(Customer customer, string siteType)
        {
            // Must be a Style Ambassador that meets one of the following conditions:
            // (1) joined on or after August 7, 2015
            // Reward is for the replicated site only (not applicable for event).  Included back office so we can show messaging to the customer when 
            // this reward is active

            var EBProgramStartDate = new DateTime(2015, 8, 7); //grandfathered people from august 7th onwards. Ideally, this should be a DB driven field, but alas!
            
            if (siteType != "rep" && siteType != "backOffice") return false;
            if (customer.CustomerTypeID != CustomerTypes.IndependentStyleAmbassador) return false;
            
            DateTime SAStartDate = WhenCustomerBecameStyleAmbassador(customer);

            return (SAStartDate >= EBProgramStartDate && ((DateTime.Now.Date.Subtract(SAStartDate.Date).TotalDays <= 100))) ;
        }

        /// <summary>
        /// This method returns the date of the 30th day that the customer became a Style Ambassador.
        /// </summary>
        protected DateTime WhenCustomerBecameStyleAmbassador(Customer customer)
        {
            return customer.Date1.HasValue ? customer.Date1.Value.Date : DateTime.Now.Date;
        }

        #endregion

        /// <summary>
        /// This method determines if all the eligiblity requirements are met for this reward.
        /// </summary>
        public bool IsEligible(Customer customer, string siteType)
        {
            isEligible = EligibilityRequirements(customer, siteType);
            //isEligible = true;
            if (isEligible)
            {
                CustomerId = customer.CustomerID;
            }

            return isEligible;
        }

        /// <summary>
        /// This method populates the discounts to the product list for shopping.
        /// </summary>
        public void PopulateEligibleDiscounts(List<Product> products)
        {
           //Do Nothing as i've added the Eligible Discounts in GetActiveRewardProducts
        }

        /// <summary>
        /// This method returns all active reward products for this reward.
        /// </summary>
        public IList<Product> GetActiveRewardProducts(IList<Product> productsInShoppingCart)
        {
            if (!isEligible)
            {
                throw new Exception("IsEligible must be called first to ensure the customer is eligible for any discounts.");
            }

            IList<string> ebRewardProductsInCart = productsInShoppingCart.Where(p => p.Discounts.Any(d => d.DiscountType == DiscountType.EBRewards)).Select(p => p.ItemCode).ToList();

            var activeRewardProducts = new List<Product>();
            var rewards = RewardService.GetCustomerEbRewardDiscounts(CustomerId)
                // AzamNote: This is where we let the reward be redeemable for 30 days after completion!!!
                .Where(ebr => !ebr.HasBeenRedeemed && ebr.CompletionDate >= DateTime.Now);
            foreach (var reward in rewards)
            {
                var product = ProductService.GetProductByItemCode(reward.ItemCode, returnLongDetail: false);
                if (ebRewardProductsInCart.Contains(product.ItemCode)) continue;
                var discount = new EBRewardDiscount()
                {
                    CustomerExtendedDetailId = reward.CustomerExtendedDetailId,
                    ItemCode = reward.ItemCode,
                    DiscountAmount = reward.DiscountPercent,
                    DiscountPercent = reward.DiscountPercent,
                    DiscountType = DiscountType.EBRewards,
                    PhaseNumber = reward.PhaseNumber,
                    HasBeenRedeemed = reward.HasBeenRedeemed,
                    CompletionDate = reward.CompletionDate,
                };

                product.EligibleDiscounts.Add(discount);
                activeRewardProducts.Add(product);
            }

            return activeRewardProducts;
        }

        /// <summary>
        /// This method determines if the given discount is assocaited with this reward.
        /// </summary>
        public bool IsRewardDiscount(Discount discount)
        {
            return discount.DiscountType == DiscountType.EBRewards;
        }

        /// <summary>
        /// This method determines if a reward product can be added to the current cart.
        /// </summary>
        public bool AllowAddToCart(Product rewardProduct, List<Product> productsInCart)
        {
            IList<Product> rewardProducts = GetRewardProducts(productsInCart);
            if (rewardProducts.Select(p => p.ItemCode).Contains(rewardProduct.ItemCode))
            {
                // The same product cannot be added to the cart multiple times with this reward applied
                return false;
            }

            var redeemedAndExpired = RedeemedandExpiredItemCodes();
            if (redeemedAndExpired.Contains(rewardProduct.ItemCode))
            {
                // This product has already been redeemed or is expired in a previuos order
                return false;
            }

           return true;
        }

        /// <summary>
        /// This method calculates the points account for this reward.
        /// </summary>
        public RewardsAccount CalculatePointsAccount(IList<Product> products)
        {
            //throw new NotImplementedException();
            return null;
        }

        /// <summary>
        /// This method verifies an order is valid to be submitted in regards to the reward.  An exception is thrown if the order is invalid.
        /// </summary>
        public void VerifyOrderIsValid(IOrder order)
        {
            IList<Product> rewardProducts = GetRewardProducts(order.Products);
            if (rewardProducts.Count <= 0) return;
            if (CustomerId == 0) throw new ApplicationException("CustomerId cannot be zero");

            // Quantities greater than one cannot be purchased using the reward
            IEnumerable<Product> productsWithMultipleRewards = rewardProducts.Where(p => p.Discounts.Count(d => d.DiscountType == DiscountType.EBRewards) > 1);

            if (productsWithMultipleRewards.Count() > 1)
            {
                throw new ApplicationException("Extraordinary Beginnings Reward cannot be applied multiple times to the same product.");
            }

            //Check for any rewards that have been redeemed or expired
            foreach (var rp in rewardProducts)
            {
                var discount = (EBRewardDiscount) rp.Discounts.First();
                if (discount.HasBeenRedeemed || DateTime.Now >= discount.CompletionDate)
                {
                    throw new ApplicationException(string.Format("Item {0} is not not eligible for Extraordinary Beginnings Reward", rp.ItemCode));
                }
            }
        }

        /// <summary>
        /// This method executes immediately after an order has bene submitted for any reward post processing required.
        /// </summary>
        public void OrderPostProcessing(IOrder order)
        {
            //Update the Customer Extended and set the Redeemed to true for the paticular Reward
            IList<Product> rewardProducts = GetRewardProducts(order.Products);

            if (rewardProducts.Count <= 0) return;
            if (CustomerId == 0) throw new ApplicationException("CustomerId cannot be zero");

            foreach (var rp in rewardProducts)
            {
                var discount = (EBRewardDiscount) rp.Discounts[0];

                //Update Customer Extended for each
                var result = Api.UpdateCustomerExtended(new UpdateCustomerExtendedRequest()
                {
                    CustomerID          = CustomerId,
                    CustomerExtendedID  = discount.CustomerExtendedDetailId,
                    ExtendedGroupID     = (int) CustomerExtendedGroup.EbRewards, 
                    //Field1            = discount.PhaseNumber.ToString(CultureInfo.InvariantCulture),
                    Field5              = 1.ToString(CultureInfo.InvariantCulture) //Set Redeemed to True
                });
            }
        }
    }
}