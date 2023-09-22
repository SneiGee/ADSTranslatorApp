namespace TranslateApp.Common.Persistence.ConnectionString;

public interface IConnectionStringValidator
{
    bool TryValidate(string connectionString, string? dbProvider = null);
}