
using Common.ModelsEx.Event;
using Common.ModelsEx.Shopping;
using ExigoService;
using System.Collections.Generic;
using IOrder = Common.ModelsEx.Shopping.IOrder;

namespace Common.ServicesEx
{

    /// <summary>
    /// The interface for interacting with orders.
    /// </summary>
    public interface IOrderService {

        /// <summary>
        /// Calculates the order totals for shipping, tax, and totals.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="subTotal">Output of sub total</param>
        /// <param name="shipping">Output of shipping.</param>
        /// <param name="tax">Output of tax.</param>
        /// <param name="total">Output of total.</param>
        void CalculateTotals(IOrder order, out decimal subTotal, out decimal shipping, out decimal tax, out decimal total);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        OrderConfirmation PlaceOrder(Shop shop, string giftMessage,int  customerID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollmentOrder"></param>
        OrderConfirmation PlaceEnrollmentOrder(Enrollment enrollmentOrder,string giftMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopEvent"></param>
        /// <returns></returns>
        OrderConfirmation PlaceEventOrder(ShopEvent shopEvent);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        RewardsAccount[] CalculateHostPointAccounts(IOrder order);
        void InternationalShippingCalculateTotals(InternationalShippingRequestModel Shipping, IOrder order, out decimal subTotal, out decimal shipping, out decimal tax, out decimal total);
        InternationalShippingResponseModel InternationalShippingCalculateTotals(decimal ShippingTotal,ShippingAddress ShippingAddress, IEnumerable<IShoppingCartItem> Items);
    }
}