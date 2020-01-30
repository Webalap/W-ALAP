using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Api.ExigoWebService;
using Common;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static ExigoApi WebService()
        {
            return CreateWebServiceContext(GlobalSettings.Exigo.Api.DefaultContextSource);
        }
        public static ExigoApi WebService(ExigoApiSource source)
        {
            return CreateWebServiceContext(source);
        }
        private static ExigoApi CreateWebServiceContext(ExigoApiSource source)
        {
            var sourceUrl = "";
            switch (source)
            {
                case ExigoApiSource.Live:
                    sourceUrl = GlobalSettings.Exigo.Api.WebService.LiveUrl;
                    break;
                case ExigoApiSource.Sandbox1:
                    sourceUrl = GlobalSettings.Exigo.Api.WebService.Sandbox1Url;
                    break;
                case ExigoApiSource.Sandbox2:
                    sourceUrl = GlobalSettings.Exigo.Api.WebService.Sandbox2Url;
                    break;
                case ExigoApiSource.Sandbox3:
                    sourceUrl = GlobalSettings.Exigo.Api.WebService.Sandbox3Url;
                    break;
            }

            return new ExigoApi
            {
                ApiAuthenticationValue = new ApiAuthentication
                {
                    LoginName = GlobalSettings.Exigo.Api.LoginName,
                    Password  = GlobalSettings.Exigo.Api.Password,
                    Company   = GlobalSettings.Exigo.Api.CompanyKey
                },
                Url = sourceUrl
            };
        }
    }
}