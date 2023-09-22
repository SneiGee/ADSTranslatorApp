using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.SQLite;
using Hangfire.SqlServer;
using Serilog;
using TranslateApp.Common.Persistence;

namespace TranslateApp.BackgroundJobs;

internal static class DependencyInjection
{
    private static readonly Serilog.ILogger _logger = Log.ForContext(typeof(DependencyInjection));

    internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration config)
    {
        services.AddHangfireServer(options => config.GetSection("HangfireSettings:Server").Bind(options));

        services.AddHangfireConsoleExtensions();

        var storageSettings = config.GetSection("HangfireSettings:Storage").Get<HangfireStorageSettings>();
        if (storageSettings is null) throw new Exception("Hangfire Storage Provider is not configured.");
        if (string.IsNullOrEmpty(storageSettings.StorageProvider)) throw new Exception("Hangfire Storage Provider is not configured.");
        if (string.IsNullOrEmpty(storageSettings.ConnectionString)) throw new Exception("Hangfire Storage Provider ConnectionString is not configured.");
        _logger.Information($"Hangfire: Current Storage Provider : {storageSettings.StorageProvider}");
        _logger.Information("For more Hangfire storage, visit https://www.hangfire.io/extensions.html");

        services.AddSingleton<JobActivator, ADSJobActivator>();

        services.AddHangfire((provider, hangfireConfig) => hangfireConfig
            .UseDatabase(storageSettings.StorageProvider, storageSettings.ConnectionString, config)
            .UseFilter(new ADSJobFilter(provider))
            .UseFilter(new LogJobFilter())
            .UseConsole());

        return services;
    }

    private static IGlobalConfiguration UseDatabase(this IGlobalConfiguration hangfireConfig, string dbProvider, string connectionString, IConfiguration config) =>
        dbProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.SqlServer =>
                hangfireConfig.UseSqlServerStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<SqlServerStorageOptions>()),
            DbProviderKeys.SqLite =>
                hangfireConfig.UseSQLiteStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<SQLiteStorageOptions>()),
            _ => throw new Exception($"Hangfire Storage Provider {dbProvider} is not supported.")
        };

    internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration config)
    {
        var dashboardOptions = config.GetSection("HangfireSettings:Dashboard").Get<DashboardOptions>();
        if (dashboardOptions is null) throw new Exception("Hangfire Dashboard is not configured.");
        dashboardOptions.Authorization = new[]
        {
           new HangfireCustomBasicAuthenticationFilter
           {
                User = config.GetSection("HangfireSettings:Credentials:User").Value!,
                Pass = config.GetSection("HangfireSettings:Credentials:Password").Value!
           }
        };

        return app.UseHangfireDashboard(config["HangfireSettings:Route"], dashboardOptions);
    }
}