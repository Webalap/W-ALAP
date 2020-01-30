using System.Collections.Generic;

namespace Common.ServicesEx
{
    interface ICustomerService
    {
        // TODO: GEORGE B. - change "object" to the actual types you need.
        List<object> SearchCustomers(object request);
    }
}