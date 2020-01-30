using Common;
using System.Linq;

namespace ExigoService
{
    public class Market : IMarket
    {
        public Market()
        {
            this.Configuration = GetConfiguration();
        }

        public MarketName Name { get; set; }
        public string Description { get; set; }
        public string CookieValue { get; set; }
        public string CultureCode { get; set; }
        public bool IsDefault { get; set; }
        public string CountryCode { get; set; }
        public Country Country { 
            get 
            { 
                return 
                    ExigoService.Exigo.GetCountries()
                        .Where(c => c.CountryCode == CountryCode)
                        .FirstOrDefault(); 
            } 
        } 
        public CountryRegionCollection CountryRegions 
        { 
            get 
            { 
                return 
                    ExigoService.Exigo.GetCountryRegions(
                        CountryCode
                        ); 
            } 
        }
        public IMarketConfiguration Configuration { get; set; }
        public virtual IMarketConfiguration GetConfiguration()
        {
            return new UnitedStatesConfiguration();
        }
    }
}