using Common.Attributes.JsonConverters;
using ExigoService;
using Newtonsoft.Json;

namespace Common.ModelsEx.Shopping.Discounts
{
    /// <summary>
    /// This object encapsulates a discount at the product level.
    /// </summary>
    [JsonConverter(typeof(JsonDiscountConverter))]
    public abstract class Discount
    {
        protected Discount() { }

        /// <summary>
        /// Constructs the discount.
        /// </summary>
        /// <param name="discountType">The type of discount.</param>
        /// <param name="discountAmount">The amount of the discount.</param>
        protected Discount(DiscountType discountType, decimal discountAmount) //, Product product)
        {
            //Product = product;
            DiscountType = discountType;
            DiscountAmount = discountAmount;
        }

        /// <summary>
        /// Gets the discount type.
        /// </summary>
        public DiscountType DiscountType { get; set; }

        /// <summary>
        /// Gets the discount amount.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets the applied amount of the discount
        /// </summary>
        public decimal AppliedAmount { get; set; }

        /// <summary>
        /// Brief word or two to uniquely identify the type of discount, such as 'Cash' or 'Credit'
        /// </summary>
        public abstract string DisplayText { get; }
        
        /// <summary>
        /// Longer description of what the discount is or how it works, ideal for a tooltip
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// The reward program the discount applies to.  This value is defaulted to the Display Text of the discount however can be overridden.
        /// </summary>
        public virtual string RewardProgram { get { return DisplayText; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public abstract decimal Apply(Product product);
        public abstract decimal Apply(ShoppingCartItem item);
        public abstract decimal Apply(Item item);
        /// <summary>
        /// True if the amount the item is taxed on should be overridden to the discounted price of the item.
        /// </summary>
        public virtual bool OverrideTaxableAmount { get { return true; } }

        public virtual decimal? OverrideBVAmount { get; set; }

        public virtual decimal? OverrideCVAmount { get; set; }

        public virtual string PromoCode { get; set; }
    }
}