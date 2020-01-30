using System;
using System.Web;
using System.Web.Mvc;

namespace Common.Filters
{
    public class RequireSSL : ActionFilterAttribute
    {
        /*public override void OnActionExecuting(ActionExecutingContext filterContext)
       {
           HttpRequestBase req = filterContext.HttpContext.Request;
           HttpResponseBase res = filterContext.HttpContext.Response;

           //Check if we're secure or not and if we're on the local box
           //if (!req.IsSecureConnection && !req.IsLocal)
           if (!req.IsSecureConnection) //For testing Locally
           {
               var builder = new UriBuilder(req.Url)
               {
                   Scheme = Uri.UriSchemeHttps,
                   Port = 443
               };
               res.Redirect(builder.Uri.ToString());
           }
           base.OnActionExecuting(filterContext);
       }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            HttpRequestBase req = filterContext.HttpContext.Request;
            HttpResponseBase res = filterContext.HttpContext.Response;

            //Check if we're secure or not and if we're on the local box
            //if (!req.IsSecureConnection && !req.IsLocal)
            if (!req.IsSecureConnection) //For testing Locally
            {
                var builder = new UriBuilder(req.Url)
                {
                    Scheme = Uri.UriSchemeHttps,
                    Port = 443
                };
                res.Redirect(builder.Uri.ToString());
            }
            
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            HttpRequestBase req = filterContext.HttpContext.Request;
            HttpResponseBase res = filterContext.HttpContext.Response;

            //Check if we're secure or not and if we're on the local box
            //if (!req.IsSecureConnection && !req.IsLocal)
            if (!req.IsSecureConnection) //For testing Locally
            {
                var builder = new UriBuilder(req.Url)
                {
                    Scheme = Uri.UriSchemeHttps,
                    Port = 443
                };
                res.Redirect(builder.Uri.ToString());
            }
            base.OnResultExecuting(filterContext);
        }*/

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            HttpRequestBase req = filterContext.HttpContext.Request;
            HttpResponseBase res = filterContext.HttpContext.Response;

            //Check if we're secure or not and if we're on the local box
            if (!req.IsSecureConnection && !req.IsLocal)
            //if (!req.IsSecureConnection) //For testing Locally
            {
                var builder = new UriBuilder(req.Url)
                {
                    Scheme = Uri.UriSchemeHttps,
                    Port = 443
                };
                res.Redirect(builder.Uri.ToString());
            }
            base.OnResultExecuted(filterContext);
        }

       
    }
    
}