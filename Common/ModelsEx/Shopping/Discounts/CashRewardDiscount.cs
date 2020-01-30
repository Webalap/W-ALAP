namespace Common.ModelsEx.Shopping.Discounts
{
    public class CashRewardDiscount : FixedDiscount
    {
        public CashRewardDiscount() : base() { }

        internal CashRewardDiscount(decimal discountAmount)
            : base(DiscountType.RewardsCash, discountAmount)
        { }

        public override string Description { get { return "Rewards Cash"; } }
        public override string DisplayText { get { return "Cash"; } }
        public override bool OverrideTaxableAmount { get { return false; } }
    }
}