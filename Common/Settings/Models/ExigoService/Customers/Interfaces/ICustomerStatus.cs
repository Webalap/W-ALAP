using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface ICustomerStatus
    {
        int CustomerStatusID { get; set; }
        string CustomerStatusDescription { get; set; }
    }
}