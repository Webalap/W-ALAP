using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Common.Services;

namespace Common.HtmlHelpers
{
    public static class LinksHtmlHelpers
    {
        public static MvcHtmlString SilentLoginToken(this UrlHelper helper, int customerID)
        {
            var IV = GlobalSettings.Encryptions.SilentLogins.IV;
            var key = GlobalSettings.Encryptions.SilentLogins.Key;
            var token = Security.UrlEncrypt("{0}|{1}".FormatWith(customerID, DateTime.Now.AddHours(1)), key, IV);

            return new MvcHtmlString(token);
        }
        /*
        public static MvcHtmlString SampleSilentLogin(this UrlHelper helper)
        {
            var token = SilentLoginToken(helper);
            var url = "http://www.exigo.com/silentlogin";
            var separator = (!url.Contains("?")) ? "?" : "&";
            return new MvcHtmlString(url + separator + "token=" + SilentLoginToken(helper));
        }*/
    }
}