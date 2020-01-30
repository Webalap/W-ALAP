using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using System;
using System.Collections.Generic;

namespace Common.Api.ExigoWebService
{
    public partial class OrderDetailRequest
    {
        public static explicit operator OrderDetailRequest(ShoppingCartItem item)
        {
            var model = new OrderDetailRequest();
            if (item == null) return model;

            model.ItemCode = item.ItemCode;
            model.ParentItemCode = item.ParentItemCode;
            model.ItemDescription = item.Description;
            model.Quantity = item.Quantity;
            model.PriceEachOverride = item.PriceEachOverride;
            model.BusinessVolumeEachOverride = item.BusinessVolumeEachOverride;
            model.CommissionableVolumeEachOverride = item.CommissionableVolumeEachOverride;
            model.InventoryStatus = item.InventoryStatus;
            return model;
        }
    }

    public partial class OrderDetailRequest : IShoppingCartItem
    {
        public OrderDetailRequest() { }
        public OrderDetailRequest(IShoppingCartItem item)
        {
            ItemCode = item.ItemCode;
            Quantity = item.Quantity;
            ParentItemCode = item.ParentItemCode;
            ItemDescription = item.ItemDescription;
            Type = item.Type;
            PriceTypeID = item.PriceTypeID;
            Price = item.Price;
            PriceEachOverride = item.PriceEachOverride;
            BusinessVolumeEachOverride = item.BusinessVolumeEachOverride;
            CommissionableVolumeEachOverride = item.CommissionableVolumeEachOverride;
            ApplyDiscountType = item.ApplyDiscountType;
            AppliedAmount = item.AppliedAmount;
            if (item.ApplyDiscountType == DiscountType.RetailPromoFixed || item.ApplyDiscountType == DiscountType.RetailPromoPercent)
                TaxableEachOverride = item.PriceEachOverride;
            InventoryStatus = item.InventoryStatus;
        }

        public OrderDetailRequest(Product item)
        {
            ItemCode = item.ItemCode;
            Quantity = item.Quantity;
            PriceTypeID = item.PriceTypeID;
            ItemDescription = item.Description;
            Price = item.Price;
            PriceEachOverride = item.PriceEachOverride;
            BusinessVolumeEachOverride = item.BusinessVolumeEachOverride;
            CommissionableVolumeEachOverride = item.CommissionableVolumeEachOverride;
            InventoryStatus = item.InventoryStatus;
        }

        public Guid ID { get; set; }
        public string GroupMasterItemCode { get; set; }
        public string DynamicKitCategory { get; set; }
        public string ItemDescription { get; set; }
        public ShoppingCartItemType Type { get; set; }
        public int PriceTypeID { get; set; }
        public decimal Price { get; set; }
        public List<Discount> Discounts { get; set; }
        public int MaxKitQuantity { get; set; }
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