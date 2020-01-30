using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class GetHistoricalCommissionBonusDetailsRequest : DataRequest
    {
        public GetHistoricalCommissionBonusDetailsRequest()
            : base()
        {
        }

        public int CommissionRunID { get; set; }
    }
}