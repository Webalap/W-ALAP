using System;

namespace Common.ModelsEx.Shopping.Discounts
{
    public class EBRewardDiscount : PercentDiscount
    {
        public EBRewardDiscount() : this(0M) { }

        public EBRewardDiscount(decimal discountAmount)
            : base(DiscountType.EBRewards, discountAmount)
        { }


        #region Properties

        public int CustomerExtendedDetailId { get; set; }
        public int PhaseNumber { get; set; } //Field1
        public DateTime CompletionDate { get; set; } //Field2
        public string ItemCode { get; set; } //Field3
        public decimal DiscountPercent { get; set; }  //Field4
        public bool HasBeenRedeemed { get; set; } //Field5

        #endregion

        public override string Description { get { return "Extraordinary Beginnings Reward"; } }
        public override string DisplayText { get { return "Extraordinary Beginnings"; } }
        public override string RewardProgram
        {
            get
            {
                return this.PhaseNumber > 0 ? string.Format("Step {0} Reward {1}% off Retail ", PhaseNumber, DiscountPercent.ToString("F2")) : base.RewardProgram;
            }
        }
    }
}