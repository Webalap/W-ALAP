using Common.Api.ExigoWebService;
using System;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static CustomerSite GetCustomerSite(int customerID)
        {
            try
            {
                //var site = Exigo.OData().CustomerSites
                //    .Where(c => c.CustomerID == customerID)
                //    .FirstOrDefault();
                CustomerSite site;
                using (var context= Sql())
                {
                    string sqlProcedure = string.Format("GetCustomerSite {0}", customerID);
                    site = context.Query<CustomerSite>(sqlProcedure).FirstOrDefault();
                    context.Close();
                }
                if (site == null) return new CustomerSite();

                return site;
            }
            catch (Exception exception)
            {
                if (exception.Message == "CustomerSite not found\n") return new CustomerSite();
                else throw exception;
            }
        }
        public static string GetCustomerWebAlias(int customerID)
        {
            string SiteAlias = string.Empty;
            using (var Context = Sql())
            {
                Context.Open();
                //Getting Web alias
                string sqlProcedure = string.Format("GetCustomerWebAlias {0}", customerID);
                SiteAlias = Context.Query<string>(sqlProcedure).FirstOrDefault().ToString();
                Context.Close();
            }
            return SiteAlias;

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

        public static bool IsWebAliasAvailable(string webAlias)
        {
            //var results = Exigo.OData().CustomerSites
            //    .Where(cs => cs.WebAlias == webAlias);
            //return results.Count() == 0;

            CustomerSite site;
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("CheckWebAliasAvailablity '{0}'", webAlias.Replace("'", "''"));
                site = context.Query<CustomerSite>(sqlProcedure).FirstOrDefault();
            }
            return site==null?false:site.CustomerID > 0 ? true : false;
        }

        public static bool IsWebAliasAvailable(int customerID, string webalias)
        {
            // Get the current webalias to see if it matches what we passed. If so, it's still valid.
            var currentWebAlias = Exigo.GetCustomerSite(customerID).WebAlias;
            if(currentWebAlias==null)
            if (webalias.Equals(currentWebAlias, StringComparison.InvariantCultureIgnoreCase)) return true;


            // Validate the web alias
            return Exigo.WebService().Validate(new IsLoginNameAvailableValidateRequest
            {
                LoginName = webalias
            }).IsValid;
        }
        public static bool IsWebAliasForSameCustomer(int customerID, string webAlias)
        {
            // Get the current webalias to see if it matches what we passed. If so, it's still valid.
            CustomerSite site;
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("CheckWebAliasAvailablity '{0}'", webAlias.Replace("'", "''"));
                site = context.Query<CustomerSite>(sqlProcedure).FirstOrDefault();
            }
            if (site == null)
            {
                return false;
            }
            else if (site.CustomerID == customerID)
            {
                return false;
            }
            else
                return true;
        }
    }
}