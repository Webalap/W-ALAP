﻿using Common;
using Common.Api.ExigoWebService;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static ExigoApi WebService(int sandboxID = 0)
        {
            return GetWebServiceContext(((sandboxID > 0) ? sandboxID : GlobalSettings.Exigo.Api.SandboxID));
        }
    }
}