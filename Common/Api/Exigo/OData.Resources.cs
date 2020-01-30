using Common;
using Common.Api.ExigoOData.ResourceManager;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static resourcescontext ODataResources(int sandboxID = 0)
        {
            return GetODataContext<resourcescontext>(((sandboxID > 0) ? sandboxID : GlobalSettings.Exigo.Api.SandboxID));
        }
    }
}
