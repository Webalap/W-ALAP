using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Api.ExigoOData.Calendars
{
    public partial class Calendar
    {
        public static explicit operator ExigoService.Calendar(Calendar calendar)
        {
            var model = new ExigoService.Calendar();
            if (calendar == null) return model;

            model.CalendarID     = calendar.CalendarID;
            model.CustomerID     = calendar.CustomerID;
            model.Description    = calendar.Description;
            model.CalendarTypeID = calendar.CalendarTypeID;
            model.CreatedDate    = calendar.CreatedDate;

            return model;
        }
    }
}