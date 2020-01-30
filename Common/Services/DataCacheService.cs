using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Threading.Tasks;

namespace Common.Services
{
    #region Interfaces
    public interface IDataCache
    {
        T Get<T>(string key, Func<T> command);
        T Get<T>(string key, long secondsToLive, Func<T> command);
    }
    public interface IDataCacheProvider
    {
        void Initialize();
        void Purge();
        bool TryGet<T>(string key, out DateTime entryDate, out DateTime now, out long secondsToLive, out T o);
        void Put<T>(string key, long secondsToLive, T o);
    }
    #endregion

    #region Configuration
    public static class DataCacheConfig
    {
        private static IDataCacheProvider _dataCacheProvider;
        private static long _secondsToLive = 300;

        public static void Initialize(IDataCacheProvider dataCacheProvider, long secondsToLive=300)
        {
            _secondsToLive     = secondsToLive;
            _dataCacheProvider = dataCacheProvider;
            _dataCacheProvider.Initialize();
        }

        public static IDataCacheProvider ClientCacheProvider { get { return _dataCacheProvider;} }
        public static long SecondsToLive { get { return _secondsToLive; } }
    }
    #endregion

    #region Service
    public class DataCacheService : IDataCache
    {
        private IDataCacheProvider _clientCacheProvider;

        public DataCacheService()
        {
            _clientCacheProvider = DataCacheConfig.ClientCacheProvider;
        }
        public DataCacheService(IDataCacheProvider clientCacheProvider)
        {
            _clientCacheProvider = clientCacheProvider;
        }

        public T Get<T>(string key, Func<T> command)
        {
            return Get(key, DataCacheConfig.SecondsToLive, command);
        }
        public T Get<T>(string key, long secondsToLive, Func<T> command)
        {
            if (_clientCacheProvider == null)
                throw new Exception("DataCacheProvider is not set. Run DataCacheConfig.Initialize in Application_Start()");

            DateTime entryDate;
            DateTime serverDate;
            long fetchedSecondsToLive;

            T result;

            // Try to get the data
            if (_clientCacheProvider.TryGet<T>(key, out entryDate, out serverDate, out fetchedSecondsToLive, out result))
            {
                // Looks like we have data!
                // If the data is expired, but we got it back from the server, the Purge process hasn't picked up on it yet.
                // Let's go ahead and return what we have, but silently refetch the data in the background.
                if (entryDate.AddSeconds(secondsToLive) < serverDate)
                {
                    // If it'S REALLY old, and for some reason our Purge didn't pick up on this, run it again.
                    if (entryDate.AddSeconds(secondsToLive * 5) >= serverDate)
                    {
                        Task.Run(() =>
                        {
                            _clientCacheProvider.Put<T>(key, secondsToLive, command());
                        });
                    }
                    else
                    { 
                        result = default(T);
                    }
                }
            }

            // If we didn't get any data back
            if (result == null)
            {
                result = command();
                _clientCacheProvider.Put<T>(key, secondsToLive, result);
            }

            return result;
        }
    }
    #endregion

    #region Extensions
    public static class DataCacheExtensions
    {
        private static IDataCache service = new DataCacheService();

        public static IDataCache DataCache(this System.Web.Mvc.Controller controller)
        {
            return service;
        }
        public static IDataCache DataCache(this System.Web.UI.Page page)
        {
            return service;
        }
    }
    #endregion

    #region Providers
    public class SqlCompactDataCacheProvider : IDataCacheProvider
    {
        private string _connectionString;

        public SqlCompactDataCacheProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Initialize()
        {
            // Create the required schema and tables, if applicable
            var dataDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "App_Data";
            if (!Directory.Exists(dataDirectoryPath))
            {
                Directory.CreateDirectory(dataDirectoryPath);
            }

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\cache.sdf"))
            {
                var database = new SqlCeEngine(_connectionString);
                database.CreateDatabase();
            }


            using (var connection = new SqlCeConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCeCommand())
                {
                    command.Connection = connection;
                    if (TableExists(connection, "DataCache"))
                    {
                        command.CommandText = "DROP TABLE DataCache";
                        command.ExecuteNonQuery();
                    }
                    command.CommandText = @"
                        CREATE TABLE DataCache 
                        (
                            ID nvarchar(250) NOT NULL, 
                            Data ntext,
                            CreatedDate DATETIME NOT NULL,
                            SecondsToLive bigint NOT NULL,
                            PRIMARY KEY (ID)
                        )";
                    command.ExecuteNonQuery();
                }
            }


            // Initialize the purge process
            Purge();
        }
        private bool TableExists(SqlCeConnection connection, string tableName)
        {
            using (var command = new SqlCeCommand())
            {
                command.Connection = connection;
                var sql = string.Format(
                        "SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '{0}'",
                         tableName.Replace("'", "''"));
                command.CommandText = sql;
                var count = Convert.ToInt32(command.ExecuteScalar());
                return (count > 0);
            }
        }

        public void Purge()
        {
            var purgeInterval = 180; // in seconds             
            Task.Run(async delegate
            {
                while (true)
                {
                    await Task.Delay(purgeInterval * 1000);

                    using (var connection = new SqlCeConnection(_connectionString))
                    {
                        using (var command = new SqlCeCommand(@"
                            DELETE FROM DataCache
                            WHERE DATEADD(ss,SecondsToLive,CreatedDate) < GETDATE()
                        ", connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
            });
        }

        public bool TryGet<T>(string key, out DateTime entryDate, out DateTime serverDate, out long secondsToLive, out T result)
        {
            entryDate = DateTime.MinValue;
            serverDate = DateTime.MinValue;
            secondsToLive = long.MinValue;


            string serializedData = string.Empty;
            using (var connection = new SqlCeConnection(_connectionString))
            {
                using (var command = new SqlCeCommand(@"SELECT ID, Data, CreatedDate, SecondsToLive, GETDATE() as CurrentDate FROM DataCache WHERE ID = @id ", connection))
                {
                    command.Parameters.Add("@id", System.Data.SqlDbType.NVarChar, 250).Value = key;

                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        serializedData = reader.GetString(1);
                        entryDate = reader.GetDateTime(2);
                        secondsToLive = reader.GetInt64(3);
                        serverDate = reader.GetDateTime(4);
                    }

                    connection.Close();
                }
            }

            // If we got data from the cache...
            if (!string.IsNullOrEmpty(serializedData))
            {
                // Return what we have
                result = JsonConvert.DeserializeObject<T>(serializedData);

                return true;
            }
            else
            {
                result = default(T);

                return false;
            }

        }

        public void Put<T>(string key, long secondsToLive, T o)
        {
            var serializedData = JsonConvert.SerializeObject(o);

            // Put the data into the database
            using (var connection = new SqlCeConnection(_connectionString))
            {
                using (var command = new SqlCeCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT COUNT(1) FROM DataCache WHERE ID = @id";
                    command.Parameters.Add(new SqlCeParameter("@id", key));

                    connection.Open();

                    if ((int)command.ExecuteScalar() > 0)
                    {
                        command.CommandText = @"
                            UPDATE DataCache
                            SET 
                                Data          = @data,
                                CreatedDate   = GETDATE(),
                                SecondsToLive = @secondstolive
                            WHERE ID = @id
                        ";
                    }
                    else
                    {
                        command.CommandText = @"
                            INSERT INTO DataCache
                            (
                                ID,
                                Data,
                                CreatedDate,
                                SecondsToLive
                            ) VALUES (
                                @id,
                                @data,
                                GETDATE(),
                                @secondstolive
                            )
                        ";
                    } 

                    command.Parameters.Add(new SqlCeParameter("@data", serializedData));
                    command.Parameters.Add(new SqlCeParameter("@secondstolive", secondsToLive));

                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public class SqlDataCacheProvider : IDataCacheProvider
    {
        private string _connectionString;

        public SqlDataCacheProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Initialize()
        {
            // Create the required schema and tables, if applicable
            using(var connection = new SqlConnection(_connectionString))
            {
                using(var command = new SqlCommand(@"
                    -- Ensure the datacache schema
                    IF NOT EXISTS (
                    SELECT  schema_name
                    FROM    information_schema.schemata
                    WHERE   schema_name = '_cache') 

                    BEGIN
                    EXEC sp_executesql N'CREATE SCHEMA _cache'
                    END


                    -- Ensure the table 
                    IF NOT EXISTS (SELECT * FROM sys.objects 
                    WHERE object_id = OBJECT_ID(N'[_cache].[DataCache]') AND type in (N'U'))

                    BEGIN
                    CREATE TABLE [_cache].[DataCache] (
                        ID nvarchar(250) NOT NULL, 
                        Data nvarchar(MAX),
                        CreatedDate DATETIME NOT NULL,
                        SecondsToLive bigint NOT NULL,
                        PRIMARY KEY (ID)
                    )
                    END
                ", connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            // Initialize the purge process
            Purge();
        }

        public void Purge()
        {
            var purgeInterval = 180; // in seconds             
            Task.Run(async delegate 
            {
                while (true)
                {   
                    await Task.Delay(purgeInterval * 1000);

                    using (var connection = new SqlConnection(_connectionString))
                    {
                        using (var command = new SqlCommand(@"
                            DELETE FROM _cache.DataCache
                            WHERE DATEADD(ss,SecondsToLive,CreatedDate) < GETDATE()
                        ", connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
            });
        }

        public bool TryGet<T>(string key, out DateTime entryDate, out DateTime serverDate, out long secondsToLive, out T result)
        {
            entryDate     = DateTime.MinValue;
            serverDate    = DateTime.MinValue;
            secondsToLive = long.MinValue;


            string serializedData = string.Empty;
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand(@"
                        SELECT
                            ID,
                            Data,
                            CreatedDate,
                            SecondsToLive,
                            CurrentDate = GETDATE()
                        FROM [_cache].[DataCache]
                        WHERE ID = @id
                        ", connection))
                    {
                        command.Parameters.Add("@id", System.Data.SqlDbType.NVarChar, 250).Value = key;


                        connection.Open();
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            serializedData = reader.GetString(1);
                            entryDate      = reader.GetDateTime(2);
                            secondsToLive  = reader.GetInt64(3);
                            serverDate     = reader.GetDateTime(4);
                        }

                        connection.Close();
                    }
                }

                // If we got data from the cache...
                if (!string.IsNullOrEmpty(serializedData))
                {
                    // Return what we have
                    result = JsonConvert.DeserializeObject<T>(serializedData);

                    return true;
                }
                else
                {
                    result = default(T);

                    return false;
                }
            
        }

        public void Put<T>(string key, long secondsToLive, T o)
        {
            //var stream = new MemoryStream();
            //var formatter = new BinaryFormatter();
            //formatter.Serialize(stream, o);

            var serializedData = JsonConvert.SerializeObject(o);

            // Put the data into the database
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(@"
                    UPDATE [_cache].[DataCache] 
                    SET 
                        Data          = @data,
                        CreatedDate   = GETDATE(),
                        SecondsToLive = @secondstolive
                    WHERE ID = @id;

                    IF @@ROWCOUNT=0
                        INSERT INTO [_cache].[DataCache] 
                        (
                            ID,
                            Data,
                            CreatedDate,
                            SecondsToLive
                        ) VALUES (
                            @id,
                            @data,
                            GETDATE(),
                            @secondstolive
                        );
                ", connection))
                {
                    command.Parameters.Add("@id", System.Data.SqlDbType.NVarChar, 250).Value     = key;
                    command.Parameters.Add("@data", System.Data.SqlDbType.NVarChar, -1).Value    = serializedData;
                    command.Parameters.Add("@secondstolive", System.Data.SqlDbType.BigInt).Value = secondsToLive;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
    #endregion
}