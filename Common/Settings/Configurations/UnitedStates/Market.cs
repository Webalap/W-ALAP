using ExigoService;   

namespace Common
{
    public class UnitedStatesMarket : Market
    {
        public UnitedStatesMarket()
            : base()
        {
            Name        = MarketName.UnitedStates;
            Description = "United States";
            CookieValue = "US";
            CultureCode = "en-US";
            IsDefault   = true;
            CountryCode = CountryCodes.UnitedStates;
        }

        public override IMarketConfiguration GetConfiguration()
        {
            return new UnitedStatesConfiguration();
        }
    }
}