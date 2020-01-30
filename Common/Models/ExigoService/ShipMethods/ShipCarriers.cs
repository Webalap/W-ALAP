namespace ExigoService
{
    //public class ShipCarriers
    //{

    public class ShipCarriers : IShipCarriers
    {
        public int ShipCarrierID { get; set; }
        public string ShipCarrierDescription { get; set; }
        public string TrackingUrl { get; set; }
    }
}