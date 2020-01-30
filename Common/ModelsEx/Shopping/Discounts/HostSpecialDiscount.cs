using System;
using System.Collections.Generic;
using Common.Api.ExigoOData.Rewards;
using Common.Api.ExigoWebService;
using Common.ServicesEx.Rewards;
using ExigoService;

namespace Common.ModelsEx.Shopping.Discounts
{
    public class HostSpecialDiscount : PercentDiscount, IIndividualReward
    {
        #region Constructors

        public HostSpecialDiscount() : this(0M) { }

        public HostSpecialDiscount(decimal discountAmount)
            : base(DiscountType.HostSpecial, discountAmount)
        {
        }

        #endregion

        #region Type Cast Operators

        public static explicit operator HostSpecialDiscount(HostSpecial hostSpecial)
        {
            var hostSpecialReward = new HostSpecialDiscount();
            if (hostSpecial != null)
            {
                hostSpecialReward.HostSpecialID = hostSpecial.HostSpecialID;
                hostSpecialReward.ItemCode = hostSpecial.ItemCode;
                hostSpecialReward.DiscountAmount = hostSpecial.DiscountPercent;
                hostSpecialReward.SalesThreshold = hostSpecial.SalesThreshold;
                hostSpecialReward.StartDate = hostSpecial.StartDate;
                hostSpecialReward.EndDate = hostSpecial.EndDate;
                hostSpecialReward.HasBeenRedeemed = false;
            }
             
            return hostSpecialReward;
        }

        public static explicit operator HostSpecialDiscount(CustomerExtendedResponse response)
        {
            string itemCode = response.Field8;

            // Assuming if the Item Code is populated then all other fields for the reward are correctly populated as well (DiscountAmount, Redeemed, and SalesThreshold)
            if (!string.IsNullOrWhiteSpace(itemCode))
            {
                var hostSpecialReward = new HostSpecialDiscount
                {
                    CustomerExtendedDetailId = response.CustomerExtendedID,
                    ItemCode = itemCode,
                    DiscountAmount = decimal.Parse(response.Field9),
                    HasBeenRedeemed = !string.IsNullOrEmpty(response.Field10) && response.Field10 != "FALSE" && int.Parse(response.Field10) == 1,
                    SalesThreshold = decimal.Parse(response.Field11)
                };

                return hostSpecialReward;
            }

            return null;
        }


        #endregion

        #region Properties

        public int CustomerExtendedDetailId { get; set; }
        public Guid HostSpecialID { get; set; }
        public string ItemCode { get; set; }
        public decimal SalesThreshold { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool HasBeenRedeemed { get; set; }
        
        #endregion
        
        #region PercentDiscount Overrides

        public override string Description { get { return "Host Special Reward"; } }
        public override string DisplayText { get { return "Host Special"; } }
        
        #endregion

        #region IIndividualReward Implementation

        public bool IsEligible(ExigoService.Customer customer, string siteType)
        {
            throw new NotImplementedException();
        }

        public void PopulateEligibleDiscounts(List<Product> products)
        {
            throw new NotImplementedException();
        }

        public IList<Product> GetActiveRewardProducts(IList<Product> productsInShoppingCart)
        {
            throw new NotImplementedException();
        }
        public IList<Product> GetActiveRewardProducts(ShoppingCartItem productsInShoppingCart)
        {
            throw new NotImplementedException();
        }
        public bool IsRewardDiscount(Discount discount)
        {
            throw new NotImplementedException();
        }

        public bool AllowAddToCart(Product rewardProduct, List<Product> productsInShoppingCart)
        {
            throw new NotImplementedException();
        }

        public Event.RewardsAccount CalculatePointsAccount(IList<Product> productsInShoppingCart)
        {
            throw new NotImplementedException();
        }

        public void VerifyOrderIsValid(IOrder order)
        {
            throw new NotImplementedException();
        }

        public T CreateTransactionRequests<T>()
        {
            throw new NotImplementedException();
        }

        public void OrderPostProcessing(IOrder order)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}