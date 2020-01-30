using Common.Api.ExigoOData.Rewards;
using Common.Api.ExigoWebService;
using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using Common.Services;
using Common.ServicesEx.Rewards;
using ExigoService;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Common.ServicesEx
{
    /// <summary>
    /// The default implementation of <see cref="IProductService"/>.
    /// </summary>
    public class ProductService : IProductService
    {
        #region Dependencies

        [Inject]
        public ExigoApi Api { get; set; }

        [Inject]
        public IRewardService RewardsSerivce { get; set; }

        //[Inject]
        //public ICaching Caching { get; set; }

        #endregion

        /// <summary>
        /// Returns products by category.
        /// </summary>
        /// <param name="orderConfiguration">The order configuration.</param>
        /// <returns>A collection of products.</returns>
        IEnumerable<Product> IProductService.GetStarterKits(
            IOrderConfiguration orderConfiguration, string[] itemCodes = null)
        {
            return GetStarterKitProductsByCategoryId(CategoryStarterKit, WarehouseStarterKits, itemCodes);
        }

        /// <summary>
        /// Returns true if <paramref name="product"/> is a starter kit; false otherwise;
        /// </summary>
        /// <param name="product">The producct.</param>
        /// <returns>A boolean.</returns>
        bool IProductService.IsStarterKit(Product product)
        {
            return product.CategoryId == CategoryStarterKit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="itemCodes"></param>
        /// <param name="includeDiscounts"></param>
        /// <returns></returns>
        IEnumerable<Product> IProductService.GetProducts(IOrderConfiguration configuration, string[] itemCodes, bool includeDiscounts, IList<IIndividualReward> activeRewards)
        {
            // TODO: BRIAN F. - This should call a private, reusable method to stay DRY. This implementation is too similar.
            var request = new Api.ExigoWebService.GetItemsRequest
            {
                CurrencyCode = configuration.CurrencyCode,
                PriceType = configuration.PriceTypeID,
                LanguageID = configuration.LanguageID,
                WarehouseID = configuration.WarehouseID,
                WebCategoryID = configuration.CategoryID,
                ItemCodes = itemCodes,
                ReturnLongDetail = true
            };

            // Execute request and get the response.
            //var response = Api.GetItems(request);

            // Project items into products.

            //var products = response.Items
            //    .Select(item => new Product(item) { PriceTypeID = configuration.PriceTypeID })
            //    .ToList();
            //procedure
            List<ItemResponse> responseItems = new List<ItemResponse>();
            string itemCode = request.ItemCodes.Length > 0 ? request.ItemCodes[0] : "";
            using (var context = Exigo.Sql())
            {
                int webID = 1;
                //if itemcode didn't supplied then get item based on WebCategoryID
                int WebCategoryID = itemCode == string.Empty ? Convert.ToInt32(request.WebCategoryID) : 0;
                string sqlProcedure = string.Format("GetPersonalOrderItems {0},'{1}',{2},{3},{4},'{5}',{6}"
                    , request.PriceType, request.CurrencyCode, request.WarehouseID, WebCategoryID, webID, itemCode, request.ReturnLongDetail);
                responseItems = context.Query<ItemResponse>(sqlProcedure).ToList();
            }
            // Project items into products.
            List<Product> products = new List<Product>();
            if (!string.IsNullOrEmpty(itemCode))
            {
                var product = responseItems.GroupBy(s => s.ItemCode).FirstOrDefault();
                Product singleProduct = new Product(product.FirstOrDefault());
                products.Add(singleProduct);
            }
            else
            {
                products = responseItems.Select(item => new Product(item)).ToList();
            }
            
            //end Procedure
            products.ForEach(c => c.PriceTypeID = request.PriceType);
            PopulateEligibleDiscounts(products, includeDiscounts);
            if (activeRewards != null)
            {
                foreach (var applyDiscount in activeRewards)
                {
                    applyDiscount.PopulateEligibleDiscounts(products);
                }    
            }
            return products;
        }
        IEnumerable<Product> IProductService.GetDynamicProducts(IOrderConfiguration configuration, string[] itemCodes, bool includeDiscounts, IList<IIndividualReward> activeRewards)
        {
            // TODO: BRIAN F. - This should call a private, reusable method to stay DRY. This implementation is too similar.
            var request = new Api.ExigoWebService.GetItemsRequest
            {
                CurrencyCode = configuration.CurrencyCode,
                PriceType = configuration.PriceTypeID,
                LanguageID = configuration.LanguageID,
                WarehouseID = configuration.WarehouseID,
                WebCategoryID = configuration.CategoryID,
                ItemCodes = itemCodes,
                ReturnLongDetail = true
            };

            // Execute request and get the response.
            //var response = Api.GetItems(request);

            // Project items into products.
            //var products = response.Items
            //    .Select(item => new Product(item) { PriceTypeID = configuration.PriceTypeID })
            //    .ToList();
            //procedure
            List<ItemResponse> responseItems = new List<ItemResponse>();
            using (var context = Exigo.Sql())
            {
                string itemCode = request.ItemCodes.Length > 0 ? request.ItemCodes[0] : "";
                string sqlProcedure = string.Format("GetDynamicItemDetail {0},'{1}',{2},{3},'{4}',{5}"
                    , request.PriceType, request.CurrencyCode, request.WarehouseID, request.WebCategoryID, itemCode, request.ReturnLongDetail);
                responseItems = context.Query<ItemResponse>(sqlProcedure).ToList();
            }
            // Project items into products.
            var products = responseItems.Select(item => new Product(item)).ToList();
            //end Procedure
            products.ForEach(c => c.PriceTypeID = request.PriceType);
            PopulateEligibleDiscounts(products, includeDiscounts);
            if (activeRewards != null)
            {
                foreach (var applyDiscount in activeRewards)
                {
                    applyDiscount.PopulateEligibleDiscounts(products);
                }

            }
            return products;
        }


        /// <summary>
        /// Returns the collection categories (e.g. Beauty, Handbags, Accessories).
        /// </summary>
        /// <returns>A list of category IDs.</returns>
        List<WebCategory> IProductService.GetCollectionCategories()
        {
            var categories = DisplayCategoryService.GetDisplayCategories("rep");

            return categories.Select(category => new WebCategory
            {
                Id = category.ID,
                Name = category.Name,
                ImageUrl = category.ImageURL,
                DisplayOnCorporateSite = category.ShowOnCorporateSite
            }).ToList();
        }

        /// <summary>
        /// Returns the collection categories (e.g. Beauty, Handbags, Accessories).
        /// </summary>
        /// <returns>A list of category IDs.</returns>
        List<WebCategory> IProductService.GetBackOfficeCategories(int ParentCategoryID)
        {     
            //var categories = Exigo.OData().WebCategories.Where(i => i.ParentID == ParentCategoryID).OrderBy(i => i.SortOrder);
           List<Common.Api.ExigoOData.WebCategory> categories = null;
            using (var Context = Exigo.Sql())
            {
                Context.Open();
                string sqlProcedure = string.Format("GetWebCategories");
                categories = Context.Query<Common.Api.ExigoOData.WebCategory>(sqlProcedure).Where(i => i.ParentID == ParentCategoryID).OrderBy(i => i.SortOrder).ToList();
                Context.Close();
            }

            return categories.Select(category => new WebCategory
            {
                Id = category.WebCategoryID,
                Name = category.WebCategoryDescription,
            }).ToList();
        }
        List<WebCategory> IProductService.GetDynamicCategories(int ParentCategoryID)
        {
            //var categories = Exigo.OData().WebCategories.Where(i => i.ParentID == ParentCategoryID).OrderBy(i => i.SortOrder);
            List<Common.Api.ExigoOData.WebCategory> categories = null;
            using (var Context = Exigo.Sql())
            {
                Context.Open();
                string sqlProcedure = string.Format("GetWebCategories");
                categories = Context.Query<Common.Api.ExigoOData.WebCategory>(sqlProcedure).Where(i => i.ParentID == ParentCategoryID).OrderBy(i => i.SortOrder).ToList();
                Context.Close();
            }
            return categories.Select(category => new WebCategory
            {
                Id = category.WebCategoryID,
                Name = category.WebCategoryDescription,
            }).ToList();
        }
        /// <summary>
        /// Returns products filtered by <paramref name="categoryId"/>.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="event"></param>
        /// <param name="activeRewards"></param>
        /// <param name="priceType"></param>
        /// <param name="includeDiscounts"></param>
        /// <returns>A collection of <see cref="Product"/> objects.</returns>
        IEnumerable<Product> IProductService.GetProductsByCategory(int categoryId, int priceType, bool includeDiscounts, /*bool includeBookingRewards,*/ IList<IIndividualReward> activeRewards, Event @event, List<int> categoryIds, List<string> PurchasedItemCodes,string siteType, int WarehouseID = Warehouses.DefaultUSA)
        {    
            return GetProductsByCategoryId(categoryId,WarehouseID, priceType: priceType, includeDiscounts: includeDiscounts, /*includeBookingRewardDiscount: includeBookingRewards,*/ activeRewards: activeRewards, @event: @event, categoryIds: categoryIds, PurchasedItemCodes: PurchasedItemCodes, siteType: siteType);

        }
        IEnumerable<Product> IProductService.GetDynamicProductsByCategoryId(int categoryId, string itemCode, int priceType, bool includeDiscounts, /*bool includeBookingRewards,*/ IList<IIndividualReward> activeRewards, Event @event)
        {
            return GetDynamicProductsByCategoryId(categoryId, itemCode, priceType: priceType, includeDiscounts: includeDiscounts, /*includeBookingRewardDiscount: includeBookingRewards,*/ activeRewards: activeRewards, @event: @event);
        }
        /// <summary>
        /// Returns the product that matches <paramref name="itemCode"/>.
        /// </summary>
        /// <param name="itemCode">The item code.</param>
        /// <param name="includeDiscounts"></param>
        /// <param name="activeRewards"></param>
        /// <param name="event"></param>
        /// <param name="returnLongDetail"></param>
        /// <returns>A <see cref="Product"/>.</returns>
        Product IProductService.GetProductByItemCode(string itemCode, bool includeDiscounts, IList<IIndividualReward> activeRewards, Event @event, bool returnLongDetail,int? categoryID,int WarehouseId=WarehouseDefault)
        {
            // When requesting a particular product by itemCode,
            // the category needs to be null.
            IEnumerable<Product> products = GetProductsByCategoryId(categoryID, itemCode: itemCode,warehouseId: WarehouseId, returnLongDetail: returnLongDetail, includeDiscounts: includeDiscounts, activeRewards: activeRewards, @event: @event);
            
            return categoryID > 0 ? products.FirstOrDefault(s => s.CategoryId == categoryID) : products.FirstOrDefault();
        }

        /// <summary>
        /// Returns products related to <paramref name="product"/>.
        /// </summary>
        /// <param name="product">The source product.</param>
        /// <param name="count">The number of products to find.</param>
        /// <returns>A collection of products.</returns>
        IEnumerable<Product> IProductService.GetRelatedProducts(Product product, int count)
        {
            // As of 12/17/2014, instead of performing logic to determine related products,
            // we are simply returning "India's Favorites" (category 13).
            return GetProductsByCategoryId(CategoryIndiasFavorites).Take(count);
        }

        /// <summary>
        /// Creates and adds eligible discounts per product.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="includeDiscounts"></param>
        /// <param name="includeBookingRewardDiscount"></param>
        /// <param name="event"></param>
        protected virtual void PopulateEligibleDiscounts(
            List<Product> products,
            bool includeDiscounts = false,
            bool includeBookingRewardDiscount = false,
            Event @event = null)
        {
            foreach (var product in products)
            {
                // TODO: Inject DiscountFactory dependency
                DiscountFactory factory = new DiscountFactory();
                if (includeDiscounts)
                {

                    if (!string.IsNullOrWhiteSpace(product.Field4) && product.Field4 != "No")
                    {
                        product.EligibleDiscounts.Add(factory.CreateDiscount(DiscountType.RewardsCash, 0M));
                    }

                    if (!string.IsNullOrWhiteSpace(product.Field5) && product.Field5 != "No" && null != @event)
                    {
                        product.EligibleDiscounts.Add(factory.CreateDiscount(DiscountType.HalfOffCredits, 0.5M));
                    }

                    if (!string.IsNullOrWhiteSpace(product.Field5) && product.Field5 != "No" && null == @event)
                    {
                        product.EligibleDiscounts.Add(factory.CreateDiscount(DiscountType.SAHalfOff, 0.5M));
                    }

                    if (!string.IsNullOrWhiteSpace(product.Field6) && product.Field6 != "No")
                    {
                        product.EligibleDiscounts.Add(factory.CreateDiscount(DiscountType.NewProductsLaunchReward, 0.5M));
                    }

                    //if (null != @event)
                    //{
                    //    var hostSpecialReward = factory.CreateDiscount(DiscountType.HostSpecial, @event, 0M);
                    //    // TODO: Add eligibility for Host Special Reward
                    //    product.EligibleDiscounts.Add(hostSpecialReward);
                    //}
                }
                else
                {

                    if (product.PriceTypeID != PriceTypes.Wholesale)
                    {

                        // new 10% PRV Currently no Start Date and End Date is given
                        // Exclude manually on Replicated Site Controller
                        product.EligibleDiscounts.Add(factory.CreateDiscount(DiscountType.ProductCredit, 0M));

                    }

                }

                //if (includeBookingRewardDiscount)
                //{
                //    product.EligibleDiscounts.Add(new DiscountFactory().CreateDiscount(DiscountType.BookingRewards, 0M));
                //}



                //if (Convert.ToBoolean(HttpContext.Current.Cache["RecruitingRewardEligibility"]) && (product.CategoryId != 0 && product.CategoryId != 34 && product.CategoryId != 40 && product.CategoryId != 43 && product.CategoryId != 44 && product.CategoryId != 45 && product.CategoryId != 46 && product.CategoryId != 51)) //We should not add these as rewards to the SA Half off products.
                //{
                //    product.EligibleDiscounts.Add(new DiscountFactory().CreateDiscount(DiscountType.RecruitingReward, 0M));

                //}

                //if (Convert.ToBoolean(HttpContext.Current.Cache["EnrolleeRewardEligibility"]) && (product.CategoryId != 0 && product.CategoryId != 34 && product.CategoryId != 40 && product.CategoryId != 43 && product.CategoryId != 44 && product.CategoryId != 45 && product.CategoryId != 46 && product.CategoryId != 51)) //We should not add these as rewards to the SA Half off products.
                //{
                //    product.EligibleDiscounts.Add(new DiscountFactory().CreateDiscount(DiscountType.EnrolleeReward, 0M));
                //}
            }
        }

        IEnumerable<Product> IProductService.PopulateRetailDiscounts(List<Product> products)
        {
            var discountedProducts = products as IList<Product> ?? products.ToList();
            var promos = RetailPromoService.GetRetailPromos();
            if (promos == null) return discountedProducts;

            foreach (var promo in promos)
            {
                foreach (var product in discountedProducts)
                {
                    if (promo.ItemCode != product.ItemCode) continue;
                    product.EligibleDiscounts.Add(new DiscountFactory().CreateDiscount(
                        (DiscountType)promo.DiscountType,
                        (decimal)promo.DiscountAmount,
                        promo.BV,
                        promo.CV
                        )
                    );

                    var productDiscount = product.EligibleDiscounts.Find(d => d.DiscountType == (DiscountType)promo.DiscountType);
                    product.ApplyDiscount(productDiscount);
                    product.ApplyDiscountType = productDiscount.DiscountType;
                    product.PriceEachOverride = product.DiscountedPrice;
                    product.AppliedAmount = productDiscount.AppliedAmount;
                }
            }
            return discountedProducts;

        }
        /// <summary>
        /// Add Discount for Eligible Product
        /// </summary>
        /// <param name="promo"></param>
        /// <param name="product"></param>
        private static void AddEligibleDiscounts(Common.Services.RetailPromo promo, Product product)
        {
            product.EligibleDiscounts.Add(new DiscountFactory().CreateDiscount(
           (DiscountType)promo.DiscountType,
           (decimal)promo.DiscountAmount,
           promo.BV,
           promo.CV));

            var productDiscount = product.EligibleDiscounts.Find(d => d.DiscountType == (DiscountType)promo.DiscountType);
            product.ApplyDiscount(productDiscount);
            product.ApplyDiscountType = productDiscount.DiscountType;
            product.PriceEachOverride = product.DiscountedPrice;
            product.AppliedAmount = productDiscount.AppliedAmount;
        }

        Product IProductService.PopulateRetailDiscounts(Product product)
        {
            var promo = RetailPromoService.GetRetailPromo(product);

            if (promo == null) return product;

            //product.EligibleDiscounts.Add(new DiscountFactory().CreateDiscount(
            //                                                        (DiscountType)promo.DiscountType,
            //                                                        (decimal)promo.DiscountAmount,
            //                                                         promo.BV, 
            //                                                         promo.CV));

            //var productDiscount = product.EligibleDiscounts.Find(d => d.DiscountType == (DiscountType)promo.DiscountType);
            //product.ApplyDiscount(productDiscount);
            //product.ApplyDiscountType = productDiscount.DiscountType;
            //product.PriceEachOverride = product.DiscountedPrice;
            //product.AppliedAmount = productDiscount.AppliedAmount;
            AddEligibleDiscounts(promo, product);

            return product;
        }




        PromoCode IProductService.GetPromoCode(string promoCodeText, DateTime? asOf)
        {

            asOf = asOf ?? DateTime.Now; 
               using (var context = Exigo.Sql())
                {
                    string sql = $@"CheckPromoCode '{promoCodeText}'";
                    return context.Query<PromoCode>(sql).FirstOrDefault();
                }  
        }
        bool IProductService.UpdateCoupon(int PromoCodeID,int OrderID)
        {  
            try
            {
                // Execute request and get the response.
                using (var context = Exigo.Sql())
                {
                    string sqlProcedure = $"Update_Coupon {PromoCodeID},{OrderID}";
                    var response = context.Query<int>(sqlProcedure).ToList();                          
                } 
            }
            catch (Exception)
            {                
            }
            return true;
        }




        IReadOnlyCollection<Product> IProductService.ApplyPromoCodeDiscount(IEnumerable<Product> products, PromoCode promoCode)
        {

            List<Product> productList = products.ToList();  

            if (promoCode == null)
            { 
                return productList;

            }
            //Find the products where the promo code can be applied.
            var promoCodeEligibleProducts = PromoCodeEligibleProducts(productList, promoCode);

            // divide the amount between all procucts equally 
            decimal discountAmount = promoCode.DiscountAmount;
            if (promoCode.DiscountType == (int)DiscountType.PromoCodeFixed)
            {
                decimal sum = promoCodeEligibleProducts.Sum(p => p.Price * p.Quantity);
                discountAmount = ((discountAmount / sum) * 100);
            }


            if (discountAmount > 100) discountAmount = 100;

            foreach (Product product in promoCodeEligibleProducts)
            {


                // subract the %age amount from BusinessVolume and CommissionableVolume 
                decimal? BV = (decimal)product.BusinessVolumeEachOverride == 0 ? 0M : product.BusinessVolumeEachOverride;
                decimal? CV = (decimal)product.CommissionableVolumeEachOverride == 0 ? 0M : product.CommissionableVolumeEachOverride;
                decimal? BVPercent = (decimal)BV - (((discountAmount) * BV) / 100);
                decimal? CVPercent = (decimal)CV - (((discountAmount) * CV) / 100);

                Discount discount = new DiscountFactory().CreateDiscount(DiscountType.PromoCode,
                                                                               discountAmount,
                                                                               promoCode.Code,
                                                                               BVPercent,
                                                                               CVPercent
                 );

                product.EligibleDiscounts.Add(discount);

                product.ApplyDiscount(discount);

                product.ApplyDiscountType = discount.DiscountType;

            }

            if (promoCode.ApplyToItemCodes == "All")
            {
                return promoCodeEligibleProducts.AsReadOnly();
            }

            foreach (var product in promoCodeEligibleProducts)
            {
                var i = productList.FindIndex(c => c.ItemCode == product.ItemCode);
                productList[i] = product;
            }
            return productList.AsReadOnly();

        }

        private List<Product> PromoCodeEligibleProducts(List<Product> productList, PromoCode promoCode)
        {
            List<Product> trimmedProductList = new List<Product>();
            //Find out if the code applies to All or some specific codes
            if (promoCode.ApplyToItemCodes == "All")
                return productList;

            var itemCodes = promoCode.ApplyToItemCodes.Split(',');
            foreach (var itemCode in itemCodes)
            {
                trimmedProductList.AddRange(productList.FindAll(c => c.ItemCode == itemCode));
            }

            return trimmedProductList;
        }





        /// <summary>
        /// Generic method for querying products from the Exigo API,
        /// filtering by <paramref name="categoryId"/> and
        /// <paramref name="warehouseId"/>.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="itemCode"></param>
        /// <param name="priceType"></param>
        /// <param name="returnLongDetail"></param>
        /// <param name="includeDiscounts"></param>
        /// <param name="includeBookingRewardDiscount"></param>
        /// <param name="activeRewards"></param>
        /// <param name="event"></param>
        /// <returns>A collection of products.</returns>
        private IEnumerable<Product> GetProductsByCategoryId(
            int? categoryId = CategoryDefault,
            int warehouseId = WarehouseDefault,
            string itemCode = null,
            int priceType = 1,
            bool returnLongDetail = true,
            bool includeDiscounts = false,
            bool includeBookingRewardDiscount = false,
            IList<IIndividualReward> activeRewards = null,
            Event @event = null,
            List<int> categoryIds=null,
            List<string> PurchasedItemCodes = null,
            string siteType = "" 
            )
        {
            //// Execute request and get the response.
            //var response = Api.GetItems(request);

            ////Project items into products.
            //var products2 = response.Items
            //    .Select(item => new Product(item))
            //    .ToList();
            List<ItemResponse> responseItems = new List<ItemResponse>();
            if (categoryIds != null && categoryIds.Count > 0)
            {
                foreach (int category in categoryIds)
                {
                    var response = GetItemsApiCall(category, warehouseId, itemCode, priceType, returnLongDetail);
                    responseItems.AddRange(response.ToList());
                }
            }
            else
            {
                var response = GetItemsApiCall(categoryId, warehouseId, itemCode, priceType, returnLongDetail);
                responseItems.AddRange(response.ToList());
            }
         
            //remove PurchasedItems from list
            if (PurchasedItemCodes != null && PurchasedItemCodes.Count > 0) responseItems.RemoveAll(i => PurchasedItemCodes.Contains(i.ItemCode));

            // Project items into products.
            List<Product> products = responseItems
                                .Select(item => new Product(item))
                                .ToList();
            // Project items into products.
            //end proc
            products.ForEach(c => c.PriceTypeID = priceType);
            products.ForEach(c => c.Quantity = 1);
            PopulateEligibleDiscounts(products, includeDiscounts, includeBookingRewardDiscount, @event);
       
           if (activeRewards != null)
           {
               foreach (var applyDiscount in activeRewards)
               {
                   if (priceType != PriceTypes.Wholesale) applyDiscount.PopulateEligibleDiscounts(products);
               }

           }
           //set flag for fist item in group to display in all menu 
           var FirstItemList = products
             .GroupBy(record => new { record.ItemCode })
             .Select(g => g.OrderBy(record => record.ItemCode).ThenBy(record => record.CategoryId).First()).ToList();
           products.Where(s => FirstItemList.Contains(s)).ToList().ForEach(d => d.FirstItemInGroup = "DisplayAll");
           return products;

        }
        private List<ItemResponse> GetItemsApiCall(int? categoryId, int warehouseId, string itemCode, int priceType, bool returnLongDetail)
        {
           
            
            // Execute request and get the response.
            using (var context = Exigo.Sql())
            {
                string productItemCode = itemCode;
                int? webCategoryID = categoryId != null ? categoryId : 0;
                string sqlProcedure = string.Format("GetPersonalOrderItems {0},'{1}',{2},{3},{4},'{5}',{6}"
                    , priceType, "USD", warehouseId, webCategoryID, 1, productItemCode, returnLongDetail);
                var response =  context.Query<ItemResponse>(sqlProcedure).ToList();
                return response;
            }  
        }
        /// <summary>
        /// Generic method for querying products of starter kit,
        /// <paramref name="categoryId"/>
        /// <paramref name="warehouseId"/>.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="itemCode"></param>
        /// <param name="priceType"></param>
        /// <param name="returnLongDetail"></param>
        /// <param name="includeDiscounts"></param>
        /// <param name="includeBookingRewardDiscount"></param>
        /// <param name="activeRewards"></param>
        /// <param name="event"></param>
        /// <returns>A collection of products.</returns>
        private IEnumerable<Product> GetStarterKitProductsByCategoryId(
            int? categoryId = CategoryDefault,
            int warehouseId = WarehouseDefault,
            string[] itemCodes = null,
            int priceType = 1,
            bool returnLongDetail = true,
            bool includeDiscounts = false,
            bool includeBookingRewardDiscount = false,
            IList<IIndividualReward> activeRewards = null,
            Event @event = null,
            string siteType = ""

            )
        {
            // Build the request.
            var request = new Api.ExigoWebService.GetItemsRequest
            {
                CurrencyCode = "USD", // TODO: Look up in config
                PriceType = priceType,
                LanguageID = 0,
                WarehouseID = warehouseId,
                WebID = 1,
                ReturnLongDetail = returnLongDetail,
                WebCategoryID = categoryId,
                ItemCodes = itemCodes != null ? itemCodes : null
                //ItemCodes = !string.IsNullOrWhiteSpace(itemCode)
                //    ? new[] { itemCode }
                //    : null
            };

            // Execute request and get the response.
            //var response = Api.GetItems(request);

            ////Project items into products.
            //var products2 = response.Items
            //    .Select(item => new Product(item))
            //    .ToList();
            //proc
            List<ItemResponse> responseItems = new List<ItemResponse>();
            using (var context = Exigo.Sql())
            {
                string productItemCode = request.ItemCodes != null ? request.ItemCodes.Length > 0 ? request.ItemCodes[0] : "" : "";
                int? webCategoryID = request.WebCategoryID != null ? request.WebCategoryID : 0;
                string sqlProcedure = string.Format("GetStarterKitItems {0},'{1}',{2},{3},{4},'{5}',{6}"
                    , request.PriceType, request.CurrencyCode, request.WarehouseID, webCategoryID, request.WebID, productItemCode, request.ReturnLongDetail);
                responseItems = context.Query<ItemResponse>(sqlProcedure).ToList();
            }
            // Project items into products.
            var products = responseItems.Select(item => new Product(item)).ToList();
            //end proc
            products.ForEach(c => c.PriceTypeID = priceType);
            PopulateEligibleDiscounts(products, includeDiscounts, includeBookingRewardDiscount, @event);

            if (activeRewards != null)
            {
                foreach (var applyDiscount in activeRewards)
                {
                    if (priceType == PriceTypes.Wholesale) ;
                    else applyDiscount.PopulateEligibleDiscounts(products);
                }

            }
            return products;
        }

        private IEnumerable<Product> GetDynamicProductsByCategoryId(  
                  int? categoryId = CategoryDefault,
                  string itemCode = "",
                  int warehouseId = WarehouseDefault,
                  int priceType = 1,
                  bool returnLongDetail = true,
                  bool includeDiscounts = false,
                  bool includeBookingRewardDiscount = false,
                  IList<IIndividualReward> activeRewards = null,
                  Event @event = null
                  )

        {
            // Build the request.
            var request = new Api.ExigoWebService.GetItemsRequest
            {
                CurrencyCode = "USD", // TODO: Look up in config
                PriceType = priceType,
                LanguageID = 0,
                WarehouseID = warehouseId,
                WebID = 1,
                ReturnLongDetail = returnLongDetail,
                WebCategoryID = null,
                ItemCodes = !string.IsNullOrWhiteSpace(itemCode)
                    ? new[] { itemCode }
                    : null
            };

            // Execute request and get the response.
            //var response = Api.GetItems(request);
            ////Project items into products.
            //var products2 = response.Items
            //    .Select(item => new Product(item))
            //    .ToList();
            ////proc
            //List<ItemResponse> responseItems = new List<ItemResponse>();
            List<ItemResponse> responseItems = new List<ItemResponse>();
            List<Product> products = new List<Product>();
            using (var context = Exigo.Sql())
            {
                string itemCodes = string.Join(",", request.ItemCodes);
                string sqlProcedure = string.Format("GetPersonalOrderItems {0},'{1}',{2},{3},{4},'{5}',{6}"
                    , request.PriceType, request.CurrencyCode, request.WarehouseID, request.WebCategoryID != null ? request.WebCategoryID : 0, request.WebID, itemCodes, request.ReturnLongDetail);
                //responseItems = context.Query<ItemResponse>(sqlProcedure).ToList();
                responseItems = context.Query<ItemResponse>(sqlProcedure).ToList();
                //in case of  Dynamic Kit items it return Max Kit Count in Quantity field
                //products.ForEach(s => s.Quantity = 1);

                // Project items into products.
                products = responseItems
                                    .Select(item => new Product(item))
                                    .ToList();

                products.ForEach(s => s.LargePicture = GlobalUtilities.GetProductImagePath(s.LargePicture, s.ItemCode));
                products.ForEach(s => s.SmallPicture = GlobalUtilities.GetProductImagePath(s.SmallPicture, s.ItemCode));
                products.ForEach(s => s.TinyPicture = GlobalUtilities.GetProductImagePath(s.TinyPicture, s.ItemCode));
            }
            //KitMemberResponse
            // Project items into products.
            //var products = responseItems.Select(item => new Product(item)).ToList();
            KitMemberResponse kitMemberResponse;
            foreach (var productItem in products)
            {
                kitMemberResponse = new KitMemberResponse();


                using (var context = Exigo.Sql())
                {
                    //Getting Decription of parent
                    string sqlProcedure1 = string.Format("GetParentDynamicKitDescription '{0}'", productItem.ItemCode);
                    ParentDynamicKit parentDynamicKitDesc = context.Query<ParentDynamicKit>(sqlProcedure1).FirstOrDefault();
                    kitMemberResponse.Description = parentDynamicKitDesc.Description;
                    // getting members of kit
                    string sqlProcedure = string.Format("GetKitMemberItems '{0}'", productItem.ItemCode);
                    var lstKitMemberItems = context.Query<KitMemberItemResponse>(sqlProcedure).ToArray();
                    kitMemberResponse.KitMemberItems = lstKitMemberItems;
                }
                List<KitMemberResponse> lstKitMemberRespones = new List<KitMemberResponse>();
                lstKitMemberRespones.Add(kitMemberResponse);
                productItem.KitMembers = lstKitMemberRespones.ToArray();
            }

            ////end proc
            products.ForEach(c => c.PriceTypeID = priceType);
            PopulateEligibleDiscounts(products, includeDiscounts, includeBookingRewardDiscount, @event);

            if (activeRewards != null)
            {
                foreach (var applyDiscount in activeRewards)
                {
                    if (priceType != PriceTypes.Wholesale) applyDiscount.PopulateEligibleDiscounts(products);
                }

            }
            return products;
        }
        /// <summary>
        /// Getting the Dynamic Kit Description
        /// </summary>
        public class ParentDynamicKit
        {
            public string Description { get; set; }
        }
        /// <summary>
        /// The default warehouse.
        /// </summary>
        private const int WarehouseDefault = 1;

        /// <summary>
        /// The starter kit warehouse.
        /// </summary>
        private const int WarehouseStarterKits = 2;

        /// <summary>
        /// Web category for recommended addons.
        /// </summary>
        private const int CategoryRecommendedAddons = 4;

        /// <summary>
        /// Web category for starter kits.
        /// </summary>
        private const int CategoryStarterKit = 5;

        /// <summary>
        /// The default web category.
        /// </summary>
        private const int CategoryDefault = 7;

        /// <summary>
        /// &quot;India's Favorites&quot;
        /// </summary>
        private const int CategoryIndiasFavorites = 13;
    }
}