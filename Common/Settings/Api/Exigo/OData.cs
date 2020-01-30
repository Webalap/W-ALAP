using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Api.ExigoOData;
using System.Data.Services.Client;
using Common;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static ExigoContext OData()
        {
            return CreateODataContext(GlobalSettings.Exigo.Api.DefaultContextSource);
        }
        public static ExigoContext OData(ExigoApiSource source)
        {
            return CreateODataContext(source);
        }
        private static ExigoContext CreateODataContext(ExigoApiSource source)
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

            var context = new ExigoContext(new Uri(sourceUrl + "/model"));
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