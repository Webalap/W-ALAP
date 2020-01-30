using System;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using Common.ModelsEx.Event;

namespace Common.ServicesEx
{
    /// <summary>
    /// A simple discount validator that ensures a product
    /// doesn't already have a discount applied to it.
    /// </summary>
    public class SimpleDiscountValidator : IDiscountValidator
    {
        /// <summary>
        /// Checks if the <paramref name="discount"/> can be applied to the <paramref name="product"/>
        /// and returns true or false respectively.
        /// </summary>
        /// <param name="discount">The discount.</param>
        /// <param name="product">The product.</param>
        /// <param name="event"></param>
        /// <returns>A boolean.</returns>
        bool IDiscountValidator.IsValidFor(Discount discount, Product product, Event @event)
        {
            if (discount == null)
                throw new ArgumentNullException("discount");
            if (product == null)
                throw new ArgumentNullException("product");

            // Hey, if this product doesn't already have
            // a discount applied to it, go for it!
            return (product.Discounts.Count > 0);
        }
    }
}