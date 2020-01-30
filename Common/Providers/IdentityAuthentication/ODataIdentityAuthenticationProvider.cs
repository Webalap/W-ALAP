using Common.Helpers;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Providers
{
    public class ODataIdentityAuthenticationProvider : IIdentityAuthenticationProvider
    {
        public int AuthenticateCustomer(string loginname, string password)
        {
            var customer = (from c in Exigo.OData().CreateQuery<Customer>("AuthenticateLogin")
                    .AddQueryOption("loginName", "'" + loginname + "'")
                    .AddQueryOption("password", "'" + password + "'")
                            select new Customer { CustomerID = c.CustomerID }).FirstOrDefault();

            if (customer == null) return 0;
            else return customer.CustomerID;
        }
        public int AuthenticateCustomer(int customerid)
        {
            Customer customer = Exigo.GetCustomer(customerid);
            return customer.CustomerID;
        }
    }
}
