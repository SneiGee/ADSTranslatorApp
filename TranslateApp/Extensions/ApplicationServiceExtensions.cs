using TranslateApp.BackgroundJobs;
using TranslateApp.Helpers;
using TranslateApp.Interfaces;
using TranslateApp.Repository;
using TranslateApp.Services;

namespace TranslateApp.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient();
        services.AddBackgroundJobs(config);
        services.Configure<TranslationApiConfig>(config.GetSection("TranslationApiConfig"));
        services.AddSingleton(config.GetSection("TranslationApiConfig").Get<TranslationApiConfig>()!);
        services.AddScoped<ITranslateRepository, TranslateRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<ITranslationService, TranslationService>();
        return services;
    }
}