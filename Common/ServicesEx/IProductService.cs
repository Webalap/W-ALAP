using Common.Api.ExigoOData.Rewards;
using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ServicesEx.Rewards;
using ExigoService;
using System;
using System.Collections.Generic;


namespace Common.ServicesEx
{
    /// <summary>
    /// The interface for working with products.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Returns products by category.
        /// </summary>
        /// <param name="orderConfiguration">The order configuration.</param>
        /// <returns>A collection of products.</returns>
        IEnumerable<Product> GetStarterKits(
            IOrderConfiguration orderConfiguration,  string[] itemCodes = null
        );

        /// <summary>
        /// Returns true if <paramref name="product"/> is a starter kit; false otherwise;
        /// </summary>
        /// <param name="product">The producct.</param>
        /// <returns>A boolean.</returns>
        bool IsStarterKit(Product product);

        IEnumerable<Product> GetProducts(IOrderConfiguration configuration, string[] itemCodes, bool includeDiscounts = false, IList<IIndividualReward> activeRewards=null);
        IEnumerable<Product> GetDynamicProducts(IOrderConfiguration configuration, string[] itemCodes, bool includeDiscounts = false, IList<IIndividualReward> activeRewards = null);
        /// <summary>
        /// Returns the collection categories (e.g. Beauty, Health, Accessories).
        /// </summary>
        /// <returns>A list of category IDs.</returns>
        List<WebCategory> GetCollectionCategories();

        /// <summary>
        /// Returns products filtered by <paramref name="categoryId"/>.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="includeDisounts"></param>
        /// <param name="activeRewards"></param>
        /// <param name="event"></param>
        /// <returns>A collection of <see cref="Product"/> objects.</returns>
        IEnumerable<Product> GetProductsByCategory(int categoryId, int priceType = PriceTypes.Retail, bool includeDisounts = false, /*bool includeBookingRewards = false,*/ IList<IIndividualReward> activeRewards = null, Event @event = null, List<int> categoryIds = null, List<string> PurchasedItemCodes = null, string siteType = "",int WarehouseID=Warehouses.DefaultUSA);
        IEnumerable<Product> GetDynamicProductsByCategoryId(int categoryId, string itemCode, int priceType = PriceTypes.Retail, bool includeDisounts = false, /*bool includeBookingRewards = false,*/ IList<IIndividualReward> activeRewards = null, Event @event = null);
       
        
        /// <summary>
        /// Returns the product that matches <paramref name="itemCode"/>.
        /// </summary>
        /// <param name="itemCode">The item code.</param>
        /// <param name="includeDiscounts"></param>
        /// <param name="activeRewards"></param>
        /// <param name="event"></param>
        /// <param name="returnLongDetail"></param>
        /// <returns>A <see cref="Product"/>.</returns>
        Product GetProductByItemCode(string itemCode, bool includeDiscounts = false, IList<IIndividualReward> activeRewards = null, Event @event = null, bool returnLongDetail = true,int? category = null,int WarehouseId = Warehouses.DefaultUSA);

        /// <summary>
        /// Returns products related to <paramref name="product"/>.
        /// </summary>
        /// <param name="product">The source product.</param>
        /// <param name="count">The number of products to find.</param>
        /// <returns>A collection of products.</returns>
        IEnumerable<Product> GetRelatedProducts(Product product, int count = 10);

        IEnumerable<Product> PopulateRetailDiscounts(List<Product> products);

        Product PopulateRetailDiscounts(Product product);


        PromoCode GetPromoCode( String promoCode, DateTime? asOf = null );
        bool UpdateCoupon(int PromoCodeID, int OrderID);

        IReadOnlyCollection<Product> ApplyPromoCodeDiscount( IEnumerable<Product> products, PromoCode promoCode);


        List<WebCategory> GetBackOfficeCategories(int ShoppingCategory);
        List<WebCategory> GetDynamicCategories(int ShoppingCategory);
        
    }

}
