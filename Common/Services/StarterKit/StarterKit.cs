using Common.Api.ExigoWebService;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Common.Services.StarterKit
{
    public class StarterKit : IStarterKit
    {
        //[Inject]
        public ExigoApi Api { get; set; }

        /// <summary>
        /// Web category for starter kits.
        /// </summary>
        private const int CategoryStarterKit = 5;
        /// <summary>
        /// Web category for starter kits items.
        /// </summary>
        private const int CategoryId = 116;
        /// <summary>
        /// The starter kit warehouse.
        /// </summary>
        private const int WarehouseStarterKits = 2;
        /// <summary>
        /// The starter kit items warehouse  .
        /// </summary>
        private const int CategoryWarehouseId = 1;

        public StarterKit(ExigoApi api)
        {
            Api = api;
        }

        public IEnumerable<StarterKitItems> GetStarterKits()
        {         
        return Getitems(CategoryStarterKit,WarehouseStarterKits);
        }
        public IEnumerable<StarterKitItems> GetStarterKitItems()
        {
            return Getitems(CategoryId, CategoryWarehouseId);
        }
        


        /// <summary>
        /// Generic method for querying products from the Exigo API,
        /// filtering by <paramref name="categoryId"/> and
        /// <paramref name="warehouseId"/>.
        /// </summary>      
        /// <returns>A collection of products.</returns>
        private IEnumerable<StarterKitItems> Getitems(int categoryId,int WarehouseId)
        {
            // Build the request.
            var request = new Api.ExigoWebService.GetItemsRequest
            {
                CurrencyCode = "USD", // TODO: Look up in config
                PriceType = 1,
                LanguageID = 0,
                WebCategoryID = categoryId,
                WarehouseID = WarehouseId,
                WebID = 1,
                ReturnLongDetail = true,
                ItemCodes = null
            };

            // Execute request and get the response.
            var response = Api.GetItems(request);
            var products = response.Items.GroupBy(s => s.ItemCode).Select(x => new StarterKitItems()
               {
                   Price = x.FirstOrDefault().Price,
                   SmallPicture = x.FirstOrDefault().SmallPicture,
                   ItemCode = x.FirstOrDefault().ItemCode,
                   ShortDetail = x.FirstOrDefault().ShortDetail,
                   //RetailPrice = Convert.ToDecimal(x.ShortDetail4)

               }).ToList();
            return products;
        }
        /// <summary>
        /// fetching all saved starter kits
        /// </summary>
        /// <param name="StarterKitCategoryID"></param>
        /// <returns></returns>
        public StarterKitItems GetSavedStarterKits(string StarterKitCategoryID)
        {
            try
            {
                using (var contextsql = Exigo.Sql())
                {
                    var sql = string.Format(@"Exec GetSavedStarterKitItems '{0}'", StarterKitCategoryID);
                    var list = contextsql.Query<StarterKitItems>(sql).FirstOrDefault();
                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool SaveStarterKitItems(string StarterKitCategoryID, string strItemCode)
        {

            try
            {
                using (var contextsql = Exigo.Sql())
                {
                    var sql = string.Format(@"Exec InsertKitItems '{0}' , '{1}'", strItemCode, StarterKitCategoryID);
                    var list = contextsql.Query<StarterKitItems>(sql).ToList();
                    return list.Count > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}