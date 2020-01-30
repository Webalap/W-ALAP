using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface ICustomerType
    {
        int CustomerTypeID { get; set; }
        string CustomerTypeDescription { get; set; }
    }
}