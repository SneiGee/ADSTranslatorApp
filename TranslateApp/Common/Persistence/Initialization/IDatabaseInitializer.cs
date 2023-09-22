namespace TranslateApp.Common.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForUserAsync(CancellationToken cancellationToken);
}