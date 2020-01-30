using Common.Api.ExigoWebService;
using ExigoService;
using System.IO;
using System.Net;
using System.Web;

namespace Common
{
    public class UnitedStatesConfiguration : IMarketConfiguration
    {
        private MarketName marketName = MarketName.UnitedStates;

        public MarketName MarketName
        {
            get
            {
                return marketName;
            }
        }
        public IOrderConfiguration Orders
        {
            get
            {
                return new OrderConfiguration();
            }
        }
        public IOrderConfiguration CustomerOrders
        {
            get
            {
                return new CustomerOrderConfiguration();
            }
        }
        public IAutoOrderConfiguration AutoOrders
        {
            get
            {
                return new AutoOrderConfiguration();
            }
        }

        // TODO: gwb - Set item defaults here by country
        // ToDo: taj - set the pricetype based on customer type
        public class OrderConfiguration : IOrderConfiguration
        {
            public virtual int WarehouseID { get { return GetCountryCodeByIP() == CountryCodes.Canada ? Warehouses.Canada : Warehouses.DefaultUSA; } }
            public virtual string CurrencyCode { get { return CurrencyCodes.DollarsUS; } }
            public virtual int PriceTypeID { get { return PriceTypes.Wholesale; } }
            public virtual int LanguageID { get { return Languages.English; } }
            public virtual string DefaultCountryCode { get { return "US"; } }
            public virtual int DefaultShipMethodID { get { return WarehouseID == Warehouses.Canada ? 14 : 2; } }
            public virtual int StarterKitSubscriptionWarehouseID { get { return Warehouses.StarterKitsandSubscriptionsUSA; } }
            public virtual int CategoryID { get { return WarehouseID == Warehouses.Canada ? 32 : 14; } }
            public int NewLaunchCategoryID { get { return 51; } }
            public virtual int WebID { get { return 1; } }
        }

        public class CustomerOrderConfiguration : IOrderConfiguration
        {
            public virtual int WarehouseID { get {
                    return GetCountryCodeByIP()==CountryCodes.Canada?Warehouses.Canada:Warehouses.DefaultUSA;
                } }
            public virtual string CurrencyCode { get { return CurrencyCodes.DollarsUS; } }
            public virtual int PriceTypeID { get { return PriceTypes.Retail; } }
            public virtual int LanguageID { get { return Languages.English; } }
            public virtual string DefaultCountryCode { get { return "US"; } }
            public virtual int DefaultShipMethodID { get { return WarehouseID==Warehouses.Canada?14:2; } }
            public virtual int StarterKitSubscriptionWarehouseID { get { return Warehouses.StarterKitsandSubscriptionsUSA; } }
            public virtual int CategoryID { get { return WarehouseID == Warehouses.Canada ? 32 : 7; } }
            public int NewLaunchCategoryID { get { return 51; } }
            public virtual int WebID { get { return 1; } }
        }

        public class AutoOrderConfiguration : OrderConfiguration, IAutoOrderConfiguration
        {
            private FrequencyType _defaultFrequencyType;
            public override int PriceTypeID { get { return PriceTypes.Wholesale; } }
            //public int NewLaunchCategoryID { get { return 51; } }
            public virtual FrequencyType DefaultFrequencyType { get { return _defaultFrequencyType; } set { _defaultFrequencyType = value; } }
        }
        public static  string GetCountryCodeByIP()
        {
            string userIP = /*HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];  */"71.19.249.53";
            string localeAPIURL = GlobalSettings.IpStack.IpStackUrl + userIP+ "?access_key="+ GlobalSettings.IpStack.IpStackKey+ "&fields=country_code";
            dynamic JsonResponseData = null;
            var country = "US";
            HttpWebRequest r = (HttpWebRequest)WebRequest.Create(localeAPIURL);
            r.Method = "Get";
            try
            {
                WebResponse response = r.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    var responseData = reader.ReadToEnd();
                    JsonResponseData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);
                    country = JsonResponseData["country_code"].Value;
                }
                country = string.IsNullOrEmpty(country) ? CountryCodes.UnitedStates : country;
            }
            catch (WebException ex)
            {
                country = "US";
            }
            return country;

        }


    }
}