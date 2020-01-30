using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public enum ShoppingCartItemType
    {
        Order    = 0,
        AutoOrder = 1,
        WishList = 2,
        Coupon   = 3
    }
}