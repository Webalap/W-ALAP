using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping.Discounts;
using Common.ServicesEx;
using Common.ServicesEx.Rewards;
using ExigoService;
using Newtonsoft.Json;
using Ninject;
using System.Collections.Generic;
using System.Linq;

namespace Common.ModelsEx.Shopping
{
    public class ShoppingCart : IOrder
    {
        #region Dependencies

        [Inject]
        [JsonIgnore]
        public IOrderConfiguration Configuration { get; set; }

        // TODO: gwb20141130 - Need to refactor.  Just adding this for now for subscription configuration.  
        // Brian to refactor if/when auto orders are required.
        [Inject]
        [JsonIgnore]
        public IOrderConfiguration AutoOrderConfiguration { get; set; }

        [Inject]
        [JsonIgnore]
        public IOrderService OrderService { get; set; }

        [Inject]
        [JsonIgnore]
        public DiscountFactory DiscountFactory { get; set; }

        [Inject]
        [JsonIgnore]
        public IRewardService RewardService { get; set; }

        #endregion

        #region Constructors

        public ShoppingCart()
        {
            Products = new List<Product>();
        }

        #endregion

        #region IOrder implementation

        public int? EventId { get; set; }

        public bool ShopperIsHost { get; set; }

        public bool ShopperIsEventSa { get; set; }
        // here set Charity Order True/False
        public bool IsCharitable { get; set; }
        public bool IsInternationalShipping { get; set; }
        public string ShipMethodDescription { get; set; }

        //public bool ShopperIsBookingRewardsOwner { get; set; }

        public IList<IIndividualReward> ActiveAwards { get; set; }

        public RewardsAccount[] RewardsAccounts { get; set; }

        // TODO: Any rewards that are not stored by Exigo in a Point Account should
        // be refactored into an array of a common interface / abstract class.
        // Same comment left in the IOrder interface.
        public HostSpecialDiscount HostSpecialReward { get; set; }

        //public BookingReward[] BookingRewards { get; set; }

        public RewardsAccount GetRewardsAccount(int pointAccountId)
        {
            return (from pa in RewardsAccounts
                    where pa.PointAccountID == pointAccountId
                    select pa).FirstOrDefault();
        }

        public List<Discount> TotalDiscounts
        {
            get
            {
                return SummarizeDiscounts();
            }
        }

        public bool DiscountsApplied
        {
            get
            {
                return Products.Any(product => 0 != product.Discounts.Count);
            }
        }

        public decimal Subtotal
        {
            get { return _subTotal; }
        }

        public decimal DiscountedSubtotal
        {
            get { return _discountedSubTotal; }
        }

        public decimal Total
        {
            get { return _total; }
        }
        //start InternationalShipping properties
        public string carrier { get; set; }
        public string shippingMethod { get; set; }
        public string targetCurrencyCode { get; set; }
        public decimal shippingAmount { get; set; }
        //end      InternationalShipping properties
        public void UpdateTotals()
        {
            if (IsInternationalShipping)
            {
                InternationalShippingRequestModel Shipping = new InternationalShippingRequestModel();
                Shipping.carrier = carrier;
                Shipping.shippingMethod = shippingMethod;
                Shipping.shippingCostTotal = shippingAmount.ToString();
                Shipping.targetCurrencyCode = targetCurrencyCode;
                Shipping.discountTotal = SummarizeDiscounts().Sum(d => d.AppliedAmount).ToString();
                Shipping.additionalInsuranceTotal = "0.00"; 
                OrderService.InternationalShippingCalculateTotals(Shipping, this, out _subTotal, out _shipping, out _tax, out _total);
            } 
            else if (0 != ShippingMethodId &&
                 null != ShippingAddress &&
                 !string.IsNullOrWhiteSpace(ShippingAddress.State) &&
                 !string.IsNullOrWhiteSpace(ShippingAddress.Country))
            {
                OrderService.CalculateTotals(this, out _subTotal, out _shipping, out _tax, out _total);
            }
            else
            {
                CalculateTotals();
            }

            if //(this.EventId.HasValue &&
                 (ShopperIsHost || ShopperIsEventSa) // || ShopperIsBookingRewardsOwner) )
            {
                // If discounts have been applied to any of the products,
                // then need to re-calc the subtotal.
                if (DiscountsApplied)
                {
                    //CalculateSubTotal(); show update total according to discount price due to on Qa request. 
                }

                // Calculate point accounts balance based on discounts applied to products
                RewardsAccounts = OrderService.CalculateHostPointAccounts(this);
            }

            if (ActiveAwards == null || ActiveAwards.Count() == 0) return;
            var rewardAccounts = ActiveAwards.Select(activeAward => activeAward.CalculatePointsAccount(Products)).Where(rewardAccount => rewardAccount != null).ToList();

            // This was wiping out the original disounts for cash rewards and half-off credits.
            // Adding condition to only replace RewardsAccounts if the locally declared list
            // has a count greater than zero.
            // TODO: CHRIS or ADAM - Need to assess the impact for existing rewards/discounts.
            if (EventId.HasValue && !ShopperIsEventSa) return; //do not add non GT awards if we are in a GT for host (if Host and SA are same, show rewards)
            if (rewardAccounts.Any() && !EventId.HasValue)
            {
                RewardsAccounts = rewardAccounts.ToArray();
            }
            else
            {
                if (RewardsAccounts == null) return;
                var rewardsAccounts = RewardsAccounts.ToList();

                foreach (var ra in rewardAccounts.Where(ra => rewardsAccounts.FirstOrDefault(c => c.PointAccountID == ra.PointAccountID) == null))
                {
                    rewardsAccounts.Add(ra);
                }
                RewardsAccounts = rewardsAccounts.ToArray();
            }

        }

        public void CalculateTotals()
        {
            CalculateSubTotal();
            CalculateTotal();
        }
        public void CalculateTotals(OrderCalculationResponse responce)
        {
            _subTotal = responce.Subtotal;
            _total = responce.Total;
            _shipping = responce.Shipping;
            _tax = responce.Tax;
        }
        public void CalculateSubTotal()
        {
            _subTotal = 0;
            foreach (var product in Products)
            {
                _subTotal += product.Quantity * product.Price;
                _discountedSubTotal = _subTotal - SummarizeDiscounts().Sum(d => d.AppliedAmount);
            }
        }

        public void CalculateTotal()
        {
            _shipping = 0M;
            _tax = 0M;
            _total = Products.Sum(p => p.Quantity * p.Price);
            _total -= SummarizeDiscounts().Sum(d => d.AppliedAmount);
        }

        public List<Product> Products { get; set; }

        #endregion

        #region ITaxable implementation

        public ShippingAddress ShippingAddress { get; set; }

        public decimal Tax
        {
            get { return _tax; }
        }

        #endregion

        #region IShippable implementation

        public int ShippingMethodId { get; set; }

        public decimal Shipping
        {
            get { return _shipping; }
        }

        #endregion

        #region Discount Methods

        // TODO: NewHostRewards - gwb:20150705 - Need to discuss refatoring this method to improve effiency
        protected virtual List<Discount> SummarizeDiscounts()
        {
            var credits = new DiscountFactory().CreateDiscount(
                DiscountType.HalfOffCredits, 0M);

            var cash = new DiscountFactory().CreateDiscount(
                DiscountType.RewardsCash, 0M);

            //var bookingRewardsCash = new DiscountFactory().CreateDiscount(
            //    DiscountType.BookingRewards, 0M);

            var hostSpecialReward = new DiscountFactory().CreateDiscount(
                DiscountType.HostSpecial, 0M);

            var ebReward = new DiscountFactory().CreateDiscount(
                DiscountType.EBRewards, 0M);

            var saHalfOff = new DiscountFactory().CreateDiscount(DiscountType.SAHalfOff, 0M);

            var saHalfOffOngoing = new DiscountFactory().CreateDiscount(DiscountType.SAHalfOffOngoing, 0M);

            var newProductsLaunchReward = new DiscountFactory().CreateDiscount(DiscountType.NewProductsLaunchReward, 0M);

            var recruitingCash = new DiscountFactory().CreateDiscount(
                DiscountType.RecruitingReward, 0M);

            var enrolleeCash = new DiscountFactory().CreateDiscount(
                DiscountType.EnrolleeReward, 0M);

            var retailPercent = new DiscountFactory().CreateDiscount(
                DiscountType.RetailPromoPercent, 0M, 0M, 0M);

            var retailAmount = new DiscountFactory().CreateDiscount(
                DiscountType.RetailPromoFixed, 0M, 0M, 0M);

            var promoCodePercent = new DiscountFactory().CreateDiscount(
                DiscountType.PromoCode, 0M, null, 0M, 0M);

            foreach (var p in Products)
            {
                credits.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.HalfOffCredits) ? d.AppliedAmount : 0);
                cash.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RewardsCash) ? d.AppliedAmount : 0);
                //bookingRewardsCash.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.BookingRewards) ? d.AppliedAmount : 0);
                hostSpecialReward.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.HostSpecial) ? d.AppliedAmount : 0);
                ebReward.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.EBRewards) ? d.AppliedAmount : 0);
                saHalfOff.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.SAHalfOff) ? d.AppliedAmount : 0);
                saHalfOffOngoing.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.SAHalfOffOngoing) ? d.AppliedAmount : 0);
                newProductsLaunchReward.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.NewProductsLaunchReward) ? d.AppliedAmount : 0);
                recruitingCash.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RecruitingReward) ? d.AppliedAmount : 0);
                enrolleeCash.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.EnrolleeReward) ? d.AppliedAmount : 0);
                retailPercent.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RetailPromoPercent) ? d.AppliedAmount * p.Quantity : 0);
                retailPercent.OverrideBVAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RetailPromoPercent) ? d.OverrideBVAmount : 0);
                retailPercent.OverrideCVAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RetailPromoPercent) ? d.OverrideCVAmount : 0);
                retailAmount.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RetailPromoFixed) ? d.AppliedAmount * p.Quantity : 0);
                retailAmount.DiscountAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RetailPromoFixed) ? d.AppliedAmount : 0);
                retailAmount.OverrideBVAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RetailPromoFixed) ? d.OverrideBVAmount : 0);
                retailAmount.OverrideCVAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RetailPromoFixed) ? d.OverrideCVAmount : 0);
                promoCodePercent.AppliedAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.PromoCode) ? d.AppliedAmount * p.Quantity : 0);
                promoCodePercent.OverrideBVAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.PromoCode) ? d.OverrideBVAmount : 0);
                promoCodePercent.OverrideCVAmount += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.PromoCode) ? d.OverrideCVAmount : 0);
                _countsPercentage += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RetailPromoPercent) ? 1 : 0);
                _countsPromoCodePercentage += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.PromoCode) ? 1 : 0);
                _sumPercentage += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.RetailPromoPercent) ? d.DiscountAmount : 0);
                _sumPromoCodePercentage += p.Discounts.Sum(d => d.DiscountType.Equals(DiscountType.PromoCode) ? d.DiscountAmount : 0);
            }
            var discounts = new List<Discount>();
            if (0 != cash.AppliedAmount)
            {
                discounts.Add(cash);
            }
            if (0 != credits.AppliedAmount)
            {
                discounts.Add(credits);
            }
            //if (0 != bookingRewardsCash.AppliedAmount)
            //{
            //    discounts.Add(bookingRewardsCash);
            //}
            if (0 != hostSpecialReward.AppliedAmount)
            {
                discounts.Add(hostSpecialReward);
            }
            if (0 != ebReward.AppliedAmount)
            {
                discounts.Add(ebReward);
            }
            if (0 != saHalfOff.AppliedAmount)
            {
                discounts.Add(saHalfOff);
            }
            if (0 != saHalfOffOngoing.AppliedAmount)
            {
                discounts.Add(saHalfOffOngoing);
            }
            if (0 != newProductsLaunchReward.AppliedAmount)
            {
                discounts.Add(newProductsLaunchReward);
            }
            if (0 != recruitingCash.AppliedAmount)
            {
                discounts.Add(recruitingCash);
            }
            if (0 != enrolleeCash.AppliedAmount)
            {
                discounts.Add(enrolleeCash);
            }
            if (0 != retailPercent.AppliedAmount)
            {
                retailPercent.DiscountAmount = _sumPercentage / _countsPercentage;
                discounts.Add(retailPercent);
            }
            if (0 != retailAmount.AppliedAmount)
            {
                discounts.Add(retailAmount);
            }
            if (0 != promoCodePercent.AppliedAmount)
            {
                promoCodePercent.DiscountAmount = _sumPromoCodePercentage / _countsPromoCodePercentage;
                discounts.Add(promoCodePercent);
            }


            return discounts;
        }

        #endregion

        #region Private Fields

        private decimal _subTotal, _discountedSubTotal, _shipping, _tax, _total, _countsPercentage, _sumPercentage, _countsPromoCodePercentage, _sumPromoCodePercentage;
        private decimal? _eventSalesTotal;

        #endregion
    }
}