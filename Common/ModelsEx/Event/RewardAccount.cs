using ExigoService;

namespace Common.ModelsEx.Event
{
    public class RewardsAccount : CustomerPointAccount
    {
        public decimal AppliedAmount { get; set; }

        public decimal AmountRemaining
        {
            get
            {
                return (Balance - AppliedAmount);
            }
        }
        public string AppliedItemCode { get; set; }
    }
}