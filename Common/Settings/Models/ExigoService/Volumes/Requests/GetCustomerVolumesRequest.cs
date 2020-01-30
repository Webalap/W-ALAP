using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class GetCustomerVolumesRequest
    {
        public GetCustomerVolumesRequest()
            : base()
        {
        }

        public int CustomerID { get; set; }
        public int PeriodTypeID { get; set; }
        public int? PeriodID { get; set; }
        public int[] VolumeIDs { get; set; }
    }
}