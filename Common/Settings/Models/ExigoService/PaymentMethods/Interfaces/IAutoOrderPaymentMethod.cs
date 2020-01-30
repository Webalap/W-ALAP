using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface IAutoOrderPaymentMethod
    {
        int[] AutoOrderIDs { get; set; }

        bool IsUsedInAutoOrders { get; }
        AutoOrderPaymentType AutoOrderPaymentType { get; }
    }
}