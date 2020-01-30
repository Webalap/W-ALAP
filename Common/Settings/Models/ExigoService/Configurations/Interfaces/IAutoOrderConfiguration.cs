using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface IAutoOrderConfiguration : IOrderConfiguration
    {
        int WarehouseID { get; }
        string CurrencyCode { get; }
        int PriceTypeID { get; }
        int LanguageID { get; }
        string DefaultCountryCode { get; }
        int DefaultShipMethodID { get; }
        int CategoryID { get; }
        FrequencyType DefaultFrequencyType { get; }
    }
}