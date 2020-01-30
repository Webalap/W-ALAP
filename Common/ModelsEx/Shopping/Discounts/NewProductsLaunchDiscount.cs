namespace Common.ModelsEx.Shopping.Discounts
{
    public class NewProductsLaunchDiscount : PercentDiscount
    {
        public NewProductsLaunchDiscount() : this(0M) { }

        public NewProductsLaunchDiscount(decimal discountAmount)
            : base(DiscountType.NewProductsLaunchReward, 50M) 
        { }

        public override string Description
        {
            get { return string.Format("{0}% off Retail", DiscountAmount.ToString("F2")); }
        }
        public override string DisplayText { get { return "Discount"; } }

        public override string RewardProgram
        {
            get
            {
                if (DiscountType == DiscountType.NewProductsLaunchReward )
                {
                    return "50% Off";
                }

                return base.RewardProgram;
            }
        }
    }
}