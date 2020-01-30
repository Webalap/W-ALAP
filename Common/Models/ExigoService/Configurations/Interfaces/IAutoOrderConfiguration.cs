using Common.Api.ExigoWebService;

namespace ExigoService
{
    public interface IAutoOrderConfiguration : IOrderConfiguration
    {
        FrequencyType DefaultFrequencyType { get; }
    }
}