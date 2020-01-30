using ExigoService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Common.Api.ExigoWebService
{

    public partial class CreateOrderRequest {

        private readonly TraceSource _log = new TraceSource( "IhBackoffice", SourceLevels.All );


        public CreateOrderRequest() {

        }


        public CreateOrderRequest(int customerId, IOrderConfiguration configuration,string giftMessage, int shipMethodID, Common.ModelsEx.Shopping.IOrder order, decimal? ShippingOverride=null , decimal? TaxOverride=null) {

            CustomerID = customerId;
            WarehouseID = configuration.WarehouseID;
            PriceType = configuration.PriceTypeID;
            CurrencyCode = configuration.CurrencyCode;
            OrderDate = DateTime.Now;
            OrderType = OrderType.ShoppingCart;
            OrderStatus = OrderStatusType.Incomplete;
            ShipMethodID = shipMethodID;
            ShippingAmountOverride = ShippingOverride ?? ShippingOverride;
            TaxRateOverride = TaxOverride ?? TaxOverride;
            Details = order.Products.Select(c => new OrderDetailRequest(c)).ToArray();

            FirstName = order.ShippingAddress.FirstName;
            LastName = order.ShippingAddress.LastName;
            Email = order.ShippingAddress.Email;
            Phone = order.ShippingAddress.Phone;
            Address1 = order.ShippingAddress.Address1;
            Address2 = order.ShippingAddress.Address2;
            City = order.ShippingAddress.City;
            State = order.ShippingAddress.State;
            Zip = order.ShippingAddress.Zip;
            Country = order.ShippingAddress.Country;
            Other20 = giftMessage;
        }



        public CreateOrderRequest(IOrderConfiguration configuration, int shipMethodID, string giftMessage, IEnumerable<IShoppingCartItem> items, ShippingAddress address,decimal? shippingOverride=null , decimal? taxOverride = null) {

            WarehouseID             = configuration.WarehouseID;
            PriceType               = configuration.PriceTypeID;
            CurrencyCode            = configuration.CurrencyCode;
            OrderDate               = DateTime.Now;
            OrderType               = OrderType.ShoppingCart;
            OrderStatus             = OrderStatusType.Incomplete;
            ShipMethodID            = shipMethodID;
            ShippingAmountOverride  = shippingOverride;
            TaxRateOverride         = taxOverride;
            Details                 = items.Select(c => (OrderDetailRequest)(c as ShoppingCartItem)).ToArray();

            FirstName               = address.FirstName;
            LastName                = address.LastName;
            Email                   = address.Email;
            Phone                   = address.Phone;
            Address1                = address.Address1;
            Address2                = address.Address2;
            City                    = address.City;
            State                   = address.State;
            Zip                     = address.Zip;
            Country                 = address.Country;
            Other20                 = giftMessage;

        }

        
    }

}
