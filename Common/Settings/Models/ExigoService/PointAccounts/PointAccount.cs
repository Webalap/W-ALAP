using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public class PointAccount : IPointAccount
    {
        public int PointAccountID { get; set; }
        public string PointAccountDescription { get; set; }
    }
}
