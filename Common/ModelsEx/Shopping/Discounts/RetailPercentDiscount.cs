namespace Common.ModelsEx.Shopping.Discounts
{
    public class RetailPercentDiscount : PercentDiscount
    {
        public RetailPercentDiscount() : this(0M) { }

        public RetailPercentDiscount(decimal discountAmount)
            : base(DiscountType.RetailPromoPercent, discountAmount) 
        { }

        public RetailPercentDiscount(decimal discountAmount, decimal? BV, decimal? CV)
            : base(DiscountType.RetailPromoPercent, discountAmount) 
        {
            OverrideBVAmount = BV;
            OverrideCVAmount = CV;
        }


        public override string Description
        {
            get { return string.Format("{0}% off Retail", DiscountAmount.ToString("F2")); }
        }
        public override string DisplayText { get { return "Discount"; } }
       // public override bool OverrideTaxableAmount { get { return true; } }
        public override decimal? OverrideBVAmount { get; set; }
        public override decimal? OverrideCVAmount { get; set; }
    }
}