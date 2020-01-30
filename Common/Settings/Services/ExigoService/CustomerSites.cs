using Common;
using Common.Api.ExigoWebService;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static CustomerSite GetCustomerSite(int customerID)
        {
            try
            {
                var site = Exigo.OData().CustomerSites
                    .Where(c => c.CustomerID == customerID)
                    .FirstOrDefault();
                if (site == null) return null;

                return (CustomerSite)site;
            }
            catch (Exception exception)
            {
                if (exception.Message == "CustomerSite not found\n") return null;
                else throw exception;
            }
        }
        public static CustomerSite UpdateCustomerSite(CustomerSite request)
        {
            // There is no way to update the customer site with the web service - it only allows you to set all the fields.
            // Essentially, it's an all-or-none approach.
            // This method will let us update only the fields we want to update.

            // If the customer site passed is null, or we don't have the CustomerID set, stop here.
            if (request == null || request.CustomerID == 0) return request;


            // First, get the existing customer's site info
            var customerSite = GetCustomerSite(request.CustomerID);
            if (customerSite != null)
            {
                // Determine if the web alias has changed between the request and the existing data.
                // If it isn't available, set the requested web alias to null so we don't attempt to update it.
                if (request.WebAlias.IsNullOrEmpty())
                {
                    request.WebAlias = customerSite.WebAlias;
                }
                else if (request.WebAlias.ToUpper() != customerSite.WebAlias.ToUpper() && !IsWebAliasAvailable(request.CustomerID, request.WebAlias))
                {
                    request.WebAlias = null;
                }

                // Reflect each property and populate it if the requested value is not null.
                // This does nto currently take INTs into account:
                // The properties in the CustomerSiteResponse object in the Exigo API are all strings as of this writing on 7/8/2014.
                var customerSiteType = customerSite.GetType();
                foreach (var property in customerSiteType.GetProperties())
                {
                    if (property.CanWrite && property.GetValue(request) != null)
                    {
                        property.SetValue(customerSite, property.GetValue(request));
                    }
                }
            }
            else
            {
                customerSite = request;
                if (customerSite.WebAlias.IsNullOrEmpty())
                {
                    customerSite.WebAlias = customerSite.CustomerID.ToString();
                }
                if (customerSite.WebAlias.IsNotNullOrEmpty() && !IsWebAliasAvailable(customerSite.CustomerID, customerSite.WebAlias))
                {
                    return customerSite;
                }
            }

            // Update the data
            SetCustomerSite(customerSite);

            // Return the modified request we used to update the data
            return customerSite;
        }
        public static CustomerSite SetCustomerSite(CustomerSite request)
        {
            Exigo.WebService().SetCustomerSite(new SetCustomerSiteRequest(request));
            return request;
        }

        public static bool IsWebAliasAvailable(int customerID, string webalias)
        {
            // Get the current webalias to see if it matches what we passed. If so, it's still valid.
            var currentWebAlias = Exigo.GetCustomerSite(customerID).WebAlias;
            if (webalias.Equals(currentWebAlias, StringComparison.InvariantCultureIgnoreCase)) return true;


            // Validate the web alias
            return Exigo.WebService().Validate(new IsLoginNameAvailableValidateRequest
            {
                LoginName = webalias
            }).IsValid;
        }
    }
}