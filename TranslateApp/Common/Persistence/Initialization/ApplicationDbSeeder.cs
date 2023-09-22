using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TranslateApp.Data;
using TranslateApp.Helpers;
using TranslateApp.Models;

namespace TranslateApp.Common.Persistence.Initialization;

internal class ApplicationDbSeeder
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly CustomSeederRunner _seederRunner;
    private readonly ILogger<ApplicationDbSeeder> _logger;

    public ApplicationDbSeeder(
        RoleManager<AppRole> roleManager,
        UserManager<AppUser> userManager,
        CustomSeederRunner seederRunner,
        ILogger<ApplicationDbSeeder> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _seederRunner = seederRunner;
        _logger = logger;
    }

    public async Task SeedDatabaseAsync(TranslateDbContext dbContext, CancellationToken cancellationToken)
    {
        await SeedRolesAsync(dbContext, cancellationToken);
        await SeedAdminUserAsync();
        await _seederRunner.RunSeedersAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(TranslateDbContext dbContext, CancellationToken cancellationToken)
    {
        foreach (string roleName in ADSRoles.DefaultRoles)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                is not AppRole role)
            {
                // Create the role
                _logger.LogInformation("Seeding {role} Role for the System.", roleName);
                role = new AppRole(roleName, $"{roleName} Role for the system ");
                await _roleManager.CreateAsync(role);
            }
        }
    }
    
    private async Task SeedAdminUserAsync()
    {
        if (await _userManager.Users.AnyAsync())
        {
            return;
        }

        var adminUser = new AppUser
        {
            FirstName = "Paul",
            LastName = "Walker",
            Email = "admin@test.com",
            UserName = "admin",
        };

        _logger.LogInformation("Seeding Default System Administrator User.");
        var password = new PasswordHasher<AppUser>();
        adminUser.PasswordHash = password.HashPassword(adminUser, "password");
        await _userManager.CreateAsync(adminUser);

        // Assign role to user
        if (!await _userManager.IsInRoleAsync(adminUser, ADSRoles.Admin))
        {
            _logger.LogInformation("Assigning Default System Administrator Role to Admin User.");
            await _userManager.AddToRoleAsync(adminUser, ADSRoles.Admin);
        }
    }
}