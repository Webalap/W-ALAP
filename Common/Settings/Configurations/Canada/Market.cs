using ExigoService;

namespace Common
{
    public class CanadaMarket : Market
    {
        public CanadaMarket()
            : base()
        {
            Name = MarketName.Canada;
            Description = "Canada";
            CookieValue = "CA";
            CultureCode = "en-US";
            CountryCode = CountryCodes.Canada;
        }

        public override IMarketConfiguration GetConfiguration()
        {
            return new CanadaConfiguration();
        }
    }
}