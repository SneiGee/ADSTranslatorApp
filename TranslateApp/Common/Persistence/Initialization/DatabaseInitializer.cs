
namespace TranslateApp.Common.Persistence.Initialization;

internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
    {
        // _logger.LogInformation("Applying Admin Root Migrations.");

        await InitializeApplicationDbForUserAsync(cancellationToken);

        _logger.LogInformation("Welcome to Advance Field App - Webs & Mobile Apps API ");
        _logger.LogInformation("For documentations and guides, visit https://github.com/SneiGee/KeyDetectApp/tree/master/Docs/Api");
        _logger.LogInformation("Build by MICHAEL SCHNEIDER! -:)");
    }

    public async Task InitializeApplicationDbForUserAsync(CancellationToken cancellationToken)
    {
        // First create a new scope
        using var scope = _serviceProvider.CreateScope();

        // Then run the initialization in the new scope
        await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
            .InitializeAsync(cancellationToken);
    }
}