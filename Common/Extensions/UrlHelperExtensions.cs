using System.Web.Mvc;

namespace Common.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string ActionWithWebAlias(this UrlHelper helper, string actionName, string controllerName = null, object id = null)
        {
            if (helper == null) return string.Empty;

            var webAlias = helper.RequestContext.RouteData.Values["webalias"];

            return helper.Action(actionName, controllerName, new {id = id, webalias = webAlias});
        }
    }
}