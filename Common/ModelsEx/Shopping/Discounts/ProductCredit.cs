namespace Common.ModelsEx.Shopping.Discounts
{
    public class ProductCredit : FixedDiscount
    {
        public ProductCredit() : this(0M) { }

        public ProductCredit(decimal discountAmount)
            : base(DiscountType.ProductCredit, discountAmount) 
        { }

        public override string Description
        {
            get { return string.Format("Product Credit"); }
        }
        public override string DisplayText { get { return "Product Credit Discount"; } }

        public override string RewardProgram
        {
            get
            {
                if (DiscountType == DiscountType.ProductCredit )
                {
                    return "Reward Products";
                }

                return base.RewardProgram;
            }
        }
    }
}