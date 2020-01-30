using System.Globalization;
using System.Threading;
using System.Web;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Sets the CultureCode of the site based on the current market.
        /// </summary>
        public static void SetCurrentCulture(string cultureCode)
        {
            if (!HttpContext.Current.Request.IsAuthenticated) return;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureCode);
        }

        /// <summary>
        /// Sets the CurrentUICulture of the site based on the user's language preferences.
        /// </summary>
        public static void SetCurrentUICulture(string cultureCode)
        {
            if (!HttpContext.Current.Request.IsAuthenticated) return;

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureCode);
        }
    }
}