using Common.ModelsEx.Shopping.Discounts;
using Newtonsoft.Json.Linq;
using System;

namespace Common.Attributes.JsonConverters
{
    public class JsonDiscountConverter : JsonCreationConverter<Discount>
    {
        protected override Discount Create(Type objectType, JObject jObject)
        {
            var jToken = jObject["DiscountType"];

            if (jToken == null) return null;
            var discountType = jToken.Value<int>();

            switch(discountType)
            {
                case (int)DiscountType.Fixed:
                    return new FixedDiscount();
                case (int)DiscountType.Percent:
                    return new PercentDiscount();
                case (int)DiscountType.RewardsCash:
                    return new CashRewardDiscount();
                case (int)DiscountType.HalfOffCredits:
                    return new HalfOffDiscount(DiscountType.HalfOffCredits);
                //case (int)DiscountType.BookingRewards:
                //    return new BookingRewardDiscount();
                case (int)DiscountType.HostSpecial:
                    return new HostSpecialDiscount();
                case (int)DiscountType.EBRewards:
                    return new EBRewardDiscount();
                case (int)DiscountType.SAHalfOff:
                    return new HalfOffDiscount(DiscountType.SAHalfOff);
                case (int)DiscountType.SAHalfOffOngoing:
                    return new HalfOffDiscount(DiscountType.SAHalfOffOngoing);
                case (int)DiscountType.NewProductsLaunchReward:
                    return new NewProductsLaunchDiscount();
                case (int)DiscountType.RecruitingReward:
                    return new RecruitingRewardDiscount();
                case (int)DiscountType.EnrolleeReward:
                    return new EnrolleeRewardDiscount();
                case (int)DiscountType.RetailPromoFixed:
                    return new RetailFixedDiscount();
                case (int)DiscountType.RetailPromoPercent:
                    return new RetailPercentDiscount();
                case (int)DiscountType.ProductCredit:
                    return new ProductCredit();

                case (int)DiscountType.PromoCode:
                    return new PromoCodePercentDiscount();
                default:
                    return null;
            }
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }
}