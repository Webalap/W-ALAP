using Common.Filters;
using Common.ModelsEx.Shopping.Discounts;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Common.ModelBinders
{
    public class CustomModelBinder : DefaultModelBinder
    {

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;
            return base.BindModel(controllerContext, bindingContext);
        }

        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            // Check if the property has the PropertyBinderAttribute, meaning it's specifying a different binder to use.
            try
            {
                var propertyBinderAttribute = TryFindPropertyBinderAttribute(propertyDescriptor);
                if (propertyBinderAttribute != null)
                {
                    var binder = CreateBinder(propertyBinderAttribute);
                    var value = binder.BindModel(controllerContext, bindingContext);
                    try
                    {
                        propertyDescriptor.SetValue(bindingContext.Model, value);
                    }
                    catch { }
                }
                else // revert to the default behavior.
                {
                   
                    base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
                }
            }
            catch (Exception)
            {
            }
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType != typeof(Discount)) return base.CreateModel(controllerContext, bindingContext, modelType);
            var discountType = 0;
            var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "[DiscountType]");

            if (null == result || !int.TryParse(result.AttemptedValue, out discountType)) return null;

            switch (discountType)
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
                    var hostSpecialDiscount = new HostSpecialDiscount();
                    bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => hostSpecialDiscount, typeof(HostSpecialDiscount));
                    return hostSpecialDiscount;
                case (int)DiscountType.EBRewards:
                    var ebRewardDiscount = new EBRewardDiscount();
                    bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => ebRewardDiscount, typeof(EBRewardDiscount));
                    return ebRewardDiscount;
                //return new EBRewardDiscount();
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
                case (int)DiscountType.PromoCode:
                    return new PromoCodePercentDiscount();
                default:
                    return null;
            }
        }

        IModelBinder CreateBinder(PropertyBinderAttribute propertyBinderAttribute)
        {
            return (IModelBinder)DependencyResolver.Current.GetService(propertyBinderAttribute.BinderType);
        }

        PropertyBinderAttribute TryFindPropertyBinderAttribute(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.Attributes
              .OfType<PropertyBinderAttribute>()
              .FirstOrDefault();
        }
    }
}