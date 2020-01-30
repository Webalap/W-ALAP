using Common.Api.ExigoWebService;
using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using IOrder = Common.ModelsEx.Shopping.IOrder;

namespace Common.ServicesEx.Rewards
{
    public abstract class BaseStyleAmbassadorHalfOffReward : IIndividualReward
    {
        #region Dependencies

        [Inject]
        public ExigoApi Api { get; set; }

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

        #region Abstract Protected Properties

        protected abstract DiscountType RewardHalfOffDiscountType { get; }

        protected abstract int RewardItemCategory { get; }

        protected abstract int? RewardPointsAccountId { get; }

        #endregion

        #region Protected Properties

        protected bool IsRewardPointsAccount { get { return RewardPointsAccountId.HasValue; } }

        #endregion

        #region IIndividualReward Methods

        /// <summary>
        /// This method determines if all the eligiblity requirements are met for this reward.
        /// </summary>
        public bool IsEligible(Customer customer, string siteType)
        {
            isEligible = EligibilityRequirements(customer, siteType);

            if (isEligible)
            {
                CustomerId = customer.CustomerID;
            }

            return isEligible;
        }

        /// <summary>
        /// This method populates the discounts to the product list for shopping.
        /// </summary>
        public virtual void PopulateEligibleDiscounts(List<Product> products)
        {
            if (!isEligible)
            {
                throw new Exception("IsEligible must be called first to ensure the customer is eligilbe for any discounts.");
            }

            var eligibleCodes = EligibleItemCodes();

            foreach (Product product in products)
            {
                if (eligibleCodes.Contains(product.ItemCode))
                {
                    product.EligibleDiscounts.Add(new DiscountFactory().CreateDiscount(RewardHalfOffDiscountType, 0M));
                }
            }
        }

        public List<Product> PopulateRewardEligibleDiscounts(List<Product> products)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns all active reward products for this reward.
        /// </summary>
        public IList<Product> GetActiveRewardProducts(IList<Product> productsInShoppingCart)
        {
            if (!isEligible)
            {
                throw new Exception("IsEligible must be called first to ensure the customer is eligilbe for any discounts.");
            }

            if (IsRewardPointsAccount && CalculatePointsAccount(productsInShoppingCart).AmountRemaining <= 0)
                return new List<Product>();
            IList<string> saHalfOffItemCodesInShoppingCart = productsInShoppingCart.Where(p => p.Discounts.Any(d => d.DiscountType == RewardHalfOffDiscountType)).Select(p => p.ItemCode).ToList();

            var activeRewardProducts = EligibleProducts();

            // Filter out products that are already in the Shopping Cart with the SA Half Off Reward
            activeRewardProducts = activeRewardProducts.Where(p => !saHalfOffItemCodesInShoppingCart.Contains(p.ItemCode)).ToList();

            foreach (var product in activeRewardProducts)
            {
                product.EligibleDiscounts.Add(new DiscountFactory().CreateDiscount(RewardHalfOffDiscountType, 0M));
            }

            return activeRewardProducts;
        }

        /// <summary>
        /// This method determines if the given discount is assocaited with this reward.
        /// </summary>
        public bool IsRewardDiscount(Discount discount)
        {
            return discount.DiscountType == RewardHalfOffDiscountType;
        }

        /// <summary>
        /// This method determines if a reward product can be added to the current cart.
        /// </summary>
        public virtual bool AllowAddToCart(Product rewardProduct, List<Product> productsInShoppingCart)
        {
            if (!rewardProduct.HalfOffRewardsCreditsEligible)
            {
                // Product is not eligible for half off
                return false;
            }

            IList<Product> rewardProducts = GetRewardProducts(productsInShoppingCart);
 
            if (rewardProducts.Select(p => p.ItemCode).Contains(rewardProduct.ItemCode))
            {
                // The same product cannot be added to the cart multiple times with this reward applied
                return false;
            }

            if (IsRewardPointsAccount)
            {
                RewardsAccount rewardsAccount = CalculatePointsAccount(productsInShoppingCart);

                if (rewardsAccount.AmountRemaining <= 0)
                {
                    // A credit must be available to apply a reward to a another product
                    return false;
                }
            }

            List<string> alreadyPurchasedItemCodes = PurchasedItemCodes();
            alreadyPurchasedItemCodes.AddRange(productsInShoppingCart.Select(i => i.ItemCode).ToList());
            if (alreadyPurchasedItemCodes.Contains(rewardProduct.ItemCode))
            {
                // This product has already been purchased in a previuos order
                return false;
            }

            return true;
        }

        public virtual bool AllowAddToCart(Item rewardProduct, List<IShoppingCartItem> productsInShoppingCart)
        {
            
            var rewardProducts = GetRewardProducts(productsInShoppingCart);

            if (rewardProducts.Select(p => p.ItemCode).Contains(rewardProduct.ItemCode))
            {
                // The same product cannot be added to the cart multiple times with this reward applied
                return false;
            }


            if (IsRewardPointsAccount)
            {
                RewardsAccount rewardsAccount = CalculatePointsAccount(productsInShoppingCart);

                if (rewardsAccount.AmountRemaining <= 0)
                {
                    // A credit must be available to apply a reward to a another product
                    return false;
                }
            }

            List<string> alreadyPurchasedItemCodes = PurchasedItemCodes();
            alreadyPurchasedItemCodes.AddRange(productsInShoppingCart.Select(i => i.ItemCode).ToList());
            if (alreadyPurchasedItemCodes.Contains(rewardProduct.ItemCode))
            {
                // This product has already been purchased in a previuos order
                return false;
            }

            return true;
        }

        /// <summary>
        /// This method calculates the points account for this reward.
        /// </summary>
        public RewardsAccount CalculatePointsAccount(IList<Product> productsInShoppingCart)
        {
            if (IsRewardPointsAccount)
            {
                var pointAccountResponse = Exigo.GetCustomerPointAccount(CustomerId, RewardPointsAccountId.Value);

                var creditsRemaining = (null != pointAccountResponse ? pointAccountResponse.Balance : 0M);

                var productWithDiscount = GetRewardProducts(productsInShoppingCart);

                RewardsAccount account = new RewardsAccount { PointAccountID = RewardPointsAccountId.Value, Balance = creditsRemaining, AppliedAmount = productWithDiscount.Count() };

                return account;
            }

            return null;
        }

        /// <summary>
        /// This method calculates the points account for this reward.
        /// </summary>
        public RewardsAccount CalculatePointsAccount(IList<IShoppingCartItem> productsInShoppingCart)
        {
            if (IsRewardPointsAccount)
            {
                var pointAccountResponse = Exigo.GetCustomerPointAccount(CustomerId, RewardPointsAccountId.Value);

                var creditsRemaining = (null != pointAccountResponse ? pointAccountResponse.Balance : 0M);

                var productWithDiscount = GetRewardProducts(productsInShoppingCart);

                RewardsAccount account = new RewardsAccount { PointAccountID = RewardPointsAccountId.Value, Balance = creditsRemaining, AppliedAmount = productWithDiscount.Count() };

                return account;
            }

            return null;
        }

        /// <summary>
        /// This method verifies an order is valid to be submitted in regards to the reward.  An exception is thrown if the order is invalid.
        /// </summary>
        public void VerifyOrderIsValid(IOrder order)
        {
            IList<Product> rewardProducts = GetRewardProducts(order.Products);

            if (rewardProducts.Count() > 0)
            {
                if (CustomerId == 0)
                {
                    throw new ApplicationException("CustomerId cannot be zero");
                }

                // All products must be eligible for half off
                IEnumerable<Product> productsNotEligibleHalfOff = rewardProducts.Where(p => !p.HalfOffRewardsCreditsEligible);
                if (productsNotEligibleHalfOff.Count() > 0)
                {
                    throw new ApplicationException(string.Format("{0} is not not eligible for half off credit", productsNotEligibleHalfOff.First().Description));
                }

                // Quantities greater than one cannot be purchased using the reward
                IEnumerable<Product> productsWithMultipleRewards = rewardProducts.Where(p => p.Discounts.Where(d => d.DiscountType == RewardHalfOffDiscountType).Count() > 1);

                if (productsWithMultipleRewards.Count() > 1)
                {
                    throw new ApplicationException("Half Off Discount cannot be applied multiple times to the same product.");
                }

                if (IsRewardPointsAccount)
                {
                    // Credits cannot be exceeded (one credit per product purchased with this reward)
                    RewardsAccount rewardsAccount = CalculatePointsAccount(order.Products);

                    if (rewardsAccount.AmountRemaining < 0)
                    {
                        throw new ApplicationException("You've exceeded your rewards balance");
                    }
                }

                // Product could not have been purchased before using this reward
                var previouslyPurchasedItemCodes = PurchasedItemCodes();

                IEnumerable<Product> alreadyPurchasedProducts = order.Products.Where(p => previouslyPurchasedItemCodes.Contains(p.ItemCode));

                if (alreadyPurchasedProducts.Count() > 0)
                {
                    throw new ApplicationException(string.Format("{0} has already been purchased with this reward", alreadyPurchasedProducts.First().Description));
                }
            }
        }

        /// <summary>
        /// This method executes immediately after an order has bene submitted for any reward post processing required.
        /// </summary>
        public virtual void OrderPostProcessing(IOrder order)
        {
            IList<Product> rewardProducts = GetRewardProducts(order.Products);

            if (rewardProducts.Count > 0)
            {
                if (CustomerId == 0)
                {
                    throw new ApplicationException("CustomerId cannot be zero");
                }

                if (IsRewardPointsAccount)
                {
                    // Deduct points
                    CreatePointTransactionRequest createPointTransactionRequest = new CreatePointTransactionRequest
                    {
                        CustomerID = CustomerId,
                        PointAccountID = RewardPointsAccountId.Value,
                        TransactionType = PointTransactionType.Adjustment,
                        Amount = -1 * rewardProducts.Count(),
                        Reference = string.Format("Applied Item Code: {0}", string.Join(", ", rewardProducts.Select(p => p.ItemCode)))
                    };

                    var response = Api.CreatePointTransaction(createPointTransactionRequest);
                }

                AddProductsToPurchasedExtendedGroup(rewardProducts);
            }
        }

        #endregion

        #region Abstract Protected Methods

        /// <summary>
        /// This method determines if all the eligiblity requirements are met for this reward.
        /// </summary>
        protected abstract bool EligibilityRequirements(Customer customer, string siteType);

        #endregion

        #region Protected Methods

        /// <summary>
        /// This method returns the date of the 30th day that the customer became a Style Ambassador.
        /// </summary>
        protected DateTime ThirtyDaysWhenCustomerBecameStyleAmbassador(Customer customer)
        {
            if (customer.Date1.HasValue)
            {
                // Subtracting a day to since Day 1 begins when the customer joins as a Style Ambassador (determined by Date1)
                return customer.Date1.Value.Date.AddDays(30);
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// This method returns the reward products found in the passed in list of products.
        /// </summary>
        protected IList<Product> GetRewardProducts(IList<Product> products)
        {
            return products.Where(p => p.Discounts.Any(d => d.DiscountType == RewardHalfOffDiscountType)).ToList();
        }

        protected IList<IShoppingCartItem> GetRewardProducts(IList<IShoppingCartItem> products)
        {
            var discountedProductsInCart =
                products.Where(p => p.Discounts.Any(d => d.DiscountType == RewardHalfOffDiscountType)).ToList();
            return discountedProductsInCart;
        }

        /// <summary>
        /// This method returns the list of products that are eligible so any item codes that the promotion contains that has yet to be purchased by the customer.
        /// </summary>
        /// <returns></returns>
        protected IList<Product> EligibleProducts()
        {
            IList<Product> promotionProducts = PromotionProducts();
            IList<string> purchasedItemCodes = PurchasedItemCodes();

            return promotionProducts.Where(p => !purchasedItemCodes.Contains(p.ItemCode)).ToList();
        }

        /// <summary>
        /// This method returns the list of item codes that are eligible so any item codes that the promotion contains that has yet to be purchased by the customer.
        /// </summary>
        /// <returns></returns>
        protected IList<string> EligibleItemCodes()
        {
            IList<Product> eligibleProducts = EligibleProducts();

            return eligibleProducts.Select(p => p.ItemCode).ToList();
        }

        /// <summary>
        /// This method returns the list of products which the promotion contains.
        /// </summary>
        /// <returns></returns>
        protected IList<Product> PromotionProducts()
        {
            IEnumerable<Product> products = ProductService.GetProductsByCategory(RewardItemCategory); //, includeBookingRewards:false);

            IList<Product> promotionProducts = products.Where(p => p.HalfOffRewardsCreditsEligible).ToList();

            return promotionProducts;
        }

        /// <summary>
        /// This method returns the list of item codes which the customer has already purchased.
        /// </summary>
        protected List<string> PurchasedItemCodes()
        {
            List<string> itemCodes = new List<string>();

            var response = Exigo.GetCustomerExtendedDetails(CustomerId, (int)CustomerExtendedGroup.HalfOffPurchase);

            foreach (var row in response)
            {
                var sku = row.Field1;

                // There shouldn't be any duplicates but it is better to perform the check just in case versus having the application crash...
                if (!itemCodes.Contains(sku))
                {
                    itemCodes.Add(sku);
                }
            }

            return itemCodes;

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

                var customerExtendedRequest = new CreateCustomerExtendedRequest { CustomerID = CustomerId, ExtendedGroupID = (int)CustomerExtendedGroup.HalfOffPurchase, Field1 = product.ItemCode, Field2 = DateTime.Now.ToString(), Field3 = product.Subtotal.ToString("c") };

                product.Quantity = originalQuantity;

                apiRequests.Add(customerExtendedRequest);
            }

            Api.ProcessTransaction(new TransactionalRequest { TransactionRequests = apiRequests.ToArray() });
        }

        #endregion
    }
}