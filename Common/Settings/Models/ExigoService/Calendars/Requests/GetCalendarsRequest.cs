using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class GetCalendarsRequest
    {
        public int? CustomerID { get; set; }
        public bool IncludeCalendarSubscriptions { get; set; }
        public IEnumerable<Guid> CalendarIDs { get; set; }
    }
}