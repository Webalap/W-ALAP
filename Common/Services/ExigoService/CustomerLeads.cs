using Common;
using Common.Api.ExigoWebService;
using System;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static Customer GetCustomerLead(int customerLeadID)
        {
            //var customer = Exigo.OData().Customers.Expand("CustomerStatus")
            //    .Where(c => c.CustomerID == customerLeadID)
            //    .Where(c => c.CustomerTypeID == CustomerTypes.Lead)
            //    .FirstOrDefault();
            var customer = Exigo.GetCustomerByTypeID(customerLeadID, CustomerTypes.Lead);
            if (customer == null) return null;

            return (Customer)customer;
        }

        public static int CreateCustomerLead(CreateCustomerLeadRequest Request)
        {
            CreateCustomerLeadResponse Response = new CreateCustomerLeadResponse();
            ExigoApi Api = Exigo.WebService();
            Response = Api.CreateCustomerLead(Request);
            if (ResultStatus.Success.Equals(Response.Result.Status))
            {
                return Response.CustomerLeadID;
            }

            throw new ApplicationException("Create CustomerLead Failed.");

        }

    }

    
}