using Common.Api.ExigoWebService;
using ExigoService;
using System;

namespace Common.ModelsEx.Reward
{
    [Serializable]
    public class ShoppingCartCheckoutPropertyBag : BasePropertyBag
    {
        private string version = "1.0.0";
        private int expires = 0;



        #region Constructors
        public ShoppingCartCheckoutPropertyBag()
        {
            //this.CustomerID = Identity.Current.CustomerID;
            this.Expires = expires;
        }
        #endregion

        #region Properties
        public int CustomerID { get; set; }

        public ShippingAddress ShippingAddress { get; set; }

        public DateTime AutoOrderStartDate { get; set; }
        public FrequencyType AutoOrderFrequencyType { get; set; }

        public int ShipMethodID { get; set; }

        public CreditCard PaymentMethod { get; set; }
        public string GifftMessage { get; set; }
        #endregion

        #region Methods
        public override T OnBeforeUpdate<T>(T propertyBag)
        {
            propertyBag.Version = version;

            return propertyBag;
        }
        public override bool IsValid()
        {
            //var currentCustomerID = Identity.Current.CustomerID;
            return this.Version == version;
        }
        #endregion
    }
}
