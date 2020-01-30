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
using System.Web;
using IOrder = Common.ModelsEx.Shopping.IOrder;


namespace Common.ServicesEx.Rewards
{
    public class RecruitingReward : IIndividualReward
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

        public int CustomerId { get; set; }

        #endregion

        #region Private Instance Variables

        private bool isEligible = false;

        #endregion

        #region Proctected Properties

        protected int? RewardPointsAccountId { get {return PointAccounts.RecruitingRewards; } }
        protected bool IsRewardPointsAccount { get { return RewardPointsAccountId.HasValue; } }


        #endregion

        #region IndividualReward-Methods

        protected IList<Product> GetRewardProducts(IList<Product> products)
        {
            return products.Where(p => p.Discounts.Any(d => d.DiscountType == DiscountType.RecruitingReward)).ToList();
        }

        protected IList<string> RedeemedandExpiredItemCodes()
        {
           
            List<string> itemCodes = new List<string>();
            //var rewards = RewardService.GetCustomerRecruitingRewardDiscounts(CustomerId);
            
            //foreach (var reward in rewards)
            //{
            //    string sku = reward.ItemCode;

            //    // There shouldn't be any duplicates but it is better to perform the check just in case versus having the application crash...
            //    // AzamNote: This is where we let the reward be redeemable for 30 days after completion!!!
            //    if (!itemCodes.Contains(sku) && (reward.HasBeenRedeemed || DateTime.Now >= reward.CompletionDate))
            //    {
            //        itemCodes.Add(sku);
            //    }
            //}

            return itemCodes;
        }
        public bool IsEligible(Customer customer, string siteType)
        {
            //We do not allows rewards for rep corporate identity
            if (customer.CustomerID < 999) return false;

            isEligible = EligibilityRequirements(customer, siteType);

            if (isEligible)
            {
                CustomerId = customer.CustomerID;
            }

            //Used for PopulateEligibleDiscounts
            HttpContext.Current.Cache["RecruitingRewardEligibility"] = isEligible;

            return isEligible;
        }

        protected bool EligibilityRequirements(Customer customer, string siteType)
        {
           var request = new GetPointAccountRequest()
            {
                CustomerID = customer.CustomerID,
                PointAccountID = PointAccounts.RecruitingRewards
            };
            var rewardAmount = Exigo.GetRecruitingReward(request);

            return rewardAmount > 0M;
        

        }

        protected DateTime WhenCustomerBecameStyleAmbassador(Customer customer)
        {
            return customer.Date1.HasValue ? customer.Date1.Value.Date.AddDays(-1) : DateTime.Now;
        }

        public void PopulateEligibleDiscounts(List<Product> products)
        {
            foreach (var product in products)
            {
                //if (product.EligibleDiscounts.Where(i => i.DiscountType.Equals(DiscountType.RecruitingReward)).FirstOrDefault() != null) continue;
                var productDiscount = product.EligibleDiscounts.Where(i=>i.DiscountType.Equals(DiscountType.RecruitingReward)).FirstOrDefault();
                if (productDiscount == null) return;
                product.ApplyDiscount(productDiscount);
                product.ApplyDiscountType = productDiscount.DiscountType;
            }
        }

        public IList<Product> GetActiveRewardProducts(IList<Product> productsInShoppingCart)
        {
            if (!isEligible)
            {
                throw new Exception("IsEligible must be called first to ensure the customer is eligible for any discounts.");
            }
            
            IList<string> recruitingRewardProductsInCart = productsInShoppingCart.Where(p => p.Discounts.Any(d => d.DiscountType == DiscountType.RecruitingReward)).Select(p => p.ItemCode).ToList();

            var activeRewardProducts = new List<Product>();

            //var rewards = RewardService.GetCustomerRecruitingRewardDiscounts(CustomerId);
            //    // AzamNote: This is where we let the reward be redeemable for 30 days after completion!!!
            //    //.Where(ebr => !ebr.HasBeenRedeemed); //&& ebr.CompletionDate >= DateTime.Now);
            //foreach (var reward in rewards)
            //{
            //    var product = ProductService.GetProductByItemCode(reward.ItemCode, returnLongDetail: false);
            //    if (recruitingRewardProductsInCart.Contains(product.ItemCode)) continue;
            //    var discount = new RecruitingRewardDiscount()
            //    {
            //        CustomerExtendedDetailId = reward.CustomerExtendedDetailId,
            //        ItemCode = reward.ItemCode,
            //        DiscountAmount = reward.DiscountAmount,
            //        DiscountType = DiscountType.RecruitingReward,
            //        HasBeenRedeemed = reward.HasBeenRedeemed,
            //        CompletionDate = reward.CompletionDate,
            //    };

            //    product.EligibleDiscounts.Add(discount);
            //    activeRewardProducts.Add(product);
            //}

            return activeRewardProducts;
        }

        public bool IsRewardDiscount(Discount discount)
        {
            return discount.DiscountType == DiscountType.RecruitingReward;
        }

        public bool AllowAddToCart(Product rewardProduct, List<Product> productsInCart)
        {
            //IList<Product> rewardProducts = GetRewardProducts(productsInCart);
            //if (rewardProducts.Select(p => p.ItemCode).Contains(rewardProduct.ItemCode))
            //{
            //    // The same product cannot be added to the cart multiple times with this reward applied
            //    return false;
            //}

            //var redeemedAndExpired = RedeemedandExpiredItemCodes();
            //if (redeemedAndExpired.Contains(rewardProduct.ItemCode))
            //{
            //    // This product has already been redeemed or is expired in a previuos order
            //    return false;
            //}

            return true;
        }

        public RewardsAccount CalculatePointsAccount(IList<Product> products)
        {
            var appliedAmount = 0M;
            if (IsRewardPointsAccount)
            {
                var pointAccountResponse = Exigo.GetCustomerPointAccount(CustomerId, RewardPointsAccountId.Value);

                var creditsRemaining = (null != pointAccountResponse ? pointAccountResponse.Balance : 0M);

                var productWithDiscount = GetRewardProducts(products);
                foreach (var product in productWithDiscount)
                {
                    if (product.Quantity < product.Discounts.Count)
                    {
                        throw new ApplicationException("Number of discounts applied cannot exceed the number of products.");
                    }

                    appliedAmount += product.Discounts.Where(discount => discount.DiscountType == DiscountType.RecruitingReward).Sum(discount => discount.AppliedAmount);
                }

                RewardsAccount account = new RewardsAccount { PointAccountID = RewardPointsAccountId.Value, Balance = creditsRemaining, AppliedAmount = appliedAmount };

                return account;
            }
            return null;
        }

        public void VerifyOrderIsValid(IOrder order)
        {
            IList<Product> rewardProducts = GetRewardProducts(order.Products);
            if (rewardProducts.Count <= 0) return;
            if (CustomerId == 0) throw new ApplicationException("CustomerId cannot be zero");

            // Quantities greater than one cannot be purchased using the reward
            IEnumerable<Product> productsWithMultipleRewards = rewardProducts.Where(p => p.Discounts.Count(d => d.DiscountType == DiscountType.RecruitingReward) > 1);

            if (productsWithMultipleRewards.Count() > 1)
            {
                throw new ApplicationException("Recruiting Reward cannot be applied multiple times to the same product.");
            }

            //Check for any rewards that have been redeemed or expired
            foreach (var rp in rewardProducts)
            {
                var discount = (RecruitingRewardDiscount)rp.Discounts.First();
                if (discount.HasBeenRedeemed)
                {
                    throw new ApplicationException(string.Format("Item {0} is not not eligible for Recruiting Reward.", rp.ItemCode));
                }
            }
        }

        /// <summary>
        /// This method adds the purchased reward products to the customer extended group for this rewards program.
        /// </summary>
        protected void AddProductsToPurchasedExtendedGroup(IEnumerable<Product> rewardProducts)
        {
            List<ApiRequest> apiRequests = new List<ApiRequest>();

            foreach (var product in rewardProducts)
            {
                // Temporarily reset the quantity to one to ensure subtotal for the product is correct for the customer extended table
                decimal originalQuantity = product.Quantity;
                product.Quantity = 1;

                var customerExtendedRequest = new CreateCustomerExtendedRequest { CustomerID = CustomerId, ExtendedGroupID = (int) CustomerExtendedGroup.RecruitingRewards, Field1 = product.ItemCode, Field2 = DateTime.Now.ToString(CultureInfo.InvariantCulture), Field3 = 1.ToString(CultureInfo.InvariantCulture), Field4 = product.Discounts[0].AppliedAmount.ToString("c") };

                product.Quantity = originalQuantity;

                apiRequests.Add(customerExtendedRequest);
            }

            Api.ProcessTransaction(new TransactionalRequest { TransactionRequests = apiRequests.ToArray() });
        }

        public void OrderPostProcessing(IOrder order)
        {

            var rewardProducts = GetRewardProducts(order.Products);

            if (rewardProducts.Count <= 0) return;
            if (CustomerId == 0)
            {
                throw new ApplicationException("CustomerId cannot be zero");
            }

            if (!IsRewardPointsAccount) return;
            foreach (var rp in rewardProducts)
            {
                var discount = (RecruitingRewardDiscount) rp.Discounts[0];

                // Deduct  reward cash
                var createPointTransactionRequest = new CreatePointTransactionRequest
                {
                    CustomerID = CustomerId,
                    PointAccountID = RewardPointsAccountId.Value,
                    TransactionType = PointTransactionType.Adjustment,
                    Amount = -1 * discount.AppliedAmount,
                    Reference =
                        string.Format("Applied Item Code: {0}",
                            string.Join(", ", rp.ItemCode))
                };

                var response = Api.CreatePointTransaction(createPointTransactionRequest);
            }
            AddProductsToPurchasedExtendedGroup(rewardProducts);
        }

        #endregion

    }
}