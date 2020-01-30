using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using System;
using System.Collections.Generic;

namespace ExigoService
{
    public class Item : IItem
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Notes { get; set; }
        public string DynamicKitDescription { get; set; }
        public int MaxKitQuantity { get; set; }
        public decimal Weight { get; set; }
        public int ItemTypeID { get; set; }
        public string InventoryStatus { get; set; }
        public string TinyImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
        public string ShortDetail1 { get; set; }
        public string ShortDetail2 { get; set; }
        public string ShortDetail3 { get; set; }
        public string ShortDetail4 { get; set; }
        public string LongDetail1 { get; set; }
        public string LongDetail2 { get; set; }
        public string LongDetail3 { get; set; }
        public string LongDetail4 { get; set; }

        public List<Discount> Discounts { get; set; }

        public DiscountType ApplyDiscountType { get; set; }
        public decimal AppliedAmount { get; set; }

        public bool IsVirtual { get; set; }
        public bool AllowOnAutoOrder { get; set; }
        public bool IsGroupMaster { get; set; }
        public string GroupMasterItemDescription { get; set; }
        public string GroupMembersDescription { get; set; }
        public IEnumerable<ItemGroupMember> GroupMembers { get; set; }

        public bool IsDynamicKitMaster { get; set; }
        public IEnumerable<DynamicKitCategory> DynamicKitCategories { get; set; }

        public int PriceTypeID { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceEachOverride { get; set; }
        public decimal BV { get; set; }
        public decimal CV { get; set; }
        public decimal? BusinessVolumeEachOverride { get; set; }
        public decimal? CommissionableVolumeEachOverride { get; set; }
        public decimal OtherPrice1 { get; set; }
        public decimal OtherPrice2 { get; set; }
        public decimal OtherPrice3 { get; set; }
        public decimal OtherPrice4 { get; set; }
        public decimal OtherPrice5 { get; set; }
        public decimal OtherPrice6 { get; set; }
        public decimal OtherPrice7 { get; set; }
        public decimal OtherPrice8 { get; set; }
        public decimal OtherPrice9 { get; set; }
        public decimal OtherPrice10 { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }

        public bool OtherCheck1 { get; set; }
        public bool OtherCheck2 { get; set; }
        public bool OtherCheck3 { get; set; }
        public bool OtherCheck4 { get; set; }
        public bool OtherCheck5 { get; set; }



        // IShoppingCartItem Members
        public Guid ID { get; set; }
        public decimal Quantity { get; set; }
        public string ParentItemCode { get; set; }
        public string GroupMasterItemCode { get; set; }
        public string DynamicKitCategory { get; set; }
        public ShoppingCartItemType Type { get; set; }

        public int ItemEventId { get; set; }

        public virtual void ApplyDiscount(Discount discount)
        {
            // Apply the discount
            PriceEachOverride = discount.Apply(this);
            // Add the discount to this product's list.
            Discounts.Add(discount);
        }
        public Item(Product product)
        {
            ID = product.ID;
            ItemCode = product.ItemCode;
            ItemDescription = product.Description;
            Description = product.Description;
            ItemTypeID = product.ItemTypeID;
            Notes = product.Notes;
            LongDetail1 =   product.LongDetail1;
            LongDetail2 = product.LongDetail2;
            LongDetail3 = product.LongDetail3;
            Price = product.Price;
            PriceEachOverride = product.DiscountedPrice;
            PriceTypeID = product.PriceTypeID;
            ShortDetail1 = product.ShortDetail;
            ShortDetail2 = product.ShortDetail2;
            ShortDetail3 = product.ShortDetail3;
            ShortDetail4 = product.ShortDetail4;
            TinyImageUrl = product.TinyPicture; 
            SmallImageUrl =product.SmallPicture;
            LargeImageUrl =product.LargePicture;
            Quantity = product.Quantity;
            Discounts = product.Discounts;
            ApplyDiscountType = product.ApplyDiscountType;
            Field4 = product.Field4;
            Field5 = product.Field5;
            Field6 = product.Field6;
            BusinessVolumeEachOverride = product.BusinessVolumeEachOverride;
            CommissionableVolumeEachOverride = product.CommissionableVolumeEachOverride;
            MaxKitQuantity = product.MaxKitQuantity;
            InventoryStatus = product.InventoryStatus;
        }
        public Item() { }
    }
}
