using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class GetCustomerRealTimeCommissionsRequest
    {
        public int CustomerID { get; set; }
        public int[] VolumeIDs { get; set; }
    }
}