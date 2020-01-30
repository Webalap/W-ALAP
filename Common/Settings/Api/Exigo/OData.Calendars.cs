using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Api.ExigoOData.Calendars;
using System.Data.Services.Client;
using Common;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static calendarcontext ODataCalendars()
        {
            return CreateODataCalendarsContext(GlobalSettings.Exigo.Api.DefaultContextSource);
        }
        public static calendarcontext ODataCalendars(ExigoApiSource source)
        {
            return CreateODataCalendarsContext(source);
        }
        private static calendarcontext CreateODataCalendarsContext(ExigoApiSource source)
        {
            var sourceUrl = "";
            switch (source)
            {
                case ExigoApiSource.Live:
                    sourceUrl = GlobalSettings.Exigo.Api.OData.LiveUrl;
                    break;
                case ExigoApiSource.Sandbox1:
                    sourceUrl = GlobalSettings.Exigo.Api.OData.Sandbox1Url;
                    break;
                case ExigoApiSource.Sandbox2:
                    sourceUrl = GlobalSettings.Exigo.Api.OData.Sandbox2Url;
                    break;
                case ExigoApiSource.Sandbox3:
                    sourceUrl = GlobalSettings.Exigo.Api.OData.Sandbox3Url;
                    break;
            }

            var context = new calendarcontext(new Uri(sourceUrl + "/db/calendarcontext"));
            context.IgnoreMissingProperties = true;
            context.IgnoreResourceNotFoundException = true;
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(GlobalSettings.Exigo.Api.LoginName + ":" + GlobalSettings.Exigo.Api.Password));
            context.SendingRequest +=
                (object s, SendingRequestEventArgs e) =>
                    e.RequestHeaders.Add("Authorization", "Basic " + credentials);
            return context;
        }
    }
}