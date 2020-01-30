using Common;
using Common.Api.ExigoAdminWebService;
using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        private static Dictionary<string, ExigoApiAdmin> AdminWebServiceContexts = new Dictionary<string, ExigoApiAdmin>();

        private static ExigoApiAdmin GetAdminWebServiceContext(int sandboxID)
        {
            var key = typeof(ExigoApiAdmin).Name + sandboxID;
            var context = AdminWebServiceContexts.Where(c => c.Key == key).FirstOrDefault().Value;
            if (context == null)
            {
                context = CreateAdminWebServiceContext(sandboxID);
                AdminWebServiceContexts.Add(key, context);
            }
            return context;
        }

        private static ExigoApiAdmin CreateAdminWebServiceContext(int sandboxID)
        {
            // Determine which URL we should use
            var url = GetAdminWebServiceUrl(sandboxID);

            // Create the context
            return new ExigoApiAdmin
            {
                ApiAuthenticationValue = new ApiAuthentication
                {
                    LoginName = GlobalSettings.Exigo.Api.LoginName,
                    Password  = GlobalSettings.Exigo.Api.Password,
                    Company   = GlobalSettings.Exigo.Api.CompanyKey
                },
                Url = url
            };
        }

        private static string GetAdminWebServiceUrl(int sandboxID)
        {
            var urlFormat = "http://{0}.exigo.com/admin/1.0/exigoapiadmin.asmx";
            var cname = GlobalSettings.Exigo.Api.GetSubdomain(sandboxID);

            return string.Format(urlFormat, cname);
        }
    }
}