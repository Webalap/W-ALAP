using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface IRank
    {
        int RankID { get; set; }
        string RankDescription { get; set; }
    }
}