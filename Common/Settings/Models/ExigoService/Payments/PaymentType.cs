using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public class PaymentType : IPaymentType
    {
        public int PaymentTypeID { get; set; }
        public string PaymentTypeDescription { get; set; }
    }
}
