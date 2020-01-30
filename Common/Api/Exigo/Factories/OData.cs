using Common;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;

namespace ExigoService
{
    public static partial class Exigo
    {
        private static Dictionary<string, object> ODataContexts = new Dictionary<string, object>();

        private static T GetODataContext<T>(int sandboxID = 0) where T : DataServiceContext
        {
            var key = typeof(T).Name + sandboxID;
            var context = ODataContexts.Where(c => c.Key == key).FirstOrDefault().Value;
            if (context == null)
            {
                lock (_lockObject)
                {
                    if (ODataContexts.ContainsKey(key))
                    {
                        context = ODataContexts[key];
                    }
                    else
                    {
                        context = CreateODataContext<T>(sandboxID);
                        ODataContexts.Add(key, context);
                    }
                }
            }
            return (T)context;
        }
        public static T CreateODataContext<T>(int sandboxID) where T : DataServiceContext
        {
            // Determine some helpful variables
            var type = typeof(T);
            var typeName = type.Name;
            var schemaName = string.Empty;
            var url = string.Empty;

            // Determine which URL we should use
            switch (typeName)
            {
                case "ExigoContext": schemaName = "model"; break;
                case "ExigoReportingContext": schemaName = "reporting"; break;
                default: schemaName = typeName; break;
            }
            url = GetODataUrl(schemaName, sandboxID);

            // Create the context
            T context = (T)Activator.CreateInstance(typeof(T), new Uri(url));
            context.IgnoreMissingProperties = true;
            context.IgnoreResourceNotFoundException = true;
            context.MergeOption = MergeOption.OverwriteChanges;
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(GlobalSettings.Exigo.Api.LoginName + ":" + GlobalSettings.Exigo.Api.Password));
            context.SendingRequest += (object s, SendingRequestEventArgs e) =>
                    e.RequestHeaders.Add("Authorization", "Basic " + credentials);

            return context;
        }

        private static string GetODataUrl(string schema, int sandboxID)
        {
            var urlFormat = "http://{0}.exigo.com/4.0/{1}/{2}";

            var cname = GlobalSettings.Exigo.Api.GetSubdomain(sandboxID);

            var dbschema = string.Empty;
            if (!string.IsNullOrEmpty(schema))
            {
                var standardSchemas = new List<string> { "model", "reporting" };
                if (standardSchemas.Contains(schema)) dbschema = schema;
                else dbschema = "db/" + schema;
            }

            return string.Format(urlFormat, cname, GlobalSettings.Exigo.Api.CompanyKey, dbschema);
        }
    }
}