namespace Common.ModelsEx.Shopping.Discounts
{
    public class RetailFixedDiscount : FixedDiscount
    {
        public RetailFixedDiscount() : this(0M) { }
        public RetailFixedDiscount(decimal discountAmount)
            : base(DiscountType.RetailPromoFixed, discountAmount)
        { }

        public RetailFixedDiscount(decimal discountAmount, decimal? BV , decimal? CV)
            : base(DiscountType.RetailPromoFixed, discountAmount)
        {
            OverrideBVAmount = BV;
            OverrideCVAmount = CV;
        }

        public override string Description { get { return string.Format("${0} off Retail", DiscountAmount.ToString("F2")); } }
        public override string DisplayText { get { return "Discount"; } }
        //public override bool OverrideTaxableAmount { get { return false; } }
        public override decimal? OverrideBVAmount { get; set; }
        public override decimal? OverrideCVAmount { get ; set; }
    }
}