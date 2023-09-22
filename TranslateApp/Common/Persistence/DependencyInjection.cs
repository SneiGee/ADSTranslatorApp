using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using TranslateApp.Common.Persistence.ConnectionString;
using TranslateApp.Common.Persistence.Initialization;
using TranslateApp.Data;

namespace TranslateApp.Common.Persistence;

internal static class DependencyInjection
{

    private static readonly Serilog.ILogger _logger = Log.ForContext(typeof(DependencyInjection));

    internal static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(nameof(DatabaseSettings))
            .PostConfigure(databaseSettings =>
            {
                _logger.Information("Current DB Provider: {dbProvider}", databaseSettings.DBProvider);
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services
            .AddDbContext<TranslateDbContext>((p, m) =>
            {
                var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                m.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
            })

            .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
            .AddTransient<ApplicationDbInitializer>()
            .AddTransient<ApplicationDbSeeder>()
            .AddServices(typeof(ICustomSeeder), ServiceLifetime.Transient)
            .AddTransient<CustomSeederRunner>()

            .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>()
            .AddTransient<IConnectionStringValidator, ConnectionStringValidator>();
    }


    internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
    {
        return dbProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.SqlServer => builder.UseSqlServer(connectionString),
            DbProviderKeys.SqLite => builder.UseSqlite(connectionString),
            _ => throw new InvalidOperationException($"DB Provider {dbProvider} is not supported."),
        };
    }
}