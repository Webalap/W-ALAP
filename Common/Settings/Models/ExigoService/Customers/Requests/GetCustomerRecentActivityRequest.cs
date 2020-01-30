using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class GetCustomerRecentActivityRequest : DataRequest
    {
        public GetCustomerRecentActivityRequest()
            : base()
        {
        }

        public int CustomerID { get; set; }
        public DateTime? StartDate { get; set; }
    }
}