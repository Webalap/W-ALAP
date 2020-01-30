namespace ExigoService
{
    public interface IOrderConfiguration
    {
        int WarehouseID { get; }
        string CurrencyCode { get; }
        int PriceTypeID { get; }
        int LanguageID { get; }
        string DefaultCountryCode { get; }
        int DefaultShipMethodID { get; }
        int StarterKitSubscriptionWarehouseID { get; }
        int CategoryID { get; }
        int NewLaunchCategoryID { get; }
    }
}