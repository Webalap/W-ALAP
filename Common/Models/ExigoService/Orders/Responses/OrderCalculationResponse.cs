using System.Collections.Generic;

namespace ExigoService
{
    public class OrderCalculationResponse
    {
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        public IEnumerable<IShipMethod> ShipMethods { get; set; }

        public OrderCalculationResponse()
        {

        }

        public OrderCalculationResponse(decimal _sub, decimal _tax, decimal _discount, decimal _total, decimal _shipping, IEnumerable<IShipMethod> _shippingMethods)
        {
            this.Subtotal = _sub;
            this.Tax = _tax;
            this.Discount = _discount;
            this.Total = _total;
            this.Shipping = _shipping;
            this.ShipMethods = _shippingMethods;
        }
        public static OrderCalculationResponse operator +(OrderCalculationResponse OC1, OrderCalculationResponse OC2)
        {
            if (OC1 == null ) return OC2;
            return new OrderCalculationResponse(OC1.Subtotal + OC2.Subtotal,
                OC1.Tax + OC2.Tax, 
                OC1.Discount+OC2.Discount ,
                OC1.Total+OC2.Total,OC1.Shipping,OC1.ShipMethods);
        }
    }
}