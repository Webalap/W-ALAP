namespace ExigoService
{
    public class VolumeCollectionGrid
    {
        public int PeriodID { get; set; }
        public string PeriodDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Rank { get; set; }
        public string PaidRank { get; set; }
        public decimal PBV { get; set; }
        public decimal GBV { get; set; }
        public decimal PRV { get; set; }
        public decimal LRV { get; set; }
        public decimal TGRV { get; set; }        
        public int Count { get; set; }
        public decimal Revenue { get; set; }
        public int TotalRows { get; set; }
        public int RowsSearched { get; set; }
    }
}