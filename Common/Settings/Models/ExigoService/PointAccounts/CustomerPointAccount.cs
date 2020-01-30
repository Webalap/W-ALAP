using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public class CustomerPointAccount : PointAccount
    {
        public int CustomerID { get; set; }
        public decimal Balance { get; set; }
    }
}
