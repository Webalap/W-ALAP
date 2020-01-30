using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Models.ExigoService.CountryRegions
{
    public class TimeZone
    {
        public int TimeZoneID { get; set; }
        public string TimeZoneName { get; set; }
        public string TimeZoneDisplayName { get; set; }
        public string CountryCode { get; set; }
    }
}