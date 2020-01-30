namespace ExigoService
{
    public class GetTreeRequest
    {
        public int CustomerID { get; set; }
        public int TopCustomerID { get; set; }
        public int CustomerTypeID { get; set; }
        public int Levels { get; set; }
        public int Legs { get; set; }
        public bool IncludeOpenPositions { get; set; }
        public bool IncludeNullPositions { get; set; }
    }
}