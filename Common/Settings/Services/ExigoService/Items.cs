using Common;
using Common.Api.ExigoOData;
using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static ItemCategory GetItemCategory(int itemCategoryID)
        {
            var context = Exigo.OData();


            // Get the nodes
            var categories      = new List<ItemCategory>();
            var rowcount        = 50;
            var lastResultCount = rowcount;
            var callsMade       = 0;

            while (lastResultCount == rowcount)
            {
                // Get the data
                var results = context.WebCategories
                    .Where(c => c.WebID == 1)
                    .OrderBy(c => c.ParentID)
                    .OrderBy(c => c.SortOrder)
                    .Skip(callsMade * rowcount)
                    .Take(rowcount)
                    .Select(c => c)
                    .ToList();

                results.ForEach(c =>
                {
                    categories.Add((ItemCategory)c);
                });

                callsMade++;
                lastResultCount = results.Count;
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

        public static IEnumerable<Item> GetItems(GetItemsRequest request)
        {
            // If we don't have what we need to make this call, stop here.
            if (request.Configuration == null) 
                throw new InvalidRequestException("ExigoService.GetItems() requires an OrderConfiguration.");

            if (request.Configuration.CategoryID == 0 && request.CategoryID == null && request.ItemCodes.Length == 0)
                throw new InvalidRequestException("ExigoService.GetItems() requires either a CategoryID or a collection of item codes."); ;


            // Set some defaults
            if (request.CategoryID == null && request.ItemCodes.Length == 0)
            {
                request.CategoryID = request.Configuration.CategoryID;
            }

            // Create the contexts we will use
            var context = Exigo.OData();


            // Determine how many categories we need to pull based on the levels. Currently designed to go one level deep.
            var categoryIDs = new List<int>();
            if (request.CategoryID != null)
            {
                categoryIDs.Add((int)request.CategoryID);

                if (request.IncludeChildCategories)
                {
                    // Get the child categories
                    var ids = context.WebCategories
                        .Where(c => c.WebID == 1)
                        .Where(c => c.ParentID == (int)request.CategoryID)
                        .Select(c => new
                        {
                            c.WebCategoryID
                        }).ToList();

                    categoryIDs.AddRange(ids.Select(c => c.WebCategoryID));
                }
            }


            // If we requested a specific category, get the item codes in the category
            if (categoryIDs.Count > 0) 
            {
                var categoryItemCodes = context.WebCategoryItems
                    .Where(c => c.WebID == 1)
                    .Where(categoryIDs.ToOrExpression<WebCategoryItem, int>("WebCategoryID"))
                    .Select(c => new 
                    {
                        c.Item.ItemCode
                    }).ToList();

                var existingItemCodes = request.ItemCodes.ToList();
                existingItemCodes.AddRange(categoryItemCodes.Select(c => c.ItemCode).ToList());
                request.ItemCodes = existingItemCodes.ToArray();
            }


            // If we don't have any items, stop here.
            if(request.ItemCodes.Length == 0) yield break;


            // Get the data
            var query = context.ItemWarehousePrices.Expand("Item/GroupMembers")
                    .Where(c => c.WarehouseID == request.Configuration.WarehouseID)
                    .Where(c => c.PriceTypeID == request.Configuration.PriceTypeID)
                    .Where(c => c.CurrencyCode == request.Configuration.CurrencyCode);

            if (request.ItemCodes != null && request.ItemCodes.Count() > 0)
            {
                query = query.Where(request.ItemCodes.ToList().ToOrExpression<ItemWarehousePrice, string>("Item.ItemCode"));
            }

            var odataItems = query.ToList();


            // Return the data
            foreach (var item in odataItems)
            {
                yield return (ExigoService.Item)item;
            }
        }
        public static IEnumerable<Item> GetItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IOrderConfiguration configuration)
        {
            // If we don't have what we need to make this call, stop here.
            if (configuration == null)
                throw new InvalidRequestException("ExigoService.GetItems() requires an OrderConfiguration.");

            if (shoppingCartItems.Count() == 0)
                yield break;


            // Create the contexts we will use
            var context = Exigo.OData();


            // Get the data
            var apiItems = context.ItemWarehousePrices.Expand("Item/GroupMembers")
                    .Where(c => c.WarehouseID == configuration.WarehouseID)
                    .Where(c => c.PriceTypeID == configuration.PriceTypeID)
                    .Where(c => c.CurrencyCode == configuration.CurrencyCode)
                    .Where(shoppingCartItems.Select(c => c.ItemCode).Distinct().ToList().ToOrExpression<ItemWarehousePrice, string>("Item.ItemCode"))
                    .ToList();


            // Loop through each of our cart items, and populate it with the known data
            var results = new List<Item>();
            foreach (var apiItem in apiItems)
            {
                var cartItems = shoppingCartItems.Where(c => c.ItemCode == apiItem.Item.ItemCode).ToList();
                foreach (var cartItem in cartItems)
                {
                    var newItem                 = (Item)apiItem;
                    newItem.ID                  = cartItem.ID;
                    newItem.Quantity            = cartItem.Quantity;
                    newItem.ParentItemCode      = cartItem.ParentItemCode;
                    newItem.GroupMasterItemCode = cartItem.GroupMasterItemCode;
                    newItem.DynamicKitCategory  = cartItem.DynamicKitCategory;
                    newItem.Type                = cartItem.Type;

                    yield return newItem;
                }
            }
        }
    }
}