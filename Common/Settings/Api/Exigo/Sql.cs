using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Client;
using System.Data.SqlClient;
using Common;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static SqlConnection Sql()
        {
            return new SqlConnection(GlobalSettings.Exigo.Api.Sql.ConnectionStrings.SqlReporting);
        }
        public static SqlConnection Sql(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}