using System.Data;
using Dapper;

namespace Savvyio.Assets
{
    public static class DbConnectionExtensions
    {
        public static IDbConnection SetDefaults(this IDbConnection cnn)
        {
            SqlMapper.AddTypeHandler(new GuidHandler());
            cnn.Open();
            return cnn;
        }

        public static IDbConnection AddAccountTable(this IDbConnection cnn)
        {
            cnn.ExecuteAsync("CREATE TABLE Account (Id INTEGER PRIMARY KEY, FullName VARCHAR(255), EmailAddress VARCHAR(512))");
            return cnn;
        }

        public static IDbConnection AddPlatformProviderTable(this IDbConnection cnn)
        {
            cnn.ExecuteAsync("CREATE TABLE PlatformProvider (Id CHAR(36) PRIMARY KEY, Name VARCHAR(255), ThirdLevelDomainName VARCHAR(128), Description VARCHAR(512), Policy VARCHAR(1024)");
            return cnn;
        }
    }
}
