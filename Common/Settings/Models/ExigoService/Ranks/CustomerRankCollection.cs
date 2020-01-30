using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class CustomerRankCollection
    {
        public Rank CurrentPeriodRank { get; set; }
        public Rank HighestPaidRankInAnyPeriod { get; set; }
        public Rank HighestPaidRankUpToPeriod { get; set; }
    }
}