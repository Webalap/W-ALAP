using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class Rank : IRank
    {
        public int RankID { get; set; }
        public string RankDescription { get; set; }
    }
}