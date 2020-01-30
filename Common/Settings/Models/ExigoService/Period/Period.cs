using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class Period : IPeriod
    {
        public int PeriodID { get; set; }
        public int PeriodTypeID { get; set; }
        public string PeriodDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}