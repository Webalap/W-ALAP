using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping.Discounts;
using Common.ServicesEx.Rewards;
using ExigoService;
using System.Collections.Generic;

namespace Common.ModelsEx.Shopping
{
    public interface IOrder : ITaxable, IShippable
    {
        List<Product> Products { get; set; }

        IList<IIndividualReward> ActiveAwards { get; set; }

        int? EventId { get; set; }

        bool ShopperIsHost { get; set; }

        bool ShopperIsEventSa { get; set; }

        string ShipMethodDescription { get; set; }

        //bool ShopperIsBookingRewardsOwner { get; set; }

        // TODO: Any rewards that are not stored by Exigo in a Point Account should
        // be refactored into an array of a common interface / abstract class.
        HostSpecialDiscount HostSpecialReward { get; set; }

        //BookingReward[] BookingRewards { get; set; }

        RewardsAccount[] RewardsAccounts { get; set; }

        RewardsAccount GetRewardsAccount(int pointAccountId);

        bool DiscountsApplied { get; }

        IOrderConfiguration Configuration { get; set; }

        // TODO: gwb20141130 - Need to refactor.  Just adding this for now for subscription configuration.  
        // Brian to refactor if/when auto orders are required.
        IOrderConfiguration AutoOrderConfiguration { get; set; }

        decimal Subtotal { get; }

        decimal Total { get; }

        void UpdateTotals();

        void CalculateSubTotal();
        //start InternationalShipping properties
        string carrier { get; set; }
        string shippingMethod { get; set; }
        decimal shippingAmount { get; set; }   
        bool IsInternationalShipping { get; set; }
        string targetCurrencyCode { get; set; }

        //end      InternationalShipping properties

        void CalculateTotals(OrderCalculationResponse response);
    }
}