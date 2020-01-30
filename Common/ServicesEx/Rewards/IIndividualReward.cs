using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using System.Collections.Generic;
using IOrder = Common.ModelsEx.Shopping.IOrder;

namespace Common.ServicesEx.Rewards
{
    public interface IIndividualReward
    {
        /// <summary>
        /// This method determines if all the eligiblity requirements are met for this reward.
        /// </summary>
        bool IsEligible(Customer customer, string siteType);

        /// <summary>
        /// This method populates the discounts to the product list for shopping.
        /// </summary>
        void PopulateEligibleDiscounts(List<Product> products);

        /// <summary>
        /// This method returns all active reward products for this reward.
        /// </summary>
        IList<Product> GetActiveRewardProducts(IList<Product> productsInShoppingCart);

        /// <summary>
        /// This method determines if the given discount is assocaited with this reward.
        /// </summary>
        bool IsRewardDiscount(Discount discount);

        /// <summary>
        /// This method determines if a reward product can be added to the current cart.
        /// </summary>
        bool AllowAddToCart(Product rewardProduct, List<Product> productsInShoppingCart);

       /// <summary>
        /// This method calculates the points account for this reward.
        /// </summary>
        RewardsAccount CalculatePointsAccount(IList<Product> productsInShoppingCart);

        /// <summary>
        /// This method verifies an order is valid to be submitted in regards to the reward.  An exception is thrown if the order is invalid.
        /// </summary>
        void VerifyOrderIsValid(IOrder order);

        /// <summary>
        /// This method is to create a request can that is eligible to participate in a transaction
        /// For example, if the redeeming of a reward is stored in a customer extended record then 
        /// the approprtiate request should be returned from this method.
        /// </summary>
        /// <returns></returns>
        // TODO: Needs to be implemented.
        //T CreateTransactionRequests<T>();

        /// <summary>
        /// This method executes immediately after an order has been submitted for any reward post processing required.
        /// </summary>
        void OrderPostProcessing(IOrder order);

        /// <summary>
        /// This method executes for BackOffice Shopping Controller
        /// </summary>
        //void VerifyOrderIsValid(IList<IShoppingCartItem> order);
    }
}
