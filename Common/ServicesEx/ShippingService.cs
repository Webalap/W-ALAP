using Common.Api.ExigoWebService;
using ExigoService;
using Ninject;
using System.Collections.Generic;
using System.Linq;

namespace Common.ServicesEx
{
    public class ShippingService : IShippingService
    {
        #region Dependencies

        [Inject]
        public ExigoApi Api { get; set; }
       
        #endregion

        List<ShipMethodResponse> IShippingService.GetStarterKitShipMethods(IOrderConfiguration configuration)
        {
            List<ShipMethodResponse> lst= GetShipMethodsFromApi(
                new GetShipMethodsRequest
                {
                    CurrencyCode = configuration.CurrencyCode,
                    WarehouseID = Warehouses.StarterKitsandSubscriptionsUSA
                }
            );
            //List<ShipMethodResponse> lstReturn = (from i in lst where i.Description.Contains("2-day Expedited") select i).ToList();
            //lstReturn.AddRange(GetShipMethodsFromApi(
            //    new GetShipMethodsRequest
            //    {
            //        CurrencyCode = configuration.CurrencyCode,
            //        WarehouseID = Warehouses.StarterKitsandSubscriptionsUSA
            //    }
            //));
            return lst;
        }

        List<ShipMethodResponse> IShippingService.GetDefaultShipMethods(IOrderConfiguration configuration)
        {
            return GetShipMethodsFromApi(
                new GetShipMethodsRequest
                {
                    CurrencyCode = configuration.CurrencyCode,
                    WarehouseID = configuration.WarehouseID
                }
            );
        }

        string IShippingService.GetShippingMethodDescription(int shipMethodId)
        {
            try
            {
                var context = Exigo.Sql();
                var SqlProcedure = string.Format("GetShippingMethodDescription {0}", shipMethodId);
                var shipMethod = context.Query<Common.Api.ExigoOData.ShipMethod>(SqlProcedure).FirstOrDefault();

                if (null == shipMethod) return "Call Customer Service";

                return shipMethod.ShipMethodDescription;
            }
            catch
            { return "Call Customer Service"; }
        }

        private List<ShipMethodResponse> GetShipMethodsFromApi(GetShipMethodsRequest request)
        {
            var response = Api.GetShipMethods(request);
            return response.ShipMethods.ToList();
        }
    }
}