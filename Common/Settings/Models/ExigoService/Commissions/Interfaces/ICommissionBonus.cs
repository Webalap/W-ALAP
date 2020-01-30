using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface ICommissionBonus
    {
        int BonusID { get; set; }
        string BonusDescription { get; set; }
        decimal Total { get; set; }
    }
}