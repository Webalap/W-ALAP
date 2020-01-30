using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface IVolumeCollection
    {
        int CustomerID { get; set; }

        Period Period { get; set; }
        Rank HighestAchievedRankThisPeriod { get; set; }
        Rank PayableAsRank { get; set; }
    }
}