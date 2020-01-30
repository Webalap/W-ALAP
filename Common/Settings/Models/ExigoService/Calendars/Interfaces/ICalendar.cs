using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface ICalendar
    {
        Guid CalendarID { get; set; }
        int CustomerID { get; set; }
        string Description { get; set; }
        int CalendarTypeID { get; set; }
        DateTime CreatedDate { get; set; }
    }
}