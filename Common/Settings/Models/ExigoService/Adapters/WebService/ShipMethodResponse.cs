using ExigoService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Common.Api.ExigoWebService
{
    public partial class ShipMethodResponse : IShipMethod
    {
        public ShipMethodResponse() { }

        public static explicit operator ExigoService.ShipMethod(ShipMethodResponse shipmethod)
        {
            var model = new ExigoService.ShipMethod();
            if (shipmethod == null) return model;

            model.ShipMethodID          = shipmethod.ShipMethodID;
            model.ShipMethodDescription = shipmethod.Description;
            model.Price                 = shipmethod.ShippingAmount;

            return model;
        }

        public string ShipMethodDescription { get; set; }
        public decimal Price { get; set; }
        public bool Selected { get; set; }
    }
}