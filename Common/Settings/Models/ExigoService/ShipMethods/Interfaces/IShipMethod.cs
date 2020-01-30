using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface IShipMethod
    {
        int ShipMethodID { get; set; }
        string ShipMethodDescription { get; set; }
        decimal Price { get; set; }
        bool Selected { get; set; }
    }
}