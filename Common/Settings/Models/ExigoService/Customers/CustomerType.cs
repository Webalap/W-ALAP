using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class CustomerType : ICustomerType
    {
        public int CustomerTypeID { get; set; }
        public string CustomerTypeDescription { get; set; }
    }
}