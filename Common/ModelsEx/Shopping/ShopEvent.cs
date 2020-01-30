
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.ModelsEx.Shopping
{
    public class ShopEvent : ShoppingExperience
    {
        public ShopEvent() { }

        public int CustomerEventId { get; set; }
    }
}