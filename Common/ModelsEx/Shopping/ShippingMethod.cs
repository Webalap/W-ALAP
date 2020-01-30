using ExigoService;
using System.Linq;

namespace Common.ModelsEx.Shopping
{
    public class ShippingMethods
    {
        public int ShippingMethodID { get; set; }

        public int UpgradeShipping(bool Upgrade,bool Expedited) 
        {
            int IdToReturn=0;
          
            if (Upgrade)
            {
                using (var context = Exigo.Sql())
                {
                    string sqlProcedure = string.Format("UpgradeShipping2dayExpedited");
                    IdToReturn = context.Query<int>(sqlProcedure).ToList().FirstOrDefault();
                }
            }
            else if (Expedited)
            {
                using (var context = Exigo.Sql())
                {
                    string sqlProcedure = string.Format("UpgradeShippingExpedited");
                    IdToReturn = context.Query<int>(sqlProcedure).ToList().FirstOrDefault();
                }
            }
            if(IdToReturn==0|| (!Upgrade && !Expedited) )
            {
                using (var context = Exigo.Sql())
                {
                    string sqlProcedure = string.Format("UpgradeShippingStandard");
                    IdToReturn = context.Query<int>(sqlProcedure).ToList().FirstOrDefault();
                }
            }
            return IdToReturn;
            
        }
        public int ShippingForAlaskaAndHawai(string state, int defaultValue) 
        {
            var IdToReturn = defaultValue;
            //if (string.Compare(state, "AK", StringComparison.Ordinal) == 0)
            //{
            //    using (var context = Exigo.Sql())
            //    {
            //        var sql = @"select ShipMethodID from ShipMethods where ShipmethodDescription='Alaska'";
            //        IdToReturn = context.Query<int>(sql).ToList().FirstOrDefault();
            //    }
            //}
            //if (string.Compare(state, "HI", StringComparison.Ordinal) == 0)
            //{
            //    using (var context = Exigo.Sql())
            //    {
            //        var sql = @"select ShipMethodID from ShipMethods where ShipmethodDescription='Hawaii'";
            //        IdToReturn = context.Query<int>(sql).ToList().FirstOrDefault();
            //    }
            //}
            //if (IdToReturn==0)
            //{
            //    IdToReturn = defaultValue;
            //}
            return IdToReturn;
        }
       
    }

    public enum ShipMethods
    {
        
        Standard = 2,
        Expedited3Day = 3,
        TwoDayExpedited = 4,
        Overnight = 5,
        StarterKitShipping = 7,
        WillCall = 9,
        VirtualItemRecurringOrderZeroDollar = 10,
        Expedited = 13 ,
        Canada = 14
    }
    public static class InternationalShipping {

        public static readonly string[] InternationalShipMethods = { "ANY", "AIR", "GROUND", "2DAY" };
        public static readonly string[] InternationalShipCarriers = { "ANY", "UPS", "FEDEX", "DHL" };
    }

}