using Common;
using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using Common.ServicesEx;
using Common.ServicesEx.Rewards;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ReplicatedSite.Controllers
{
    /// <summary>
    /// An abstraction of a shopping controller. This class should
    /// be extended and implemented to inherit common shopping
    /// functionality as well as hooks for standard actions.
    /// </summary>
    public abstract class AbstractShoppingController : Controller
    {
        #region Dependencies

        /// <summary>
        /// The discount validator injected by our IoC.
        /// </summary>
        [Inject]
        public IDiscountValidator DiscountValidator { get; set; }

        [Inject]
        public IRewardService RewardService { get; set; }

        #endregion

        /// <summary>
        /// Returns the cart as JSON.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Cart()
        {
            var cart = GetShoppingCart();
            cart.UpdateTotals();

            return new JsonNetResult(cart);
        }

        /// <summary>
        /// Returns the view of the customer's cart.
        /// </summary>
        public virtual ActionResult Basket()
        {
            var cart = GetShoppingCart();
            cart.UpdateTotals();

            return View(cart);
        }

        /// <summary>
        /// Adds the product to the cart.
        /// </summary>
        /// <param name="product">The product to add/update.</param>
        /// <returns></returns>
        public virtual ActionResult AddToCart(Product product)
        {
            var shoppingCart = GetShoppingCart();

            try
            {
                product.AddToOrder(shoppingCart);
                return new JsonNetResult(new
                {
                    status = 200,
                    cart = shoppingCart
                });
            }
            catch (Exception)
            {
                return new JsonNetResult(new
                {
                    status = 500,
                    cart = shoppingCart
                });
            }
        }

        // TODO: This method can replace the other AddToCart method once it's implemented.
        protected ActionResult AddToCart(Product product, Discount discount = null, IIndividualReward reward = null, Event @event = null)
        {
            var shoppingCart = GetShoppingCart();

            try
            {
                while(product.Discounts.Any())
                {
                    product.UnapplyDiscount(product.Discounts.First());
                }

                // Add the product to the order.
                var p = product.AddToOrder(shoppingCart);

                RewardsAccount pointAccount = null;

                // Validate that the discount can be applied
                // to the product using our validator.
                if (null != discount)
                {
                    bool eventAllowsDiscount = shoppingCart.ShopperIsHost || shoppingCart.ShopperIsEventSa; // || shoppingCart.ShopperIsBookingRewardsOwner;
                    bool rewardAllowsDiscount = reward != null && reward.IsRewardDiscount(discount) && reward.AllowAddToCart(product, shoppingCart.Products);

                    if ((eventAllowsDiscount || rewardAllowsDiscount) && DiscountValidator.IsValidFor(discount, p, @event))
                    {
                        // TODO: Need to find a better way to map discounts and which point accounts
                        // are deducted based on the discount.
                        switch (discount.DiscountType)
                        {
                            case DiscountType.RewardsCash:
                                pointAccount = shoppingCart.GetRewardsAccount(PointAccounts.HostRewardsCash);
                                discount.DiscountAmount = pointAccount.AmountRemaining;
                                break;
                            case DiscountType.RecruitingReward:
                                pointAccount = shoppingCart.GetRewardsAccount(PointAccounts.RecruitingRewards);
                                discount.DiscountAmount = pointAccount.AmountRemaining;
                                break;
                            case DiscountType.EnrolleeReward:
                                pointAccount = shoppingCart.GetRewardsAccount(PointAccounts.EnrolleeRewards);
                                discount.DiscountAmount = pointAccount.AmountRemaining;
                                break;
                            case DiscountType.HalfOffCredits:
                                pointAccount = shoppingCart.GetRewardsAccount(PointAccounts.Host12offcredits);
                                break;
                            //case DiscountType.BookingRewards:
                            //    pointAccount = shoppingCart.GetRewardsAccount(PointAccounts.BookingsRewardsCash);
                            //    discount.DiscountAmount = pointAccount.AmountRemaining;
                            //    break;
                            case DiscountType.HostSpecial:
                                // TODO: Look into why DI is broken on AJAX calls, seemingly.
                                var hostSpecialReward = new RewardService().GetHostSpecialReward(shoppingCart.EventId.Value);
                                // TODO: Need to validate that the discount exceeds the sales threshold in an IDiscountValidator
                                discount.DiscountAmount = hostSpecialReward.DiscountAmount;
                                break;
                            case DiscountType.Unknown:
                                break;
                            case DiscountType.Fixed:
                                break;
                            case DiscountType.Percent:
                                break;
                            case DiscountType.EBRewards:
                                break;
                            case DiscountType.SAHalfOff:
                                break;
                            case DiscountType.SAHalfOffOngoing:
                                break;
                            case DiscountType.NewProductsLaunchReward:
                                break;
                            case DiscountType.RetailPromoFixed:
                                break;
                            case DiscountType.RetailPromoPercent:
                                break;
                            case DiscountType.PromoCode:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        if ((pointAccount == null && discount.DiscountAmount > 0M) || (pointAccount != null && pointAccount.AmountRemaining > 0M))
                        {
                            p.ApplyDiscount(discount);
                        }
                        else
                        {
                            // TODO: Need implement error handling at the client for JSON response
                            //ModelState.AddModelError("RewardsError", "The applied rewards amount exceeds the current balance.");
                        }
                    }
                }

                return new JsonNetResult(new
                {
                    status = 200, cart = shoppingCart
                });
            }
            catch (Exception)
            {
                return new JsonNetResult(new
                {
                    status = 500, cart = shoppingCart
                });
            }
        }

        /// <summary>
        /// Removes the product's quantity from the cart.
        /// </summary>
        /// <param name="product">The product to reduce/remove.</param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult RemoveFromCart(Product product)
        {
            var shoppingCart = GetShoppingCart();
            try
            {
                // TODO: Need reference to the Event
                //var @event = new Event();
                //// For each discount applied to this product...
                //product.Discounts.ForEach(
                //    discount =>
                //    {
                //        // Remove the discount from the product.
                //        product.RemoveDiscount(discount, () =>
                //        {
                //            // Refund the discount to the Event.
                //            // TODO: @event.AddDiscount(discount);
                //        });
                //    });
                // Remove the product from the cart.
                product.RemoveFromOrder(shoppingCart, true);
               
                return new JsonNetResult(new
                {
                    status = 200, 
                    cart = shoppingCart
                });
            }
            catch (Exception)
            {
                return new JsonNetResult(new
                {
                    status = 500, cart = shoppingCart
                });
            }
        }

        [HttpPost]
        public virtual ActionResult UpdateQuantity(Product product)
        {
            var shoppingCart = GetShoppingCart();
            try
            {
                // If quantity is zero, remove the product.
                if (product.Quantity == 0)
                {
                    product.RemoveFromOrder(shoppingCart);
                }
                // Find the existing product and update the quantity.
                if (product.Discounts.Count() == 0)
                {
                    var existingProduct = (from p in shoppingCart.Products where p.ItemCode == product.ItemCode && p.CategoryId == product.CategoryId && p.ApplyDiscountType == product.ApplyDiscountType select p).FirstOrDefault();
                    if (null != existingProduct && product.Quantity > 0)
                    {
                        existingProduct.Quantity = product.Quantity;
                    }
                }
                else
                {
                    var existingProduct = (from p in shoppingCart.Products where p.ItemCode == product.ItemCode && p.CategoryId == product.CategoryId && p.ApplyDiscountType == product.ApplyDiscountType select p).FirstOrDefault();
                    if (null != existingProduct && product.Quantity > 0)
                    {
                        existingProduct.Quantity = 1;
                    }
                }
                return new JsonNetResult(new
                {
                    status = 200, cart = shoppingCart
                });
            }
            catch (Exception)
            {
                return new JsonNetResult(new
                {
                    status = 500, cart = shoppingCart
                });
            }
        }

        /// <summary>
        /// Returns the shopping cart.
        /// </summary>
        /// <remarks>
        /// Any classes that extend this controller must override
        /// this method. The storage of the shopping cart
        /// is independent of this class.
        /// </remarks>
        /// <returns>An <see cref="IOrder"/> object.</returns>
        protected abstract IOrder GetShoppingCart();
    }
}