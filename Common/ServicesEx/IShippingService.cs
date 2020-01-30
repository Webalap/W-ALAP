using Common.Api.ExigoWebService;
using ExigoService;
using System.Collections.Generic;

namespace Common.ServicesEx
{
    public interface IShippingService
    {
        List<ShipMethodResponse> GetDefaultShipMethods(IOrderConfiguration configuration);

        List<ShipMethodResponse> GetStarterKitShipMethods(IOrderConfiguration configuration);

        string GetShippingMethodDescription(int shipMethodId);
    }
}
