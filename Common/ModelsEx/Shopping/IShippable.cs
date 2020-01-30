namespace Common.ModelsEx.Shopping
{
    public interface IShippable
    {
        int ShippingMethodId { get; set; }

        decimal Shipping { get; }
    }
}