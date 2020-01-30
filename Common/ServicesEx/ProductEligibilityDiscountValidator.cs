using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using System;
using System.Linq;

namespace Common.ServicesEx
{
    /// <summary>
    /// A discount validator that ensures a product is eligible for a given discount type.
    /// </summary>
    public class ProductEligibilityDiscountValidator : IDiscountValidator
    {
        /// <summary>
        /// Checks if the <paramref name="discount"/> can be applied to the <paramref name="product"/>
        /// and returns true or false respectively.
        /// </summary>
        /// <param name="discount">The discount.</param>
        /// <param name="product">The product.</param>
        /// <returns>A boolean.</returns>
        bool IDiscountValidator.IsValidFor(Discount discount, Product product, Event @event)
        {
            if (discount == null)
                throw new ArgumentNullException("discount");
            if (product == null)
                throw new ArgumentNullException("product");

            // Don't allow more than one discount per product.
            //if (product.Discounts.Count > 2)
            //{
            //    return false;
            //}

            // Retrieve the eligible discount from product based on 
            // the discount parameter that's being validated.
            var eligibleDiscount = product.GetEligibleDiscountByType(discount.DiscountType);

            // If we get a hit, also ensure that the number of discounts applied doesn't
            // exceed quantity of products.  This is ensure that each discount can be 
            // applied to only single product.  Multiple discounts applied to a single
            // product is not supported.
            if (null == eligibleDiscount || product.Quantity > product.Discounts.Count + 1)
                throw new ApplicationException(
                    "Either the discount applied to the product is not valid or attempted to apply muliple discounts to a single product.");
            // Ensure the eligible discount was properly created from the beginning.
            switch (eligibleDiscount.DiscountType)
            {
                case DiscountType.RewardsCash:
                    return (!string.IsNullOrWhiteSpace(product.Field4));
                case DiscountType.HalfOffCredits:
                    return (!string.IsNullOrWhiteSpace(product.Field5));
                case DiscountType.SAHalfOff:
                    return product.HalfOffRewardsCreditsEligible;
                case DiscountType.SAHalfOffOngoing:
                    return product.HalfOffRewardsCreditsEligible;
                case DiscountType.NewProductsLaunchReward:
                    return product.NewProductsLaunchRewardsCreditsEligible;
                //case DiscountType.BookingRewards:
                //    return true; //TODO: Do another check for Booking Rewards Owner?
                case DiscountType.EBRewards:
                    return true;
                case DiscountType.RecruitingReward: //Same as HalfOff as all the products on that list should be available here also.
                    return product.HalfOffRewardsCreditsEligible;
                case DiscountType.EnrolleeReward: //Same as HalfOff as all the products on that list should be available here also.
                    return product.HalfOffRewardsCreditsEligible;
                case DiscountType.HostSpecial:
                    if (null == @event)
                    {
                        throw new ApplicationException("This reward can only be redeemed when shopping an event.");
                    }

                    if (product.ItemCode != @event.HostSpecialReward.ItemCode)
                    {
                        throw new ApplicationException("This product is not eligible for this reward.");
                    }

                    //Exclude SA from the host special if this is their party
                    if (@event.Host.CustomerTypeID == CustomerTypes.IndependentStyleAmbassador && @event.Creator.CustomerID == @event.Host.CustomerID )
                    {
                        throw new ApplicationException("Host Reward is not available for self-hosted party.");
                    }

                    if (((HostSpecialDiscount)discount).SalesThreshold >= @event.PartySalesTotal)
                    {
                        throw new ApplicationException("The total event sales does not qualify for this reward.");
                    }

                    var count = (from d in product.Discounts
                        where typeof(HostSpecialDiscount) == d.GetType()
                        select d).Count();

                    // If count is 1 then the reward was already redeemed; return false.
                    return count != 1;

                case DiscountType.Unknown:
                    break;
                case DiscountType.Fixed:
                    break;
                case DiscountType.Percent:
                    break;
                case DiscountType.RetailPromoFixed:
                    return true;
                case DiscountType.RetailPromoPercent:
                    return true;
                default:
                    return false;
            }
            throw new ApplicationException("Either the discount applied to the product is not valid or attempted to apply muliple discounts to a single product.");
        }
    }
}