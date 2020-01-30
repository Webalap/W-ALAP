using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface IMarketConfiguration
    {
        MarketName MarketName { get; }
        IOrderConfiguration Orders { get; }
        IAutoOrderConfiguration AutoOrders { get; }
    }
}