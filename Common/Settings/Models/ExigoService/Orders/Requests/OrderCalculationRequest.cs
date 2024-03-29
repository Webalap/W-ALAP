﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class OrderCalculationRequest
    {
        public IOrderConfiguration Configuration { get; set; }
        public IEnumerable<IShoppingCartItem> Items { get; set; }
        public IAddress Address { get; set; }
        public int ShipMethodID { get; set; }
        public bool ReturnShipMethods { get; set; }
    }
}