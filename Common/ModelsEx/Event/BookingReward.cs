using ExigoService;

namespace Common.ModelsEx.Event
{
    public class BookingReward : CustomerPointAccount
    {
        public string PartyName { get; set; }
        public string WebAlias { get; set; }
        public int BookingRewardOwner { get; set; }
    }
}