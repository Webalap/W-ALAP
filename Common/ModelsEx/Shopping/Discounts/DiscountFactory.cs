using Common.ServicesEx;
using Ninject;
using System;

namespace Common.ModelsEx.Shopping.Discounts
{
    /// <summary>
    /// The factory for creating discounts.
    /// </summary>
    public class DiscountFactory
    {
        #region Dependencies

        // TODO: Fix DI.  These aren't working for some reason.
        [Inject]
        public IEventService EventService { get; set; }

        [Inject]
        public IRewardService RewardService { get; set; }

        #endregion

        public Discount CreateDiscount(DiscountType discountType, decimal discountAmount)
        {
            switch (discountType)
            {
                case DiscountType.Fixed:
                    return new FixedDiscount(discountAmount);

                case DiscountType.Percent:
                    return new PercentDiscount(discountAmount);

                case DiscountType.RewardsCash:
                    return new CashRewardDiscount(discountAmount);

                case DiscountType.HalfOffCredits:
                    return new HalfOffDiscount(DiscountType.HalfOffCredits);

                //case DiscountType.BookingRewards:
                //    return new BookingRewardDiscount(discountAmount);

                case DiscountType.HostSpecial:
                    return new HostSpecialDiscount(discountAmount);

                case DiscountType.EBRewards:
                    return new EBRewardDiscount(discountAmount);

                case DiscountType.SAHalfOff:
                    return new HalfOffDiscount(DiscountType.SAHalfOff);

                case DiscountType.SAHalfOffOngoing:
                    return new HalfOffDiscount(DiscountType.SAHalfOffOngoing);

                case DiscountType.NewProductsLaunchReward:
                    return new NewProductsLaunchDiscount(discountAmount);

                case DiscountType.RecruitingReward:
                    return new RecruitingRewardDiscount(discountAmount);

                case DiscountType.EnrolleeReward:
                    return new EnrolleeRewardDiscount(discountAmount);
                case DiscountType.ProductCredit:
                    return new ProductCredit(discountAmount);
                case DiscountType.QualifiedDiscounts:
                    return new QualifiedDiscounts(DiscountType.ProductCredit,discountAmount);
                case DiscountType.RetailPromoFixed :
                    return new RetailFixedDiscount(discountAmount);
                default:
                    throw new NotImplementedException(string.Format("No implementation for {0}.", discountType));
            }
        }

        public Discount CreateDiscount(DiscountType discountType, Event.Event @event, decimal discountAmount)
        {
            switch (discountType)
            {
                case DiscountType.HostSpecial:

                    var hostSpecialReward = new RewardService().GetHostSpecialReward(@event.ActualDate);

                    hostSpecialReward.DiscountAmount = @event.HostSpecialReward.DiscountAmount;
                    hostSpecialReward.ItemCode = @event.HostSpecialReward.ItemCode;
                    hostSpecialReward.SalesThreshold = @event.HostSpecialReward.SalesThreshold;
                    hostSpecialReward.HasBeenRedeemed = @event.HostSpecialReward.HasBeenRedeemed;

                    return hostSpecialReward;

                default:
                    throw new NotImplementedException(string.Format("No implementation for {0}.", discountType));
            }
        }
        
        //Retail promo Discounts
        public Discount CreateDiscount(DiscountType discountType, decimal discountAmount, decimal? BV, decimal? CV)
        {
            switch (discountType)
            {
                case DiscountType.RetailPromoFixed:
                    return new RetailFixedDiscount(discountAmount, BV, CV);

                case DiscountType.RetailPromoPercent:
                    return new RetailPercentDiscount(discountAmount, BV, CV);

                default:
                    throw new NotImplementedException(string.Format("No implementation for {0}.", discountType));
            }
        }

        //Promo Code Discounts
        public Discount CreateDiscount(DiscountType discountType, decimal discountAmount, string promoCode, decimal? BV, decimal? CV)
        {

            switch ( discountType ) {
                case DiscountType.PromoCode:
                    return new PromoCodePercentDiscount(discountAmount,promoCode,BV,CV);
                default:
                    throw new NotImplementedException( string.Format( "No implementation for {0}.", discountType ) );
            }

        }

    }

}
