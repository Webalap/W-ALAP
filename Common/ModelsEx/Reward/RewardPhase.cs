using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;

namespace Common.ModelsEx.Reward
{
    public class RewardPhase
    {
        public int PhaseNum { get; set; }
        public double TotalDays { get; set; }
        public double TotalDaysEnd { get; set; }
        public decimal AmountCompleted { get; set; }
        public decimal PercentCompleted { get; set; }
        public decimal CurrentVolume { get; set; }
        public decimal SalesThreshold { get; set; }
        public EBRewardDiscount Reward { get; set; }
        public Product Product { get; set; }
    }
}