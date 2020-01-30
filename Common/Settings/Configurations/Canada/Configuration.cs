using Common.Api.ExigoWebService;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public class CanadaConfiguration : IMarketConfiguration
    {
        private MarketName marketName = MarketName.Canada;

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

        public class OrderConfiguration : IOrderConfiguration
        {
            public int WarehouseID { get { return Warehouses.Canada; } }
            public string CurrencyCode { get { return CurrencyCodes.DollarsUS; } }
            public int PriceTypeID { get { return PriceTypes.Wholesale; } }
            public int LanguageID { get { return Languages.English; } }
            public string DefaultCountryCode { get { return "CA"; } }
            public int DefaultShipMethodID { get { return 14; } }
            public virtual int StarterKitSubscriptionWarehouseID { get { return Warehouses.StarterKitsandSubscriptionsUSA; } }
            public int CategoryID { get { return 32; } }
            public int NewLaunchCategoryID { get { return 51; } }
            public int WebID { get { return 1; } }
        }
        public class CustomerOrderConfiguration : IOrderConfiguration
        {
            public int WarehouseID { get { return Warehouses.Canada; } }
            public string CurrencyCode { get { return CurrencyCodes.DollarsUS; } }
            public int PriceTypeID { get { return PriceTypes.Retail; } }
            public int LanguageID { get { return Languages.English; } }
            public string DefaultCountryCode { get { return "CA"; } }
            public int DefaultShipMethodID { get { return 14; } }
            public virtual int StarterKitSubscriptionWarehouseID { get { return Warehouses.StarterKitsandSubscriptionsUSA; } }
            public int CategoryID { get { return 32; } }
            public int NewLaunchCategoryID { get { return 51; } }

            public int WebID { get { return 1; } }
        }
        public class AutoOrderConfiguration : OrderConfiguration, IAutoOrderConfiguration
        {
            public FrequencyType DefaultFrequencyType { get { return FrequencyType.Monthly; } }
        }
    }
}