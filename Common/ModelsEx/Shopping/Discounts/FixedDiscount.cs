using ExigoService;

namespace Common.ModelsEx.Shopping.Discounts
{
    /// <summary>
    /// This represents a discount of a fixed amount (e.g. $10 off).
    /// </summary>
    public class FixedDiscount : Discount
    {
        public FixedDiscount() : base() { }

        internal FixedDiscount(decimal discountAmount)
            : base(DiscountType.Fixed, discountAmount)
        { }

        internal FixedDiscount(DiscountType discountType, decimal discountAmount)
            : base(discountType, discountAmount)
        { }

        public override string Description { get { return "Fixed"; } }
        public override string DisplayText { get { return "Fixed"; } }

        public override decimal Apply(Product product)
        {
            decimal newPrice = product.Price;

            if (0M >= DiscountAmount)
                return newPrice;

            if (DiscountAmount >= product.Price)
            {
                newPrice = 0M;
                AppliedAmount = product.Price;
            }
            else
            {
                newPrice = product.Price - DiscountAmount;
                AppliedAmount = DiscountAmount;
            }

            return newPrice;
        }

        public override decimal Apply(Item product)
        {
            decimal newPrice = product.Price;

            if (0M >= DiscountAmount)
                return newPrice;

            if (DiscountAmount >= product.Price)
            {
                newPrice = 0M;
                AppliedAmount = product.Price;
            }
            else
            {
                newPrice = product.Price - DiscountAmount;
                AppliedAmount = DiscountAmount;
            }

            return newPrice;
        }
        /// <summary>
        /// Subtracts <paramref name="amount"/> from this amount
        /// and returns a new Discount for the difference.
        /// </summary>
        public override decimal Apply(ShoppingCartItem product)
        {
            decimal newPrice = product.Price;

            if (0M >= DiscountAmount)
                return newPrice;

            if (DiscountAmount >= product.Price)
            {
                newPrice = 0M;
                AppliedAmount = product.Price;
            }
            else
            {
                newPrice = product.Price - DiscountAmount;
                AppliedAmount = DiscountAmount;
            }

            return newPrice;
        } 
        /// <summary>
        /// Subtracts <paramref name="amount"/> from this amount
        /// and returns a new Discount for the difference.
        /// </summary>
        /// <param name="amount">Amount to subtract.</param>
        /// <returns>A new Discount for the difference.</returns>
        //public override Discount Subtract(decimal amount)
        //{
        //    if (amount >= DiscountAmount)
        //        throw new ArgumentOutOfRangeException("amount", "amount must be less than DiscountAmount.");

        //    // Reduce this discount's amount.
        //    DiscountAmount -= amount;

        //    // Return a new discount for the original amount.
        //    return new DiscountFactory().CreateDiscount(DiscountType, amount);
        //}

        /// <summary>
        /// Gets the product total after applying the discount.
        /// </summary>
        //public override decimal Total
        //{
        //    get
        //    {
        //        if (Product == null) return 0;

        //        return Product.SubTotal - Math.Min(DiscountAmount, Product.SubTotal);
        //    }
        //}

        //public override string ToString()
        //{
        //    if (Product == null)
        //    {
        //        return string.Format("No product to apply {0:C} to.", DiscountAmount);
        //    }

        //    return string.Format(
        //        "{0:C} - {1:C} = {2:C}",
        //        Product.SubTotal,
        //        DiscountAmount,
        //        Total
        //    );
        //}
    }
}