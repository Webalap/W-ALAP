using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class GetUplineRequest
    {
        public int TopCustomerID { get; set; }
        public int BottomCustomerID { get; set; }
    }
}