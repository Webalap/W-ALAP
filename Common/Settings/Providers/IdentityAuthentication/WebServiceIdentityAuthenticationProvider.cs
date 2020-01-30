using Common.Api.ExigoWebService;
using Common.Helpers;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Providers
{
    public class WebServiceIdentityAuthenticationProvider : IIdentityAuthenticationProvider
    {
        public int AuthenticateCustomer(string loginname, string password)
        {
            var response = Exigo.WebService().AuthenticateCustomer(new AuthenticateCustomerRequest
            {
                LoginName = loginname,
                Password = password
            });

            return response.CustomerID;
        }
        public int AuthenticateCustomer(int customerid)
        {
            var response = Exigo.WebService().GetCustomers(new GetCustomersRequest
            {
                CustomerID = customerid
            });

            if (response.Customers.Length == 0) return 0;
            else return response.Customers[0].CustomerID;
        }
    }
}
