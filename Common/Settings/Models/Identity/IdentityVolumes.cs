using ExigoService;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Common.Models
{
    public class IdentityVolumes : IIdentityCacheable
    {
        public void Initialize(int customerID)
        {
            var weeklyVolumes = Exigo.GetCustomerVolumes(new GetCustomerVolumesRequest
            {
                CustomerID = customerID,
                PeriodTypeID = PeriodTypes.Weekly
            });

            this.WeeklyVolumes = weeklyVolumes;
        }

        public string CacheKey { get; set; }
        public void RefreshCache()
        {
            HttpContext.Current.Cache.Remove(this.CacheKey);
        }

        public VolumeCollection WeeklyVolumes { get; set; }
    }
}