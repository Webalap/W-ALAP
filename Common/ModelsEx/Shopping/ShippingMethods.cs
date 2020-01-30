using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.ModelsEx.Shopping
{
    public class ShippingMethods
    {
        public int ShippingMethodID { get; set; }
        public ShippingMethods(bool Upgrade)
        {
            if (Upgrade)
            {
                using (var context = Exigo.Sql())
                {
                    var sql = @"select ShipMethodID from ShipMethods where shipmethodDescription='2-day Expedited'";
                    ShippingMethodID = context.Query<int>(sql).ToList().FirstOrDefault();
                }
            }
            else
            {
                using (var context = Exigo.Sql())
                {
                    var sql = @"select ShipMethodID from ShipMethods where shipmethodDescription='Standard'";
                    ShippingMethodID = context.Query<int>(sql).ToList().FirstOrDefault();
                }
            }
        }
        public ShippingMethods(string state,int defaultValue)
        {
            int IdToReturn = 2;
            if (state.CompareTo("AK") == 0)
            {
                using (var context = Exigo.Sql())
                {
                    var sql = @"select ShipMethodID from ShipMethods where ShipmethodDescription='Alaska'";
                    IdToReturn = context.Query<int>(sql).ToList().FirstOrDefault();
                }
            }
            if (state.CompareTo("HI") == 0)
            {
                using (var context = Exigo.Sql())
                {
                    var sql = @"select ShipMethodID from ShipMethods where ShipmethodDescription='Hawaii'";
                    IdToReturn = context.Query<int>(sql).ToList().FirstOrDefault();
                }
            }
            ShippingMethodID= IdToReturn;
        }
    }
}