using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public class GetCustomerRanksRequest
    {
        public int CustomerID { get; set; }
        public int PeriodTypeID { get; set; }
        public int? PeriodID { get; set; }
    }
}
