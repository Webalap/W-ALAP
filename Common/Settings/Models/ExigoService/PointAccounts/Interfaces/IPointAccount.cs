using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public interface IPointAccount
    {
        int PointAccountID { get; set; }
        string PointAccountDescription { get; set; }
    }
}
