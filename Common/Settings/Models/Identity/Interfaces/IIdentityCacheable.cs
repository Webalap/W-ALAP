using ExigoService;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Common.Models
{
    public interface IIdentityCacheable
    {
        string CacheKey { get; set; }

        void Initialize(int customerID);
        void RefreshCache();
    }
}