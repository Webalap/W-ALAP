using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExigoService
{
    [ModelBinder(typeof(IPaymentMethodModelBinder))]
    public interface IPaymentMethod
    {
        bool IsComplete { get; }
        bool IsValid { get; }
    }
}