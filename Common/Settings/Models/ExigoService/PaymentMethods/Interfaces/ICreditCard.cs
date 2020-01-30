using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface ICreditCard : IPaymentMethod, IAutoOrderPaymentMethod
    {
        CreditCardType Type { get; set; }
        string NameOnCard { get; set; }
        string CardNumber { get; set; }
        int ExpirationMonth { get; set; }
        int ExpirationYear { get; set; }
        string CVV { get; set; }

        Address BillingAddress { get; set; }

        string GetToken();
        DateTime ExpirationDate { get; }
        bool IsExpired { get; }
    }
}