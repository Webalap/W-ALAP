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
    public partial class DebitBankAccountOnFileRequest
    {
        public DebitBankAccountOnFileRequest() { }
        public DebitBankAccountOnFileRequest(BankAccount account)
        {
        }

        public static explicit operator DebitBankAccountOnFileRequest(ExigoService.BankAccount account)
        {
            var model = new DebitBankAccountOnFileRequest();
            if (account == null) return model;

            return model;
        }
    }
}