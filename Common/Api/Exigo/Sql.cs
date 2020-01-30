using Common;
using System.Data.SqlClient;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static SqlConnection Sql()
        {
            return new SqlConnection(GlobalSettings.Exigo.Api.Sql.ConnectionStrings.GetConnectionString());
        }
        public static SqlConnection Sql(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}