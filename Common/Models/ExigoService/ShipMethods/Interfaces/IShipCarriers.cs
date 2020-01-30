namespace ExigoService
{
    public interface IShipCarriers
    {
        int ShipCarrierID { get; set; }
        string ShipCarrierDescription { get; set; }
        string TrackingUrl { get; set; }
    }
}


