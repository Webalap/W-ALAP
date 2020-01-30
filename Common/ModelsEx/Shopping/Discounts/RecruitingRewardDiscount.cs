﻿using System;

namespace Common.ModelsEx.Shopping.Discounts
{
    public class RecruitingRewardDiscount : FixedDiscount
    {
        public RecruitingRewardDiscount() : base() { }

        internal RecruitingRewardDiscount(decimal discountAmount)
            : base(DiscountType.RecruitingReward, discountAmount)
        { }

        public override string Description { get { return "Recruiting Rewards Cash"; } }
        public override string DisplayText { get { return "Cash"; } }
        public override bool OverrideTaxableAmount { get { return false; } }
        public int CustomerExtendedDetailId { get; set; }
        public DateTime CompletionDate { get; set; }
        public string ItemCode { get; set; }
        public bool HasBeenRedeemed { get; set; }

        public decimal RewardAmount { get; set; }
        
    }
}