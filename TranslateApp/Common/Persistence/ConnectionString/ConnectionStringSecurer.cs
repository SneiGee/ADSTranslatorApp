using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace TranslateApp.Common.Persistence.ConnectionString;

public class ConnectionStringSecurer : IConnectionStringSecurer
{
    private const string HiddenValueDefault = "*******";
    private readonly DatabaseSettings _dbSettings;

    public ConnectionStringSecurer(IOptions<DatabaseSettings> dbSettings) =>
        _dbSettings = dbSettings.Value;

    public string? MakeSecure(string? connectionString, string? dbProvider)
    {
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }

        if (string.IsNullOrWhiteSpace(dbProvider))
        {
            dbProvider = _dbSettings.DBProvider;
        }

        return dbProvider?.ToLower() switch
        {
            DbProviderKeys.SqlServer => MakeSecureSqlConnectionString(connectionString),
            DbProviderKeys.SqLite => MakeSecureSqLiteConnectionString(connectionString),
            _ => connectionString
        };
    }
    private static string MakeSecureSqlConnectionString(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);

        if (!string.IsNullOrEmpty(builder.Password) || !builder.IntegratedSecurity)
        {
            builder.Password = HiddenValueDefault;
        }

        if (!string.IsNullOrEmpty(builder.UserID) || !builder.IntegratedSecurity)
        {
            builder.UserID = HiddenValueDefault;
        }

        return builder.ToString();
    }

    private static string MakeSecureSqLiteConnectionString(string connectionString)
    {
        var builder = new SqliteConnection(connectionString);

        return builder.ToString();
    }
}