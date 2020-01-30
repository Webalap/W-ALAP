using ExigoService;

namespace Common.ModelsEx.Shopping
{
    public interface ITaxable
    {
        ShippingAddress ShippingAddress { get; set; }

        decimal Tax { get; }
    }
}