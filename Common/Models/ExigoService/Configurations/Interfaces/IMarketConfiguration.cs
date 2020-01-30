using Common;

namespace ExigoService
{
    public interface IMarketConfiguration
    {
        MarketName MarketName { get; }
        IOrderConfiguration Orders { get; }
        IOrderConfiguration CustomerOrders { get; }
        IAutoOrderConfiguration AutoOrders { get; }
    }
}