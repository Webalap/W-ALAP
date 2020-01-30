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
    public abstract class BaseNewProductsLaunchReward : IIndividualReward
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

        public Api.ExigoOData.Rewards.NewProductsLaunchReward EligibleReward { get; set; }

        public int AvailableCredits { get; set; }

        #endregion

        #region Private Instance Variables

        private bool _isEligible;

        #endregion

        #region Abstract Protected Properties

        protected abstract DiscountType RewardNewProductsLaunchDiscountType { get; }

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
            _isEligible = EligibilityRequirements(customer, siteType);

            if (_isEligible)
            {
                CustomerId = customer.CustomerID;
            }

            return _isEligible;
        }

        /// <summary>
        /// This method populates the discounts to the product list for shopping.
        /// </summary>
        public virtual void PopulateEligibleDiscounts(List<Product> products)
        {
            if (!_isEligible)
            {
                throw new Exception("IsEligible must be called first to ensure the customer is eligilbe for any discounts.");
            }

            var eligibleCodes = EligibleItemCodes();

            foreach (var product in products)
            {
                if (eligibleCodes.Contains(product.ItemCode))
                {
                    product.EligibleDiscounts.Add(new DiscountFactory().CreateDiscount(RewardNewProductsLaunchDiscountType, 0M));
                }
            }
        }
        public void VerifyOrderIsValid(IList<IShoppingCartItem> order) 
        {
            throw new NotImplementedException();
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
            if (!_isEligible)
            {
                throw new Exception("IsEligible must be called first to ensure the customer is eligilbe for any discounts.");
            }

            if (IsRewardPointsAccount && CalculatePointsAccount(productsInShoppingCart).AmountRemaining <= 0)
                return new List<Product>();
            IList<string> newProductsLaunchItemCodesInShoppingCart = productsInShoppingCart.Where(p => p.Discounts.Any(d => d.DiscountType == RewardNewProductsLaunchDiscountType)).Select(p => p.ItemCode).ToList();
            var activeRewardProducts = EligibleProducts();

            // Filter out products that are already in the Shopping Cart with the New Product Launch Reward
            activeRewardProducts = activeRewardProducts.Where(p => !newProductsLaunchItemCodesInShoppingCart.Contains(p.ItemCode)).ToList();
            foreach (var product in activeRewardProducts)
            {
                product.EligibleDiscounts.Add(new DiscountFactory().CreateDiscount(RewardNewProductsLaunchDiscountType, 0M));
            }

            return activeRewardProducts;
        }

        /// <summary>
        /// This method determines if the given discount is assocaited with this reward.
        /// </summary>
        public bool IsRewardDiscount(Discount discount)
        {
            return discount.DiscountType == RewardNewProductsLaunchDiscountType;
        }

        /// <summary>
        /// This method determines if a reward product can be added to the current cart.
        /// </summary>
        public virtual bool AllowAddToCart(Product rewardProduct, List<Product> productsInShoppingCart)
        {
            if (!rewardProduct.NewProductsLaunchRewardsCreditsEligible)
            {
                // Product is not eligible for half off
                return false;
            }

            var creditsRemaining = CreditsRemaining(CustomerId, EligibleReward);
            var productWithDiscount = GetRewardProducts(productsInShoppingCart);
            var creditsAvailable = creditsRemaining - (productWithDiscount.Count() + 1);
            AvailableCredits = creditsAvailable;

            if (creditsAvailable <= 0)
            {
                // Not enough credits available
                return false;
            }

            var rewardProducts = GetRewardProducts(productsInShoppingCart);
 
            if (rewardProducts.Select(p => p.ItemCode).Contains(rewardProduct.ItemCode))
            {
                // The same product cannot be added to the cart multiple times with this reward applied
                return false;
            }

            if (IsRewardPointsAccount)
            {
                var rewardsAccount = CalculatePointsAccount(productsInShoppingCart);

                if (rewardsAccount.AmountRemaining <= 0)
                {
                    // A credit must be available to apply a reward to another product
                    return false;
                }
            }

            var alreadyPurchasedItemCodes = PurchasedItemCodes();

            return !alreadyPurchasedItemCodes.Contains(rewardProduct.ItemCode);
        }

        public virtual bool AllowAddToCart(Item rewardProduct, List<IShoppingCartItem> productsInShoppingCart)
        {

            var creditsRemaining = CreditsRemaining(CustomerId, EligibleReward);
            var productWithDiscount = GetRewardProducts(productsInShoppingCart);
            var creditsAvailable = creditsRemaining - (productWithDiscount.Count() + 1);
            AvailableCredits = creditsAvailable;

            if (creditsAvailable < 0)
            {
                // Not enough credits available
                return false;
            }

            var rewardProducts = GetRewardProducts(productsInShoppingCart);

            if (rewardProducts.Select(p => p.ItemCode).Contains(rewardProduct.ItemCode))
            {
                // The same product cannot be added to the cart multiple times with this reward applied
                return false;
            }

            var alreadyPurchasedItemCodes = PurchasedItemCodes();

            return !alreadyPurchasedItemCodes.Contains(rewardProduct.ItemCode);
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

                var account = new RewardsAccount { PointAccountID = RewardPointsAccountId.Value, Balance = creditsRemaining, AppliedAmount = productWithDiscount.Count() };

                return account;
            }

            return null;
        }

       

        /// <summary>
        /// This method verifies an order is valid to be submitted in regards to the reward.  An exception is thrown if the order is invalid.
        /// </summary>
        public void VerifyOrderIsValid(IOrder order)
        {
            var rewardProducts = GetRewardProducts(order.Products);

            if (!rewardProducts.Any()) return;
            if (CustomerId == 0)
            {
                throw new ApplicationException("CustomerId cannot be zero.");
            }

            // All products must be eligible for half off
            var productsNotEligible = rewardProducts.Where(p => !p.NewProductsLaunchRewardsCreditsEligible);
            if (productsNotEligible.Any())
            {
                throw new ApplicationException(string.Format("{0} is not not eligible for 50% Off credit.", productsNotEligible.First().Description));
            }

            // Quantities greater than one cannot be purchased using the reward
            IEnumerable<Product> productsWithMultipleRewards = rewardProducts.Where(p => p.Discounts.Count(d => d.DiscountType == RewardNewProductsLaunchDiscountType) > 1);
            IEnumerable<Product> productCredit = rewardProducts.Where(p => p.Discounts.Count(d => d.DiscountType.Equals(DiscountType.ProductCredit)) > 1);
            if (productsWithMultipleRewards.Count() > 1)
            {
                throw new ApplicationException("50% off reward cannot be applied multiple times to the same product.");
            }
            if (productCredit.Count()>1)
            {
                throw new ApplicationException("Product Credit cannot be applied multiple times to the same product.");
            }
            if (IsRewardPointsAccount)
            {
                // Credits cannot be exceeded (one credit per product purchased with this reward)
                var rewardsAccount = CalculatePointsAccount(order.Products);

                if (rewardsAccount.AmountRemaining < 0)
                {
                    throw new ApplicationException("You've exceeded your rewards balance.");
                }
            }

            var creditsRemaining = CreditsRemaining(CustomerId, EligibleReward);
            var productWithDiscount = GetRewardProducts(order.Products);
            var creditsAvailable = creditsRemaining - productWithDiscount.Count();
            AvailableCredits = creditsAvailable;
            if (creditsAvailable < 0)
            {
                // Not enough credits available
                throw new ApplicationException("You've exceeded your credit balance.");
            }

            // Product could not have been purchased before using this reward
            var previouslyPurchasedItemCodes = PurchasedItemCodes();
            
            var alreadyPurchasedProducts = rewardProducts.Where(p => previouslyPurchasedItemCodes.Contains(p.ItemCode));

            if (alreadyPurchasedProducts.Any())
            {
                throw new ApplicationException(string.Format("{0} has already been purchased with this reward.", alreadyPurchasedProducts.First().Description));
            }
        }

        /// <summary>
        /// This method executes immediately after an order has bene submitted for any reward post processing required.
        /// </summary>
        public virtual void OrderPostProcessing(IOrder order)
        {
            IList<Product> rewardProducts = GetRewardProducts(order.Products);

            if (rewardProducts.Count <= 0) return;
            if (CustomerId == 0)
            {
                throw new ApplicationException("CustomerId cannot be zero");
            }

            if (IsRewardPointsAccount)
            {
                // Deduct points
                var createPointTransactionRequest = new CreatePointTransactionRequest
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

        #endregion

        #region Abstract Protected Methods

        /// <summary>
        /// This method determines if all the eligiblity requirements are met for this reward.
        /// </summary>
        protected abstract bool EligibilityRequirements(Customer customer, string siteType);

        /// <summary>
        /// This method determines if the custoemr has any credits left to buy reward products
        /// </summary>
        /// <param name="customerID">logged in customer Id</param>
        /// <param name="reward">eligible reward</param>
        /// <returns>availlable credits</returns>
        protected abstract int CreditsRemaining(int customerID, Api.ExigoOData.Rewards.NewProductsLaunchReward reward);

        #endregion

        #region Protected Methods

        /// <summary>
        /// This method returns the date of the 30th day that the customer became a Style Ambassador.
        /// </summary>
        protected DateTime SAStartDate(Customer customer)
        {
            if (customer.Date1.HasValue)
            {
                return customer.Date1.Value.Date;
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// This method returns the reward products found in the passed in list of products.
        /// </summary>
        protected IList<Product> GetRewardProducts(IList<Product> products)
        {
            return products.Where(p => p.Discounts.Any(d => d.DiscountType == RewardNewProductsLaunchDiscountType)).ToList();
        }

        /// <summary>
        /// This method returns the reward products found in the passed in list of products.
        /// </summary>
        protected IList<IShoppingCartItem> GetRewardProducts(IList<IShoppingCartItem> products)
        {
            return products.Where(p => p.Discounts.Any(d => d.DiscountType == RewardNewProductsLaunchDiscountType)).ToList();
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
            var products = ProductService.GetProductsByCategory(RewardItemCategory);

            IList<Product> promotionProducts = products.Where(p => p.NewProductsLaunchRewardsCreditsEligible).ToList();

            return promotionProducts;

        }

        /// <summary>
        /// This method returns the list of item codes which the customer has already purchased.
        /// </summary>
        protected IList<string> PurchasedItemCodes()
        {
            List<string> itemCodes = new List<string>();

            var response = Exigo.GetCustomerExtendedDetails(CustomerId, (int)CustomerExtendedGroup.NewProductsLaunchPurchase);

            foreach(var row in response)
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
            var apiRequests = new List<ApiRequest>();

            foreach (var product in rewardProducts)
            {
                // Temporarily reset the quantity to one to ensure subtotal for the product is correct for the customer extended table
                var originalQuantity = product.Quantity;
                product.Quantity = 1;
                var customerExtendedRequest=new CreateCustomerExtendedRequest();
                if (product.ApplyDiscountType.Equals(DiscountType.NewProductsLaunchReward))
                {
                    customerExtendedRequest = new CreateCustomerExtendedRequest { CustomerID = CustomerId, ExtendedGroupID = (int)CustomerExtendedGroup.NewProductsLaunchPurchase, Field1 = product.ItemCode, Field2 = DateTime.Now.ToString(CultureInfo.InvariantCulture), Field3 = product.Subtotal.ToString("c") };
                }
                else if (product.ApplyDiscountType.Equals(DiscountType.ProductCredit))
                {
                    customerExtendedRequest = new CreateCustomerExtendedRequest { CustomerID = CustomerId, ExtendedGroupID = (int)CustomerExtendedGroup.ProductCredit, Field1 = product.ItemCode, Field2 = DateTime.Now.ToString(CultureInfo.InvariantCulture), Field3 = product.Subtotal.ToString("c") };
                }

                product.Quantity = originalQuantity;

                apiRequests.Add(customerExtendedRequest);
            }

            Api.ProcessTransaction(new TransactionalRequest { TransactionRequests = apiRequests.ToArray() });
        }


        #endregion
    }
}