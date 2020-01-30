using System.Web.Mvc;

namespace Common.Filters
{
    public static class GlobalFilters
    {
        /// <summary>
        /// Exigo-specific API credentials and configurations
        /// </summary>
        public static void Register(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExigoErrorAttribute());
            
            // NOTE: I am commenting this line out as it's preventing
            // us from using client-side AJAX calls from Angular.
            // This functionality can still be achieved by calling
            // Html.AntiForgeryToken() within a <form> and decorating 
            // an action method with [ValidateAntiForgeryToken].
            //filters.Add(new ValidateAntiForgeryTokenOnAllPostsAttribute());
        }
    }
}