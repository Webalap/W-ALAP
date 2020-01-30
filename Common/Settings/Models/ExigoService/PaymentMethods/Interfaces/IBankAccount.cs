using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface IBankAccount : IPaymentMethod, IAutoOrderPaymentMethod
    {
        BankAccountType Type { get; set; }

        string NameOnAccount { get; set; }
        string BankName { get; set; }
        string AccountNumber { get; set; }
        string RoutingNumber { get; set; }
        Address BillingAddress { get; set; }
        int[] AutoOrderIDs { get; set; }
    }

    public enum BankAccountType
    {
        New = 0,
        Primary = 1
    }
}