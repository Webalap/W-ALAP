using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public class GetDownlineUpcomingPromotionsRequest : DataRequest
    {
        public int DownlineCustomerID { get; set; }
        public int PeriodTypeID { get; set; }
        public int? RankID { get; set; }        
    }
}
