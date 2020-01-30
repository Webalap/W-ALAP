using Common.ModelsEx.Shopping.Discounts;
using System;
using System.Collections.Generic;
namespace ExigoService
{
    public class DynamicKitCategoryItem : IDynamicKitCategoryItem
    {
        public int ItemID { get; set; }
        public string ItemDescription { get; set; } 
        public string TinyImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
        public int PriceTypeID { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceEachOverride { get; set; }
        public decimal? BusinessVolumeEachOverride { get; set; }
        public decimal? CommissionableVolumeEachOverride { get; set; }

        // IShoppingCartItem Properties
        public Guid ID { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public int MaxKitQuantity { get; set; }
        public string ParentItemCode { get; set; }
        public string GroupMasterItemCode { get; set; }
        public string DynamicKitCategory { get; set; }
        public ShoppingCartItemType Type { get; set; }
        public List<Discount> Discounts { get; set; }
        public DiscountType ApplyDiscountType { get; set; }
        public decimal AppliedAmount { get; set; }
        public int ItemEventId { get; set; }
        public string InventoryStatus { get; set; }
        public bool OtherCheck1 { get; set; }
        public bool OtherCheck2 { get; set; }
        public bool OtherCheck3 { get; set; }
        public bool OtherCheck4 { get; set; }
        public bool OtherCheck5 { get; set; }
        public string Category { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
    }
}
