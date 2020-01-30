using ExigoService;
using System.Collections.Generic;

namespace Common.ModelsEx.SavedCart
{
    public class CommonSavedCartModel
    {
        public Address BillingAddress { get; set; }
        public string BillingCVV { get; set; }
        public string BillingCardNumber { get; set; }
        public CreditCardType BillingCardType { get; set; }
        public int BillingExpirationMonth { get; set; }
        public int BillingExpirationYear { get; set; }
        public string BillingNameOnCard { get; set; }
        public TypeOfCreditCard BillingTypeOfCardCredit { get; set; }
        public string Company { get; set; }
        public int CartId { get; set; }
        public int CustomerID { get; set; }
        public IOrderConfiguration OrderConfiguration { get; set; }
        public string Email { get; set; }
        public int EnrollerID { get; set; }
        public int EventID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GiftMessage { get; set; }
        public bool IsCharity { get; set; }

        public string Other16 { get; set; }
        public string CartType { get; set; }
        public string PrimaryPhone { get; set; }
        public List<CartProducts> Products { get; set; }
        public Address ShippingAddress { get; set; }
        public int ShippingMethodId { get; set; }
        public bool ShopperIsBookingRewardsOwner { get; set; }
        public bool ShopperIsEventSa { get; set; }
        public bool ShopperIsHost { get; set; }
        public int SponsorID { get; set; }

        public CommonSavedCartModel()
        {
            BillingAddress = new Address();
            ShippingAddress = new Address();
            OrderConfiguration = new UnitedStatesConfiguration.OrderConfiguration();
        }
    }
    public class CartProducts
    {
        public string ApplyDiscountsType { get; set; }
        public decimal? BusinessVolume { get; set; }
        public int CategoryID { get; set; }
        public decimal? CommissionableVolume { get; set; }
        public string Description { get; set; }
        public decimal DiscountedPrice { get; set; }
        public bool IsLimited { get; set; }
        public string ItemCode { get; set; }
        public string LargePicture { get; set; }
        public int MaxQuantity { get; set; }
        public decimal Price { get; set; }
        public int PriceTypeID { get; set; }
        public decimal Quantity { get; set; }
        public string ShareUrlDetail { get; set; }
        public string SmallPicture { get; set; }
        public string TinyPicture { get; set; }
        public string ShortDetail { get; set; }
    }
}