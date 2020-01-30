namespace ExigoService
{
    public class HistoricalCommission : ICommission
    {
        public HistoricalCommission()
        {
            this.Period = Period.Default;
            this.PaidRank = Rank.Default;
            this.Volumes = VolumeCollection.Default;
        }
        public int CustomerID { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Total { get; set; }
        public Period Period { get; set; }

        public int CommissionRunID { get; set; }
        public decimal Earnings { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal BalanceForward { get; set; }
        public decimal Fee { get; set; }

        public Rank PaidRank { get; set; }

        public VolumeCollection Volumes { get; set; }
    }
}