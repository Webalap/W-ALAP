using ExigoService;
using System;

namespace Common.ModelsEx.Shopping.Discounts
{
    /// <summary>
    /// This represents a discount for a percentage off (e.g. 50% off).
    /// </summary>
    public class PercentDiscount : Discount
    {
        public PercentDiscount()
        { }


        internal PercentDiscount(decimal discountAmount)
            : base(DiscountType.Percent, discountAmount)
        { }

        internal PercentDiscount(DiscountType discountType, decimal discountAmount)
            : base(discountType, discountAmount)
        { }

        public override string Description { get { return "Percent"; } }
        public override string DisplayText { get { return "Percent"; } }

        public override decimal Apply(Product product)
        {
            decimal newPrice = product.Price;

            if (0M >= DiscountAmount)
                return newPrice;
            
           var discount = Math.Round(((product.Price * DiscountAmount) / 100), 2) ;

            newPrice = product.Price - discount;

            AppliedAmount = discount;

            return newPrice;
        }
        public override decimal Apply(Item product)
        {
            decimal newPrice = product.Price;

            if (0M >= DiscountAmount)
                return newPrice;

            var discount = Math.Round(((product.Price * DiscountAmount) / 100), 2);

            newPrice = product.Price - discount;

            AppliedAmount = discount;

            return newPrice;
        }
        public override decimal Apply(ShoppingCartItem item)
        {
            decimal newPrice = item.Price;

            if (0M >= DiscountAmount)
                return newPrice;

            var discount = Math.Round(((item.Price * DiscountAmount) / 100), 2);

            newPrice = item.Price - discount;

            AppliedAmount = discount;

            return newPrice;
        }

    }
}