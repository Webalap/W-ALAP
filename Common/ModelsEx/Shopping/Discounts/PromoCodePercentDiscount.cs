namespace Common.ModelsEx.Shopping.Discounts
{
    public class PromoCodePercentDiscount : PercentDiscount
    {
        public PromoCodePercentDiscount() : this( 0M, null ) { }

        public PromoCodePercentDiscount(decimal discountAmount, string promoCode)
            : base(DiscountType.PromoCode, discountAmount)
        {
            PromoCode = promoCode;
        }
        public PromoCodePercentDiscount(decimal discountAmount, string promoCode, decimal? BV, decimal? CV)
            : base(DiscountType.PromoCode, discountAmount) 
        {
            OverrideBVAmount = BV;
            OverrideCVAmount = CV;
            PromoCode = promoCode;
        }


        public override string Description
        {
            get { return string.Format("{0}% off", DiscountAmount.ToString("F2")); }
        }
        public override string DisplayText { get { return "Promo Code"; } }
       // public override bool OverrideTaxableAmount { get { return true; } }

        public override string PromoCode { get; set; }
        public override decimal? OverrideBVAmount { get; set; }
        public override decimal? OverrideCVAmount { get; set; }

    }
}