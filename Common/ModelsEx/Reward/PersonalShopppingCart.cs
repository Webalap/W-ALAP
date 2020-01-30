using ExigoService;
using System;

namespace Common.ModelsEx.Reward
{
    [Serializable]
    public class PersonalShopppingCart : BasePropertyBag
    {
        public ShoppingCartCheckoutPropertyBag ShoppingCartCheckoutPropertyBag { get; set; }
        public ShoppingCartItemsPropertyBag ShoppingCartItemsPropertyBag { get; set; }
        public PersonalShopppingCart()
        {
            ShoppingCartItemsPropertyBag = new ShoppingCartItemsPropertyBag();
            ShoppingCartCheckoutPropertyBag = new ShoppingCartCheckoutPropertyBag();
        }
    }
}