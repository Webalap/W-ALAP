using Common;
using Common.Api.ExigoWebService;
using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        private static Dictionary<string, ExigoApi> WebServiceContexts = new Dictionary<string, ExigoApi>();
        private static object _lockObject = new object();

        private static ExigoApi GetWebServiceContext(int sandboxID)
        {
            var key = typeof(ExigoApi).Name + sandboxID;
            var context = WebServiceContexts.Where(c => c.Key == key).FirstOrDefault().Value;
            if (context == null)
            {
                // Add a lock here to ensure thread-safety.
                lock (_lockObject)
                {
                    if (WebServiceContexts.ContainsKey(key))
                    {
                        context = WebServiceContexts[key];
                    }
                    else
                    {
                        context = CreateWebServiceContext(sandboxID);
                        WebServiceContexts.Add(key, context);
                    }
                }
            }
            return context;
        }
        public static ExigoApi CreateWebServiceContext(int sandboxID)
        {
            // Determine which URL we should use
            var url = GetWebServiceUrl(sandboxID);

            // Create the context
            return new ExigoApi
            {
                ApiAuthenticationValue = new ApiAuthentication
                {
                    LoginName = GlobalSettings.Exigo.Api.LoginName,
                    Password = GlobalSettings.Exigo.Api.Password,
                    Company = GlobalSettings.Exigo.Api.CompanyKey
                },
                Url = url
            };
        }

        private static string GetWebServiceUrl(int sandboxID)
        {
            var urlFormat = "https://{0}.exigo.com/3.0/ExigoApi.asmx";
            var cname = GlobalSettings.Exigo.Api.GetSubdomain(sandboxID);

            return string.Format(urlFormat, cname);
        }
    }
}