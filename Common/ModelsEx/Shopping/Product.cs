using Common.Api.ExigoWebService;
using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Common.ModelsEx.Shopping
{
    public class Product
    {
        public Product()
        {
            Quantity = 1;
            Discounts = new List<Discount>(0);
            EligibleDiscounts = new List<Discount>(0);

        }

        public Product(ItemResponse item)
            : this()
        {

            ItemCode = item.ItemCode;
            CategoryId = item.CategoryID;
            CategoryName = item.Category; 
            Category = item.Category; 
            //ItemTypeID = item.ItemTypeID;
            //Quantity = item.Quantity;
            MaxKitQuantity = item.MaxAllowedOnOrder;
            //Notes = item.Notes;
            IsDynamicKitMaster = item.IsDynamicKitMaster;
            KitMembers = item.KitMembers;
            
            Price = item.Price;

            Description = item.Description;

            ShortDetail = item.ShortDetail;
            ShortDetail2 = item.ShortDetail2;
            ShortDetail3 = item.ShortDetail3;
            ShortDetail4 = item.ShortDetail4;

            LongDetail1 = item.LongDetail;
            LongDetail2 = item.LongDetail2;
            LongDetail3 = item.LongDetail3;
            LongDetail4 = item.LongDetail4;


            CategoryId = item.CategoryID;   // We just did this a few lines above, why are we doing it again?

            BusinessVolumeEachOverride = item.BusinessVolume;
            CommissionableVolumeEachOverride = item.CommissionableVolume;
            InventoryStatus = item.InventoryStatus.ToString();


            // Convert pictures from filenames to URLs.
            LargePicture = GlobalUtilities.GetProductImagePath(item.LargePicture, item.ItemCode);
            SmallPicture = GlobalUtilities.GetProductImagePath(item.SmallPicture, item.ItemCode);
            TinyPicture = GlobalUtilities.GetProductImagePath(item.TinyPicture, item.ItemCode);
            //for landed Cost Api 
            Field1 = item.Field1;
            Field2 = item.Field2;


            // Product discount fields
            Field4 = item.Field4;
            Field5 = item.Field5;
            Field6 = item.Field6;

            OtherPrice1 = item.Other1Price;
            OtherPrice2 = item.Other2Price;
            OtherPrice3 = item.Other3Price;
            OtherPrice4 = item.Other4Price;
            OtherPrice5 = item.Other5Price;
            OtherPrice6 = item.Other6Price;
            OtherPrice7 = item.Other7Price;
            OtherPrice8 = item.Other8Price;
            OtherPrice9 = item.Other9Price;
            OtherPrice10 = item.Other10Price;

            OtherCheck1 = item.OtherCheck1;
            OtherCheck2 = item.OtherCheck2;
            OtherCheck3 = item.OtherCheck3;
            OtherCheck4 = item.OtherCheck4;
            OtherCheck5 = item.OtherCheck5;
        }

        public Product(Item item)
            : this()
        {
            ItemCode = item.ItemCode;

            Price = item.Price;
            Description = item.ItemDescription;
            ShortDetail = item.ShortDetail1;
            ShortDetail2 = item.ShortDetail2;
            ShortDetail3 = item.ShortDetail3;
            ShortDetail4 = item.ShortDetail4;
            LongDetail1 = item.LongDetail1;
            LongDetail2 = item.LongDetail2;
            LongDetail3 = item.LongDetail3;

            //InventoryStatus = Convert.ToString(item.InventoryStatus);

            // Convert pictures from filenames to URLs.
            LargePicture = GlobalUtilities.GetProductImagePath(item.LargeImageUrl, item.ItemCode);
            SmallPicture = GlobalUtilities.GetProductImagePath(item.SmallImageUrl, item.ItemCode);
            TinyPicture = GlobalUtilities.GetProductImagePath(item.TinyImageUrl, item.ItemCode);
            //for landed Cost Api 
            Field1 = item.Field1;
            Field2 = item.Field2;
            // Product discount fields
            Field4 = item.Field4;
            Field5 = item.Field5;
            Field6 = item.Field6;

            OtherPrice1 = item.OtherPrice1;
            OtherPrice2 = item.OtherPrice2;
            OtherPrice3 = item.OtherPrice3;
            OtherPrice4 = item.OtherPrice4;
            OtherPrice5 = item.OtherPrice5;
            OtherPrice6 = item.OtherPrice6;
            OtherPrice7 = item.OtherPrice7;
            OtherPrice8 = item.OtherPrice8;
            OtherPrice9 = item.OtherPrice9;
            OtherPrice10 = item.OtherPrice10;
            InventoryStatus = item.InventoryStatus;
            BusinessVolumeEachOverride = item.BusinessVolumeEachOverride;
            CommissionableVolumeEachOverride = item.CommissionableVolumeEachOverride;
            MaxKitQuantity = item.MaxKitQuantity;
        }

        //public ItemResponse Item { get; set; }
        public Guid ID { get; set; }
        public string ItemCode { get; set; }
        public int CategoryId { get; set; }  
        public string CategoryName { get; set; } 
        public string Category { get; set; } 
        public decimal Price { get; set; }
        public int PriceTypeID { get; set; }
        public int ItemTypeID { get; set; }
        public string Notes { get; set; }
        public bool IsDynamicKitMaster { get; set; }
        public int MaxKitQuantity { get; set; }
        public string ParentItemCode { get; set; }
        public KitMemberResponse[] KitMembers { get; set; }
        public string Description { get; set; }

        public string ShortDetail { get; set; }
        public string ShortDetail2 { get; set; }
        public string ShortDetail3 { get; set; }
        public string ShortDetail4 { get; set; }

        [AllowHtml]
        public string LongDetail1 { get; set; }
        [AllowHtml]
        public string LongDetail2 { get; set; }
        [AllowHtml]
        public string LongDetail3 { get; set; }
        [AllowHtml]
        public string LongDetail4 { get; set; }

        public string LargePicture { get; set; }
        public string SmallPicture { get; set; }
        public string TinyPicture { get; set; }

        public string InventoryStatus { get; set; }
        public decimal? PriceEachOverride { get; set; }


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
        public string FirstItemInGroup { get; set; }

        // for landed Cost Api
        //country
        public string Field1 { get; set; }
        //HS code
        public string Field2 { get; set; }
        // TODO: Probably should rename to make these fields more obvious 
        // of their intent.
        public string Field4 { get; set; } // Rewards Cash Eligible
        public string Field5 { get; set; } // 1/2 Rewards Credits Eligible
        public string Field6 { get; set; } // New Products Launch Credits Eligible

        public decimal AppliedAmount { get; set; } // Product Credit


        public bool NewProductsLaunchRewardsCreditsEligible
        {
            get { return !String.IsNullOrEmpty(Field6) && Field6 != "No"; }
        }

        public bool HalfOffRewardsCreditsEligible
        {
            get { return !String.IsNullOrEmpty(Field5) && Field5 != "No"; }
        }

        public bool RetailPromoEligible { get; set; }

        public decimal Quantity { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal DiscountedPrice
        {
            get { return Discounts.Sum(d => d.Apply(this)); }
        }

        public List<Discount> Discounts { get; set; }

        public List<Discount> EligibleDiscounts { get; set; }

        public DiscountType ApplyDiscountType { get; set; }

        public decimal? BusinessVolumeEachOverride { get; set; }

        public decimal? CommissionableVolumeEachOverride { get; set; }

        public bool OtherCheck1 { get; set; }

        public bool OtherCheck2 { get; set; }

        public bool OtherCheck3 { get; set; }

        public bool OtherCheck4 { get; set; }

        public bool OtherCheck5 { get; set; }


        public decimal Subtotal
        {

            get
            {

                if (0 != Discounts.Count)
                {

                    return (Price * Quantity) - Discounts.Sum(d => d.AppliedAmount * Quantity);

                }

                return (Price * Quantity);

            }

        }



        public virtual Product AddToOrder(IOrder order)
        {

            var existing = order.Products
                .FirstOrDefault(item => item.ItemCode == ItemCode && item.CategoryId == CategoryId && item.ApplyDiscountType == ApplyDiscountType);

            if (existing != default(Product))
            {

                // Update quantity of existing item.
                existing.Quantity += this.Quantity;

                // Need to return existing product as it is not the
                // same instance of the product being added to cart.
                return existing;

            }

            // Add the new item to the order.
            order.Products.Add(this);

            // If this is the first product added, return this instance.
            return this;

        }


        public virtual void RemoveFromOrder(IOrder order, bool removeAllQty = false)
        {

            var existing = order.Products
                .FirstOrDefault(item => item.ItemCode == ItemCode && item.CategoryId == CategoryId && item.ApplyDiscountType == ApplyDiscountType);

            if (existing == default(Product))
            {
                return;
            }

            // Update quantity of existing item.
            existing.Quantity--;

            if (removeAllQty || existing.Quantity <= 0)
            {

                // Remove the quantity from the line item.
                order.Products.Remove(existing);

            }

        }


        public virtual Discount GetEligibleDiscountByType(DiscountType discountType)
        {

            return (from e in EligibleDiscounts
                    where e.DiscountType.Equals(discountType)
                    select e).FirstOrDefault();

        }


        /// <summary>
        /// Applies the <paramref name="discount"/> to this product.
        /// </summary>
        /// <param name="discount">The discount to apply.</param>
        public virtual void ApplyDiscount(Discount discount)
        {

            // Apply the discount
            discount.Apply(this);

            // Add the discount to this product's list.
            Discounts.Add(discount);

        }

        public static Product GetProduct(string itemCode)
        {
            var item = Exigo.GetItem(itemCode);
            if (item != null)
                return new Product(item);
            return new Product();
        }
        /// <summary>
        /// Unapplies the <paramref name="discount"/> from this product.
        /// </summary>
        /// <param name="discount">The discount to remove.</param>
        public virtual void UnapplyDiscount(Discount discount)
        {

            // Attempt to remove the discount from the list.
            Discounts.Remove(discount);

        }



    }

}
