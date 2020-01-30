namespace Common.ModelsEx.Shopping.Discounts
{
    public class HalfOffDiscount : PercentDiscount
    {
        internal HalfOffDiscount(DiscountType discountType)
            : base(discountType, 50M) // Azam - Changed from 0.5M to 50M. Done to get around the issue of different perecntage types being used across the app!
        { }

        public override string Description { get { return "1/2 Off Credit"; } }
        public override string DisplayText { get { return "Credit"; } }

        public override string RewardProgram
        {
            get
            {
                if (this.DiscountType == Discounts.DiscountType.SAHalfOff || DiscountType == Discounts.DiscountType.SAHalfOffOngoing)
                {
                    return "50% Off Sample";
                }
                
                return base.RewardProgram;
            }
        }
    }
}