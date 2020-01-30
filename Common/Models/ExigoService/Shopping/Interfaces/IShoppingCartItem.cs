using Common.ModelsEx.Shopping.Discounts;
using System;
using System.Collections.Generic;
namespace ExigoService
{
    public interface IShoppingCartItem
    {
        Guid ID { get; set; }
        string ItemCode { get; set; }
        string Category { get; set; }
        decimal Quantity { get; set; }
        int MaxKitQuantity { get; set; }
        string ParentItemCode { get; set; }
        string ItemDescription { get; set; }
        string GroupMasterItemCode { get; set; }
        string DynamicKitCategory { get; set; }
        ShoppingCartItemType Type { get; set; }
        int PriceTypeID { get; set; }
        decimal Price { get; set; }
        decimal? PriceEachOverride { get; set; }
        decimal? BusinessVolumeEachOverride { get; set; }
        decimal? CommissionableVolumeEachOverride { get; set; }
        List<Discount> Discounts { get; set; }
        DiscountType ApplyDiscountType { get; set; }
        decimal AppliedAmount { get; set; }
        int ItemEventId { get; set; }
        string InventoryStatus { get; set; }
        bool OtherCheck1 { get; set; }
        bool OtherCheck2 { get; set; }
        bool OtherCheck3 { get; set; }
        bool OtherCheck4 { get; set; }
        bool OtherCheck5 { get; set; }
        // for landed Cost Api
        //country
         string Field1 { get; set; }
        //HS code
         string Field2 { get; set; }
    }
}
