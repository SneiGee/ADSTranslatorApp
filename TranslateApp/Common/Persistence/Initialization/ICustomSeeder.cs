namespace TranslateApp.Common.Persistence.Initialization;

public interface ICustomSeeder
{
    Task InitializeAsync(CancellationToken cancellationToken);
}