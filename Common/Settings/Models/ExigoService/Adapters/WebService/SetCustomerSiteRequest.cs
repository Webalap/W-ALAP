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
    public partial class SetCustomerSiteRequest
    {
        public SetCustomerSiteRequest() { }
        public SetCustomerSiteRequest(CustomerSite request)
        {
            CustomerID   = request.CustomerID;
            WebAlias     = request.WebAlias;
            FirstName    = request.FirstName;
            LastName     = request.LastName;
            Company      = request.Company;

            Email        = request.Email;
            Phone        = request.PrimaryPhone;
            Phone2       = request.SecondaryPhone;
            Fax          = request.Fax;

            if (request.Address != null)
            {
                Address1 = request.Address.Address1;
                Address2 = request.Address.Address2;
                City     = request.Address.City;
                State    = request.Address.State;
                Zip      = request.Address.Zip;
                Country  = request.Address.Country;
            }

            Notes1       = request.Notes1;
            Notes2       = request.Notes2;
            Notes3       = request.Notes3;
            Notes4       = request.Notes4;
        }
    }
}