using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class CustomerStatus : ICustomerStatus
    {
        public int CustomerStatusID { get; set; }
        public string CustomerStatusDescription { get; set; }
    }
}