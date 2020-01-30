using ExigoService;
using System.Collections.Generic;

namespace Common.ModelsEx.Reward
{
    public class EventCustomerOrderPropertyBag : BasePropertyBag
    {
        private string version = "1.0.0";
        private int expires    = 0;

        public EventCustomerOrderPropertyBag()
        {
            Customer = Customer ?? new Customer
            {
                CustomerTypeID = CustomerTypes.PartyGuest,
                CustomerStatusID = CustomerStatuses.Active,
                //SponsorID = Identity.Current.CustomerID,
                //EnrollerID = Identity.Current.CustomerID
            };
            ShoppingCartItems = new ShoppingCartItemCollection();
        }

        #region Personal Information

        public Customer Customer { get; set; }
        public ShoppingCartItemCollection ShoppingCartItems { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public string SameAsAddress { get; set; }
        public string SameAsAddress2 { get; set; }
        public int ShippingMethodId { get; set; }
        public string ShippingMethodDescription { get; set; }
        public IEnumerable<ShipMethod> ShipMethods { get; set; }
        public CreditCard CreditCard { get; set; }
        public CreditCard CreditCard2 { get; set; }
        public CreditCard ShippingCard { get; set; }
        public Order Order { get; set; }
        public int EventId { get; set; }

        #endregion
    }
}