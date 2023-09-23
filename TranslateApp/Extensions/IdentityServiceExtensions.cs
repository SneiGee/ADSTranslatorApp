using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using TranslateApp.Common.Persistence;
using TranslateApp.Common.Persistence.Initialization;
using TranslateApp.Data;
using TranslateApp.Models;

namespace TranslateApp.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        // services.AddDbContext<TranslateDbContext>(options =>
        //     options.UseSqlite(config.GetConnectionString("DefaultConnection")));

        services.AddIdentity<AppUser, AppRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<TranslateDbContext>();
        services.AddSession();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie();

        return services
            .AddPersistence();
    }

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }
}