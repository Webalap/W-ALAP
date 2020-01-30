using Common;

namespace ExigoService
{
    public interface IMarket
    {
        MarketName Name { get; set; }
        string Description { get; set; }
        string CookieValue { get; set; }
        string CultureCode { get; set; }
        bool IsDefault { get; set; }
        string CountryCode { get; set; }

        IMarketConfiguration Configuration { get; set; }
    }
}