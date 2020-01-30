using Common;
using Common.Api.ExigoOData;
using Common.Exceptions;
using Common.ModelsEx.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        ///this method is not calling any where
        public static ItemCategory GetItemCategory(int itemCategoryID)
        {
            //var context = Exigo.OData();


            // Get the nodes
            var categories = new List<ItemCategory>();
            var rowcount = 50;
            var lastResultCount = rowcount;
            var callsMade = 0;
            IEnumerable<Common.Api.ExigoOData.WebCategory> results = null;

            while (lastResultCount == rowcount)
            {
                // Get the data
                //var results = context.WebCategories
                //    .Where(c => c.WebID == 1)
                //    .OrderBy(c => c.ParentID)
                //    .OrderBy(c => c.SortOrder)
                //    .Skip(callsMade * rowcount)
                //    .Take(rowcount)
                //    .Select(c => c)
                //    .ToList();
                using (var Context = Exigo.Sql())
                {
                    Context.Open();
                    //Getting items list by item code
                    string sqlProcedure = string.Format("Getwebcategory {0}", itemCategoryID);
                    results = Context.Query<Common.Api.ExigoOData.WebCategory>(sqlProcedure).ToList().Skip(callsMade * rowcount).Take(rowcount);
                    categories = results.Cast<ItemCategory>().ToList();
                    Context.Close();
                }

                //results.ForEach(c =>
                //{
                //    categories.Add((ItemCategory)c);
                //});

                callsMade++;
                lastResultCount = results.Count();
            }


            // Recursively populate the children
            var category = categories.Where(c => c.ItemCategoryID == itemCategoryID).FirstOrDefault();
            if (category == null) return null;

            category.Subcategories = GetItemCategorySubcategories(category, categories);

            return category;
        }
        private static IEnumerable<ItemCategory> GetItemCategorySubcategories(ItemCategory parentCategory, IEnumerable<ItemCategory> categories)
        {
            var subCategories = categories.Where(c => c.ParentItemCategoryID == parentCategory.ItemCategoryID).ToList();

            foreach (var subCategory in subCategories)
            {
                subCategory.Subcategories = GetItemCategorySubcategories(subCategory, categories);
            }

            return subCategories;
        }

        public static Item GetItem(string ItemCode)
        {
            var Result = new Item();
            using (var Context = Sql())
            {
                Context.Open();
                //Getting items list by item code
                string sqlProcedure = string.Format("GetItemsByItemCode '{0}'", ItemCode);
                Result = Context.Query<Item>(sqlProcedure).FirstOrDefault();
                Context.Close();
            }

            return Result;
        }
        public static Item GetItem(int itemId)
        {
            var Result = new Item();
            using (var Context = Exigo.Sql())
            {
                Context.Open();
                //Getting items list by item code
                string sqlProcedure = string.Format("GetItemsByItemID {0}", itemId);
                Result = Context.Query<Item>(sqlProcedure).FirstOrDefault();
                Context.Close();
            }

            return Result;
        }

        public static IEnumerable<Item> GetItems(GetItemsRequest request)
        {
            // If we don't have what we need to make this call, stop here.
            if (request.Configuration == null)
                throw new InvalidRequestException("ExigoService.GetItems() requires an OrderConfiguration.");

            if (request.Configuration.CategoryID == 0 && request.CategoryID == null && request.ItemCodes.Length == 0)
                throw new InvalidRequestException("ExigoService.GetItems() requires either a CategoryID or a collection of item codes."); ;

            // Build the request.
            var getItemsrequest = new Common.Api.ExigoWebService.GetItemsRequest
            {
                ItemCodes = request.ItemCodes,
                CurrencyCode = request.Configuration.CurrencyCode,
                PriceType = request.Configuration.PriceTypeID,
                LanguageID = 0,
                WarehouseID = request.Configuration.WarehouseID,
                WebID = 1,
                WebCategoryID = (request.ItemCodes.Length == 0 ?
                    request.Configuration.CategoryID as Nullable<int> :
                    null)
            };

            var api = Exigo.WebService();

            // Execute request and get the response.
            var response = api.GetItems(getItemsrequest);

            foreach (var itemResponse in response.Items)
            {
                yield return (Item)itemResponse;
            }
        }
        public static IEnumerable<Product> GetProducts(GetItemsRequest request)
        {
            // If we don't have what we need to make this call, stop here.
            if (request.Configuration == null)
                throw new InvalidRequestException("ExigoService.GetItems() requires an OrderConfiguration.");

            if (request.Configuration.CategoryID == 0 && request.CategoryID == null && request.ItemCodes.Length == 0)
                throw new InvalidRequestException("ExigoService.GetItems() requires either a CategoryID or a collection of item codes."); ;

            // Build the request.
            //var getItemsrequest = new Common.Api.ExigoWebService.GetItemsRequest
            //{
            //    ItemCodes = request.ItemCodes,
            //    CurrencyCode = request.Configuration.CurrencyCode,
            //    PriceType = request.Configuration.PriceTypeID,
            //    LanguageID = 0,
            //    WarehouseID = request.Configuration.WarehouseID,
            //    WebID = 1,
            //    WebCategoryID = (request.ItemCodes.Length == 0 ?
            //        request.Configuration.CategoryID as Nullable<int> :
            //        null)
            //};

            //var api = Exigo.WebService();
            // Execute request and get the response.
            //var response = api.GetItems(getItemsrequest);
            //proc
            List<Common.Api.ExigoWebService.ItemResponse> responseItems = new List<Common.Api.ExigoWebService.ItemResponse>();
            using (var context = Exigo.Sql())
            {
                int webCategoryID = request.ItemCodes.Length == 0 ? request.Configuration.CategoryID : 0;
                int webID = 1;
                bool returnLongDetail = false;
                string itemCodes = request.ItemCodes.Length == 0 ? "" : request.ItemCodes.FirstOrDefault();
                string sqlProcedure = string.Format("GetPersonalOrderItems {0},'{1}',{2},{3},{4},'{5}',{6}"
                    , request.Configuration.PriceTypeID, request.Configuration.CurrencyCode,
                    request.Configuration.WarehouseID, webCategoryID, webID, itemCodes,
                    returnLongDetail);
                responseItems = context.Query<Common.Api.ExigoWebService.ItemResponse>(sqlProcedure).ToList();
            }
            // Project items into products.
            var products = responseItems.Select(item => new Product(item)).ToList();
            //end proc
            //foreach (var itemResponse in response.Items)
            //{
            //    yield return new Product(itemResponse);
            //}
            return products;
        }

        public static IEnumerable<Item> GetItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IOrderConfiguration configuration)
        {
            // If we don't have what we need to make this call, stop here.
            if (configuration == null)
                throw new InvalidRequestException("ExigoService.GetItems() requires an OrderConfiguration.");

            if (shoppingCartItems.Count() == 0)
                yield break;


            // Create the contexts we will use
            //var context = Exigo.OData();

            var ItemCodestringsResult = String.Join(",", shoppingCartItems.Select(i => i.ItemCode).ToList());
            int[] PriceTypeIDArray = new int[] { PriceTypes.Retail, PriceTypes.Wholesale };
            var PriceTypeIDArrayString = String.Join(", ", PriceTypeIDArray);

            var apiItems = new List<ItemWarehousePrice>();
            using (var Context = Exigo.Sql())
            {
                //Getting items list by item code
                string sqlProcedure = string.Format("ItemWarehousePrice {0},'{1}','{2}','{3}'", configuration.WarehouseID, PriceTypeIDArrayString, ItemCodestringsResult, configuration.CurrencyCode);
                apiItems = Context.Query<ItemWarehousePrice>(sqlProcedure).Distinct().ToList().GroupBy(c => new { c.PriceTypeID, c.ItemID }).Select(i => i.First()).ToList();
            }


            //// Get the data
            //var apiItems = context.ItemWarehousePrices.Expand("Item/GroupMembers")
            //        .Where(c => c.WarehouseID == configuration.WarehouseID)
            //    //.Where(c => c.PriceTypeID == configuration.PriceTypeID) Done to support Both Retail and Whole sale items for BO Rewards.
            //        .Where(c => c.PriceTypeID == PriceTypes.Retail || c.PriceTypeID == PriceTypes.Wholesale) //Done to support Both Retail and Whole sale items for BO Rewards.
            //        .Where(c => c.CurrencyCode == configuration.CurrencyCode)
            //        .Where(shoppingCartItems.Select(c => c.ItemCode).Distinct().ToList().ToOrExpression<ItemWarehousePrice, string>("Item.ItemCode"))
            //        .ToList().GroupBy(c => new { c.PriceTypeID, c.ItemID }).Select(i => i.First()).ToList();


            // Loop through each of our cart items, and populate it with the known data
            var results = new List<Item>();
            foreach (var apiItem in apiItems)
            {
                var cartItems = shoppingCartItems.Where(c => c.ItemCode == apiItem.ItemCode && c.PriceTypeID == apiItem.PriceTypeID).ToList();
                foreach (var cartItem in cartItems)
                {
                    var newItem = (Item)apiItem;
                    newItem.ID = cartItem.ID;
                    newItem.Category = cartItem.Category;
                    newItem.LargeImageUrl = cartItem.LargePicture;
                    newItem.TinyImageUrl = cartItem.TinyPicture;
                    newItem.SmallImageUrl = cartItem.SmallPicture;
                    newItem.ItemCode = cartItem.ItemCode;
                    newItem.ItemDescription = cartItem.Description;
                    newItem.Quantity = cartItem.Quantity;
                    newItem.Price = cartItem.Price;
                    newItem.AppliedAmount = cartItem.AppliedAmount;
                    newItem.ParentItemCode = cartItem.ParentItemCode;
                    newItem.GroupMasterItemCode = cartItem.GroupMasterItemCode;
                    newItem.DynamicKitCategory = cartItem.DynamicKitCategory;
                    newItem.Type = cartItem.Type;
                    newItem.PriceTypeID = cartItem.PriceTypeID;
                    newItem.ApplyDiscountType = cartItem.ApplyDiscountType;
                    newItem.PriceEachOverride = cartItem.PriceEachOverride;

                    newItem.MaxKitQuantity = cartItem.MaxKitQuantity;

                    newItem.BusinessVolumeEachOverride = cartItem.BusinessVolumeEachOverride;
                    newItem.CommissionableVolumeEachOverride = cartItem.CommissionableVolumeEachOverride;


                    yield return newItem;
                }
            }
        }
        
        /// <summary>
        /// Getting the Items
        /// </summary>
        /// <param name="itemCodes"></param>
        /// <param name="Configuration"></param>
        /// <param name="priceTypeiD"></param>
        /// <returns></returns>
        public static IEnumerable<Product> GetEligibleItems(string[] itemCodes, IOrderConfiguration Configuration, int priceTypeID)
        {
           
            List<Common.Api.ExigoWebService.ItemResponse> responseItems = new List<Common.Api.ExigoWebService.ItemResponse>();
            using (var context = Exigo.Sql())
            {
                int webCategoryID = itemCodes.Length == 0 ? Configuration.CategoryID : 0;
                int webID = 1;
                bool returnLongDetail = false;
                string Codes = itemCodes.Length == 0 ? "" : String.Join(",", itemCodes);
                string sqlProcedure = string.Format("GetPersonalOrderItems {0},'{1}',{2},{3},{4},'{5}',{6}"
                    , priceTypeID,Configuration.CurrencyCode,
                    Configuration.WarehouseID, webCategoryID, webID, Codes,
                    returnLongDetail);
                responseItems = context.Query<Common.Api.ExigoWebService.ItemResponse>(sqlProcedure).ToList();
            }
            //group by item-code and select only first   allow only those items which  Available in stock
            responseItems = responseItems.GroupBy(x => x.ItemCode).Select(x => x.First()).ToList();
            // allow only those items which  Available in stock
            responseItems = responseItems.Where(x => x.InventoryStatus.ToString().Equals(Common.Api.ExigoWebService.InventoryStatusType.Available.ToString())).ToList();
            if (responseItems.Count() == 0) return new List<Product>();
            var products = responseItems.Select(item => new Product(item)).ToList();
            products.ForEach(c => c.PriceTypeID = priceTypeID);
            return products;

        }
        public static IEnumerable<string> GetEligibleItemCodes(string[] itemCodes, IOrderConfiguration Configuration, int priceTypeiD)
        {
            var getItemsrequest = new Common.Api.ExigoWebService.GetItemsRequest
            {
                ItemCodes = itemCodes,
                CurrencyCode = Configuration.CurrencyCode,
                PriceType = priceTypeiD,
                LanguageID = Configuration.LanguageID,
                WarehouseID = Configuration.WarehouseID,
                WebID = 1,
                WebCategoryID = null
            };

            var api = Exigo.WebService();

            // Execute request and get the response.
            var response = api.GetItems(getItemsrequest);

            foreach (var itemResponse in response.Items)
            {
                yield return itemResponse.ItemCode;
            }

        }
        // v2 methods
        //////Not Caling Method
        //public static IEnumerable<Item> GetItemList(GetItemListRequest request)
        //{
        //    // If we don't have what we need to make this call, stop here.
        //    if (request.Configuration == null)
        //        throw new InvalidRequestException("ExigoService.GetItemList() requires an OrderConfiguration.");

        //    if (request.Configuration.CategoryID == 0 && request.CategoryID == null)
        //        throw new InvalidRequestException("ExigoService.GetItemList() requires either a CategoryID or a collection of item codes."); ;


        //    // Set some defaults
        //    if (request.CategoryID == null)
        //    {
        //        request.CategoryID = request.Configuration.CategoryID;
        //    }


        //    // Create the contexts we will use
        //    var context = Exigo.OData();

        //    // Get the item codes used in this category
        //    var itemCodes = context.WebCategoryItems
        //            .Where(c => c.WebID == 1)
        //            .Where(c => c.WebCategoryID == request.CategoryID)
        //            .Select(c => new { c.Item.ItemCode })
        //            .ToList()
        //            .Select(c => c.ItemCode)
        //            .ToList();



        //    // Get the item details
        //    var query = GetItemsQueryable(request.Configuration, itemCodes);
        //    var apiItems = query.Select(c => new 
        //    {
        //        ItemID                  = c.Item.ItemID,
        //        ItemCode                = c.Item.ItemCode,
        //        ItemDescription         = c.Item.ItemDescription,
        //        IsGroupMaster           = c.Item.IsGroupMaster,
        //        IsVirtual               = c.Item.IsVirtual,
        //        GroupMembersDescription = c.Item.GroupMembersDescription,
        //        IsDynamicKitMaster      = c.Item.IsDynamicKitMaster,
        //        ItemTypeID              = c.Item.ItemTypeID,
        //        SmallImageUrl           = c.Item.SmallImageUrl,
        //        AllowOnAutoOrder        = c.Item.AllowOnAutoOrder,
        //        CurrencyCode            = c.CurrencyCode,
        //        Price                   = c.Price,
        //        BV                      = c.BusinessVolume,
        //        CV                      = c.CommissionableVolume
        //    }).ToList();
        //    var items = (List<Item>)apiItems.ToNonAnonymousList(typeof(Item));


        //    // Populate the group members and dynamic kits
        //    PopulateAdditionalItemData(items);


        //    // Return the converted items
        //    foreach (var item in items)
        //    {
        //        yield return item;
        //    }
        //}
        //public static Item GetItemDetail(GetItemDetailRequest request)
        //{
        //    // If we don't have what we need to make this call, stop here.
        //    if (request.Configuration == null)
        //        throw new InvalidRequestException("ExigoService.GetItemDetail() requires an OrderConfiguration.");

        //    if (request.ItemCode.IsNullOrEmpty())
        //        throw new InvalidRequestException("ExigoService.GetItemDetail() requires an item code."); ;


        //    // Get the item details
        //    var query   = GetItemsQueryable(request.Configuration, new[] { request.ItemCode }, "Item");
        //    var apiItem = query.Select(c => c).FirstOrDefault();
        //    var item    = (ExigoService.Item)apiItem;


        //    // Populate the group members and dynamic kits
        //    PopulateAdditionalItemData(new[] { item });


        //    // Return the converted item
        //    return item;
        //}
        //public static IEnumerable<Item> GetCartItems(GetCartItemsRequest request)
        //{
        //    // If we don't have what we need to make this call, stop here.
        //    if (request.Configuration == null)
        //        throw new InvalidRequestException("ExigoService.GetItemList() requires an OrderConfiguration.");

        //    if (request.ShoppingCartItems == null || request.ShoppingCartItems.Count() == 0)
        //        yield break;


        //    // Get the item codes used in this category
        //    var itemCodes = request.ShoppingCartItems.Select(c => c.ItemCode).ToList();


        //    // Get the item details
        //    var query = GetItemsQueryable(request.Configuration, itemCodes);
        //    var apiItems = query.Select(c => new
        //    {
        //        ItemID                  = c.Item.ItemID,
        //        ItemCode                = c.Item.ItemCode,
        //        ItemDescription         = c.Item.ItemDescription,
        //        IsGroupMaster           = c.Item.IsGroupMaster,
        //        IsVirtual               = c.Item.IsVirtual,
        //        GroupMembersDescription = c.Item.GroupMembersDescription,
        //        IsDynamicKitMaster      = c.Item.IsDynamicKitMaster,
        //        ItemTypeID              = c.Item.ItemTypeID,
        //        TinyImageUrl            = c.Item.TinyImageUrl,
        //        AllowOnAutoOrder        = c.Item.AllowOnAutoOrder,
        //        CurrencyCode            = c.CurrencyCode,
        //        Price                   = c.Price,
        //        BV                      = c.BusinessVolume,
        //        CV                      = c.CommissionableVolume
        //    }).ToList();
        //    var items = (List<Item>)apiItems.ToNonAnonymousList(typeof(Item));


        //    // Populate the shopping cart item detail into the items
        //    foreach (var cartItem in request.ShoppingCartItems)
        //    {
        //        var item = items.Where(c => c.ItemCode == cartItem.ItemCode).FirstOrDefault();
        //        if (item == null) continue;

        //        item.ID                  = cartItem.ID;
        //        item.Quantity            = cartItem.Quantity;
        //        item.ParentItemCode      = cartItem.ParentItemCode;
        //        item.GroupMasterItemCode = cartItem.GroupMasterItemCode;
        //        item.DynamicKitCategory  = cartItem.DynamicKitCategory;
        //        item.Type                = cartItem.Type;
        //    }


        //    // Populate the group members and dynamic kits
        //    PopulateAdditionalItemData(items);


        //    // Return the converted items
        //    foreach (var item in items)
        //    {
        //        yield return item;
        //    }
        //}
        ///Method Not Called
        //private static IQueryable<ItemWarehousePrice> GetItemsQueryable(IOrderConfiguration configuration, IEnumerable<string> itemCodes , params string[] expansions)
        //{
        //    var query = Exigo.OData().ItemWarehousePrices;
        //    if (expansions != null && expansions.Length > 0)
        //    {
        //        query = query.Expand(string.Join(",", expansions));
        //    }

        //    return query
        //            .Where(c => c.WarehouseID == configuration.WarehouseID)
        //            .Where(c => c.PriceTypeID == configuration.PriceTypeID)
        //            .Where(c => c.CurrencyCode == configuration.CurrencyCode)
        //            .Where(itemCodes.ToList().ToOrExpression<ItemWarehousePrice, string>("Item.ItemCode"))
        //            .AsQueryable();
        //}

        //private static void PopulateAdditionalItemData(IEnumerable<Item> items)
        //{
        //    GlobalUtilities.RunAsyncTasks(
        //        () => { PopulateGroupMembers(items); },
        //        () => { PopulateDynamicKitMembers(items); }
        //    );
        //}
        ////Method Not Called
        //private static void PopulateGroupMembers(IEnumerable<Item> items)
        //{
        //    // Determine if we have any group master items
        //    var groupMasterItemCodes = items.Where(c => c.IsGroupMaster).Select(c => c.ItemCode).ToList();
        //    if (groupMasterItemCodes.Count == 0) return;

        //    // Get the item group members
        //    var context = Exigo.OData();
        //    var apiItemGroupMembers = context.ItemGroupMembers
        //        .Where(groupMasterItemCodes.ToOrExpression<Common.Api.ExigoOData.ItemGroupMember, string>("MasterItemCode"))
        //        .ToList();

        //    // Bind the item group members to the items
        //    var itemGroupMembers = apiItemGroupMembers.Select(c => (ItemGroupMember)c).ToList();
        //    foreach (var groupMasterItemCode in groupMasterItemCodes)
        //    {
        //        var item = items.Where(c => c.ItemCode == groupMasterItemCode).FirstOrDefault();
        //        if (item == null) continue;

        //        item.GroupMembers = itemGroupMembers
        //            .Where(c => c.MasterItemCode == groupMasterItemCode)
        //            .OrderBy(c => c.SortOrder)
        //            .ToList();

        //        // Populate the item's basic details for cart purposes
        //        foreach (var groupMember in item.GroupMembers)
        //        {
        //            groupMember.Item = groupMember.Item ?? new Item();
        //            groupMember.Item.ItemCode = groupMember.ItemCode;
        //            groupMember.Item.GroupMasterItemCode = groupMasterItemCode;
        //        }
        //    }
        //}
        //private static void PopulateDynamicKitMembers(IEnumerable<Item> items)
        //{
        //    // Determine if we have any dynamic kit items
        //    var dynamicKitMasterItemCodes = items.Where(c => c.IsDynamicKitMaster).Select(c => c.ItemCode).ToList();
        //    if (dynamicKitMasterItemCodes.Count == 0) return;

        //    // Get the dynamic kit data
        //    var context = Exigo.OData();
        //    var apiItemDynamicKitCagtegoryMembers = context.ItemDynamicKitCategoryMembers.Expand("MasterItem,DynamicKitCategory/DynamicKitCategoryItemMembers/DynamicKitCategory,DynamicKitCategory/DynamicKitCategoryItemMembers/Item")
        //        .Where(dynamicKitMasterItemCodes.ToOrExpression<Common.Api.ExigoOData.ItemDynamicKitCategoryMember, string>("MasterItem.ItemCode"))
        //        .ToList();

        //    // Bind the item group members to the items
        //    foreach (var dynamicKitMasterItemCode in dynamicKitMasterItemCodes)
        //    {
        //        var item = items.Where(c => c.ItemCode == dynamicKitMasterItemCode).FirstOrDefault();
        //        if (item == null) continue;

        //        var apiCategories = apiItemDynamicKitCagtegoryMembers.Where(c => c.MasterItem.ItemCode == dynamicKitMasterItemCode).ToList();
        //        item.DynamicKitCategories = apiCategories.Select(c => (DynamicKitCategory)c).ToList();

        //        foreach (var category in item.DynamicKitCategories)
        //        {
        //            foreach (var categoryItem in category.Items)
        //            {
        //                categoryItem.ParentItemCode = dynamicKitMasterItemCode;
        //            }
        //        }
        //    }
        //}
    }
}