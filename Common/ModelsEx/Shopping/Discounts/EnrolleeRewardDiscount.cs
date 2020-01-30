using System;

namespace Common.ModelsEx.Shopping.Discounts
{
    public class EnrolleeRewardDiscount : FixedDiscount
    {
        public EnrolleeRewardDiscount() : base() { }

        internal EnrolleeRewardDiscount(decimal discountAmount)
            : base(DiscountType.EnrolleeReward, discountAmount)
        { }

        public override string Description { get { return "Enrollee Rewards Cash"; } }
        public override string DisplayText { get { return "Cash"; } }
        public override bool OverrideTaxableAmount { get { return false; } }
        public int CustomerExtendedDetailId { get; set; }
        public DateTime CompletionDate { get; set; }
        public string ItemCode { get; set; }
        public bool HasBeenRedeemed { get; set; }

        public decimal RewardAmount { get; set; }
        
    }
}