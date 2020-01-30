using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;

namespace Common.ServicesEx
{
    /// <summary>
    /// The interface for validating if a discount can be applied to a product.
    /// </summary>
    public interface IDiscountValidator
    {
        /// <summary>
        /// Checks if the <paramref name="discount"/> can be applied to the <paramref name="product"/>
        /// and returns true or false respectively.
        /// </summary>
        /// <param name="discount">The discount.</param>
        /// <param name="product">The product.</param>
        /// <param name="event"></param>
        /// <returns>A boolean.</returns>
        bool IsValidFor(Discount discount, Product product, Event @event = null);
    }
}
