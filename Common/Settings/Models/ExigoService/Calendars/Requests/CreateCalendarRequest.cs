using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class CreateCalendarRequest
    {
        public int CustomerID { get; set; }
        public string Description { get; set; }
    }
}