using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class CommissionBonus : ICommissionBonus
    {
        public int BonusID { get; set; }
        public string BonusDescription { get; set; }
        public decimal Total { get; set; }
    }
}