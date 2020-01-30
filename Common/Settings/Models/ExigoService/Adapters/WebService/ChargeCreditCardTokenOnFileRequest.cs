using ExigoService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Common.Api.ExigoWebService
{
    public partial class ChargeCreditCardTokenOnFileRequest
    {
        public static explicit operator ChargeCreditCardTokenOnFileRequest(ExigoService.CreditCard card)
        {
            var model = new ChargeCreditCardTokenOnFileRequest();
            if (card == null) return model;

            model.CreditCardAccountType = card.Type.ToAccountCreditCardType();
            model.CvcCode               = card.CVV;

            return model;
        }
    }

    public partial class ChargeCreditCardTokenOnFileRequest
    {
        public ChargeCreditCardTokenOnFileRequest() { }
        public ChargeCreditCardTokenOnFileRequest(CreditCard card)
        {
            CreditCardAccountType = card.Type.ToAccountCreditCardType();
            CvcCode               = card.CVV;
        }
    }
}