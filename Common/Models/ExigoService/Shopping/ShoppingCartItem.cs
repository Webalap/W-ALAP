using Common;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace ExigoService
{

    public class ShoppingCartItem : IShoppingCartItem {

        //public ShoppingCartItem(Product product)
        //{
        //    this.AppliedAmount = product.AppliedAmount;
        //    this.ApplyDiscountType = product.ApplyDiscountType;
        //    this.BusinessVolumeEachOverride = product.BusinessVolumeEachOverride;
        //    this.CommissionableVolumeEachOverride = product.CommissionableVolumeEachOverride;
        //    this.Description = product.Description;
        //    this.Discounts = product.Discounts;
        //    this.ID = product.ID;
        //    this.ItemCode = product.ItemCode;
        //    this.LargePicture = product.LargePicture;
        //    this.LongDetail = product.LongDetail1;
        //    this.LongDetail2 = product.LongDetail2;
        //    this.LongDetail3 = product.LongDetail3;
        //    this.LongDetail4 = product.LongDetail4;
        //    this.Price = product.Price;
        //    this.PriceEachOverride = product.PriceEachOverride;
        //    this.PriceTypeID = product.PriceTypeID;
        //    this.Quantity = product.Quantity;
        //    this.ShortDetail = product.ShortDetail;
        //    this.ShortDetail2 = product.ShortDetail2;
        //    this.ShortDetail3 = product.ShortDetail3;
        //    this.ShortDetail4 = product.ShortDetail4;
        //    this.SmallPicture = product.SmallPicture;
        //    this.TinyPicture = product.TinyPicture;
        //}
        public static ShoppingCartItem Create() {

            return new ShoppingCartItem();

        }


        public static ShoppingCartItem Create( IShoppingCartItem oldCartItem) {

            var newCartItem = new ShoppingCartItem( oldCartItem );

            Item item = Exigo.GetItem( newCartItem.ItemCode );
            newCartItem.Category = oldCartItem.Category;
            newCartItem.Description = item.ItemDescription;
            newCartItem.ItemDescription = item.ItemDescription;
            newCartItem.ShortDetail = item.ShortDetail1;
            newCartItem.ShortDetail2 = item.ShortDetail2;
            newCartItem.ShortDetail3 = item.ShortDetail3;
            newCartItem.ShortDetail4 = item.ShortDetail4;

            newCartItem.LongDetail = item.LongDetail1;
            newCartItem.LongDetail2 = item.LongDetail2;
            newCartItem.LongDetail3 = item.LongDetail3;
            newCartItem.LongDetail4 = item.LongDetail4;

            newCartItem.TinyPicture = item.TinyImageUrl;
            newCartItem.SmallPicture = item.SmallImageUrl;
            newCartItem.LargePicture = item.LargeImageUrl;
            newCartItem.ItemEventId = oldCartItem.ItemEventId;
            newCartItem.MaxKitQuantity = oldCartItem.MaxKitQuantity;
            newCartItem.InventoryStatus = oldCartItem.InventoryStatus;
            newCartItem.BusinessVolumeEachOverride = oldCartItem.BusinessVolumeEachOverride;
            newCartItem.CommissionableVolumeEachOverride = oldCartItem.CommissionableVolumeEachOverride;
            newCartItem.OtherCheck1 = item.OtherCheck1;
            newCartItem.OtherCheck2 = item.OtherCheck2;
            newCartItem.OtherCheck3 = item.OtherCheck3;
            newCartItem.OtherCheck4 = item.OtherCheck4;
            newCartItem.OtherCheck5 = item.OtherCheck5;
            newCartItem.Field1 = item.Field1;
            newCartItem.Field2 = item.Field2;
            newCartItem.OtherCheck5 = item.OtherCheck5;
            return newCartItem;

        }




        private Guid _id;


        private ShoppingCartItem() {

            ID                  = Guid.NewGuid();

            ItemCode            = string.Empty;
            ParentItemCode      = string.Empty;

            Quantity            = 0;
            
            DynamicKitCategory  = string.Empty;
            GroupMasterItemCode = string.Empty;

            Type                = ShoppingCartItemType.Order;

            PriceTypeID         = 0;
            Price               = 0M;
            PriceEachOverride   = 0M;

            BusinessVolumeEachOverride = 0M;
            CommissionableVolumeEachOverride = 0M;
            Description = string.Empty;
            ItemDescription  = string.Empty;
            ApplyDiscountType   = DiscountType.Unknown;
            AppliedAmount       = 0M;
            Discounts           = new List<Discount>();
            Category = string.Empty;
        }


        private ShoppingCartItem( IShoppingCartItem item ) {

            ID                  = ( item.ID != Guid.Empty ) ? item.ID : Guid.NewGuid();

            ItemCode            = GlobalUtilities.Coalesce( item.ItemCode );
            ParentItemCode      = GlobalUtilities.Coalesce( item.ParentItemCode );
            Description = item.ItemDescription;
            ItemDescription = item.ItemDescription;
           Quantity            = item.Quantity;

            DynamicKitCategory  = GlobalUtilities.Coalesce( item.DynamicKitCategory );
            GroupMasterItemCode = GlobalUtilities.Coalesce( item.GroupMasterItemCode );

            Type                = item.Type;
            Category = item.Category;
           PriceTypeID         = item.PriceTypeID;
            Price               = item.Price;
            PriceEachOverride   = item.PriceEachOverride;

            BusinessVolumeEachOverride = item.BusinessVolumeEachOverride;
            CommissionableVolumeEachOverride = item.CommissionableVolumeEachOverride;

            ApplyDiscountType   = item.ApplyDiscountType;
            AppliedAmount       = item.AppliedAmount;
            InventoryStatus = item.InventoryStatus;
            OtherCheck1 = item.OtherCheck1;
            OtherCheck2 = item.OtherCheck2;
            OtherCheck3 = item.OtherCheck3;
            OtherCheck4 = item.OtherCheck4;
            OtherCheck5 = item.OtherCheck5;
            Field1 = item.Field1;
            Field2 = item.Field2;


            if ( item.ApplyDiscountType == DiscountType.Unknown ) {

                Discounts = new List<Discount>();

            } else {

                var factory = new DiscountFactory();

                Discounts = new List<Discount> { factory.CreateDiscount( ApplyDiscountType, item.AppliedAmount ) };

            }

        }

        public ShoppingCartItem(Product product)
        {
            if (product == null)
            {
                return;
            }
            this.ItemCode = product.ItemCode;
            this.Price = product.Price;
            this.PriceEachOverride = product.PriceEachOverride;
            this.BusinessVolumeEachOverride = product.BusinessVolumeEachOverride;
            this.CommissionableVolumeEachOverride = product.CommissionableVolumeEachOverride;
            this.Discounts = product.Discounts;
            this.ApplyDiscountType = product.ApplyDiscountType;
            this.AppliedAmount = product.AppliedAmount;
            this.Quantity = product.Quantity;
            this.Type = ShoppingCartItemType.Order;
            this.Description = product.Description;
            this.ItemDescription = product.Description;
            this.ShortDetail = product.ShortDetail;
            this.ShortDetail2 = product.ShortDetail2;
            this.ShortDetail3 = product.ShortDetail3;
            this.ShortDetail4 = product.ShortDetail4;
            this.SmallPicture = product.SmallPicture;
            this.TinyPicture=product.TinyPicture;
            this.LargePicture = product.LargePicture;
            this.LongDetail = product.LongDetail1;
            this.LongDetail2 = product.LongDetail2;
            this.LongDetail3 = product.LongDetail3;
            this.LongDetail4 = product.LongDetail4;
            this.InventoryStatus = product.InventoryStatus;
            this.PriceTypeID = product.PriceTypeID;
            this.OtherCheck1 = product.OtherCheck1;
            this.OtherCheck2 = product.OtherCheck2;
            this.OtherCheck3 = product.OtherCheck3;
            this.OtherCheck4 = product.OtherCheck4;
            this.OtherCheck5 = product.OtherCheck5;
            this.Category = product.Category;
            this.Field1 = product.Field1;
            this.Field2 = product.Field2;
        }


        public Guid ID {
            get { return _id; }
            set { _id = value; }
        }


        public string ItemCode { get; set; }

        public int PriceTypeID { get; set; }     
        public decimal Price { get; set; }
        public decimal? PriceEachOverride { get; set; }

        public decimal? BusinessVolumeEachOverride { get; set; }
        public decimal? CommissionableVolumeEachOverride { get; set; }

        public List<Discount> Discounts { get; set; }
        public DiscountType ApplyDiscountType { get; set; }
        public string Category { get; set; }
        public decimal AppliedAmount { get; set; }
        public decimal Quantity { get; set; }
        public int MaxKitQuantity { get; set; }
        public string ParentItemCode { get; set; }
        public string GroupMasterItemCode { get; set; }
        public string DynamicKitCategory { get; set; }
        public ShoppingCartItemType Type { get; set; }

        
        public string Description { get; set; }
        public string ItemDescription { get; set; }
        public string ShortDetail { get; set; }
        public string ShortDetail2 { get; set; }
        public string ShortDetail3 { get; set; }
        public string ShortDetail4 { get; set; }

        [AllowHtml]
        public string LongDetail { get; set; }

        [AllowHtml]
        public string LongDetail2 { get; set; }

        [AllowHtml]
        public string LongDetail3 { get; set; }

        [AllowHtml]
        public string LongDetail4 { get; set; }

        public string InventoryStatus { get; set; }

        public string LargePicture { get; set; }
        public string SmallPicture { get; set; }
        public string TinyPicture { get; set; }

        public int ItemEventId { get; set; }

        public bool OtherCheck1 { get; set; }

        public bool OtherCheck2 { get; set; }

        public bool OtherCheck3 { get; set; }

        public bool OtherCheck4 { get; set; }

        public bool OtherCheck5 { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }

        public decimal DiscountedPrice { 
            get { return ( Discounts != null ) ? Discounts.Sum( d => d.Apply( this ) ) : 0; }
        }

        public bool IsDynamicKitMember {
            get { return !String.IsNullOrEmpty( ParentItemCode ); }
        }



        public Product ToProduct() {

            return new Product {

                ItemCode = ItemCode,
                PriceTypeID = PriceTypeID,
                Price = Price,

                ApplyDiscountType = ApplyDiscountType,
                Discounts = Discounts,
                Quantity = Quantity,
               

                PriceEachOverride = PriceEachOverride,
                BusinessVolumeEachOverride = BusinessVolumeEachOverride,
                CommissionableVolumeEachOverride = CommissionableVolumeEachOverride,

                Description = Description,

                ShortDetail = ShortDetail,
                ShortDetail2 = ShortDetail2,
                ShortDetail3 = ShortDetail3,
                ShortDetail4 = ShortDetail4,

                LongDetail1 = LongDetail,
                LongDetail2 = LongDetail2,
                LongDetail3 = LongDetail3,
                LongDetail4 = LongDetail4,
                InventoryStatus=InventoryStatus

            };

        }

    }

}
