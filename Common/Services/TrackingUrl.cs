using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Services
{

    public class TrackingUrl
    {
             public static string  GetTrackingUrl(string trackingNumber)
        {

            


            List<ShipCarriers> Carriers = new List<ShipCarriers>();
            ShipCarriers ShipCarrierRecord = new ShipCarriers();
            try
            {
                using (var context = Exigo.Sql())
                {

                    var SqlProcedure = string.Format("GetShipCarriers");

                    Carriers = context.Query<ShipCarriers>(SqlProcedure).ToList();
                }
            }
            catch (Exception)
            {
                return "https://www.google.com.pk/search?q=" + trackingNumber;
            }  
            

            string url = "";

            if (trackingNumber.Length == 22)
            {
                ShipCarrierRecord = Carriers.FirstOrDefault(s => s.ShipCarrierID == 23);
                url = (ShipCarrierRecord.TrackingUrl + trackingNumber).ToString();
            }
         else   if (trackingNumber.Length == 20)
            {
                ShipCarrierRecord = Carriers.FirstOrDefault(s => s.ShipCarrierID == 2);
                url = (ShipCarrierRecord.TrackingUrl + trackingNumber).ToString();
            }
            else if (trackingNumber.Length == 18)
            {
                ShipCarrierRecord = Carriers.FirstOrDefault(s => s.ShipCarrierID == 3);
                url = (ShipCarrierRecord.TrackingUrl + trackingNumber).ToString();
            }
            else if (trackingNumber.Length == 10 || trackingNumber.Length == 35)
            {
                ShipCarrierRecord = Carriers.FirstOrDefault(s => s.ShipCarrierID == 20);
                url = (ShipCarrierRecord.TrackingUrl + trackingNumber).ToString();
            }
            else if (trackingNumber.Length == 12)
            {
                ShipCarrierRecord = Carriers.FirstOrDefault(s => s.ShipCarrierID == 2);
                url = (ShipCarrierRecord.TrackingUrl + trackingNumber).ToString();
            }
            // IF tracking number start with 61, 06, 63 than go to fedex
            else if (trackingNumber.Trim().StartsWith("63"))
            {
                ShipCarrierRecord = Carriers.FirstOrDefault(s => s.ShipCarrierID == 2);
                url = (ShipCarrierRecord.TrackingUrl + trackingNumber).ToString();
            }
            else if (trackingNumber.Trim().StartsWith("61"))
            {
                ShipCarrierRecord = Carriers.FirstOrDefault(s => s.ShipCarrierID == 2);
                url = (ShipCarrierRecord.TrackingUrl + trackingNumber).ToString();
            }
            else if (trackingNumber.Trim().StartsWith("06"))
            {
                ShipCarrierRecord = Carriers.FirstOrDefault(s => s.ShipCarrierID == 2);
                url = (ShipCarrierRecord.TrackingUrl + trackingNumber).ToString();
            }
            else
            {
                // IF NOT MATCH THAN GO TO GOOGLE SEARCH AND FIND PERTICULAR TRACKING NUMBER
                url = "https://www.google.com.pk/search?q=" + trackingNumber;
            }
            return url;
        }


    }
}