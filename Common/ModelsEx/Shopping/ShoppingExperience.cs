using ExigoService;
using System.Collections.Generic;

namespace Common.ModelsEx.Shopping
{
    /// <summary>
    /// The idea behind this class was a way to represent a full shopping experience
    /// that could be shared across any website (we're looking at you, Replicated Site &amp; Backoffice)
    /// </summary>
    public abstract class ShoppingExperience
    {
        public IOrder Order { get; set; }

        public Customer Customer { get; set; }
        public CreditCard ShippingCard { get; set; }
        public CreditCard CreditCard { get; set; }
        public List<CreditCard> CreditCardList { get; set; }
        public int ShipMethodID { get; set; }
        public bool IsInternationalShipping { get; set; }
        public decimal InternationalShipping { get; set; }
        public decimal InternationalShippingTax { get; set; }  

    }
}