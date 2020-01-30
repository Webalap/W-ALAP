using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public class SendSMSRequest
    {
        public int CustomerID { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}
