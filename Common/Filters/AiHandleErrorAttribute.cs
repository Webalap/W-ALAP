using System.Web.Mvc;
using Microsoft.ApplicationInsights;

namespace Common.Filters
{
        public class AiHandleErrorAttribute : HandleErrorAttribute
        {
            public override void OnException(ExceptionContext filterContext)
            {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
                {
                    filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                    //If customError is Off, then AI HTTPModule will report the exception
                    //If it is On, or RemoteOnly (default) - then we need to explicitly track the exception
                    if (filterContext.HttpContext.IsCustomErrorEnabled)
                        {
                            var ai = new TelemetryClient();
                            ai.TrackException(filterContext.Exception);
                        }
                }
                //base.OnException(filterContext);
            }
        }
}