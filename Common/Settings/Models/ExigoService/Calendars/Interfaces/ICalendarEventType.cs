using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface ICalendarEventType
    {
        int CalendarEventTypeID { get; set; }
        string CalendarEventTypeDescription { get; set; }
        string Color { get; set; }
    }
}