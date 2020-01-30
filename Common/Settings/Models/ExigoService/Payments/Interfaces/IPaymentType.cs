using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public interface IPaymentType
    {
        int PaymentTypeID { get; set; }
        string PaymentTypeDescription { get; set; }
    }
}
