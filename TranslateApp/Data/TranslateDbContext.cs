using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TranslateApp.Models;

namespace TranslateApp.Data;

public class TranslateDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public TranslateDbContext(DbContextOptions<TranslateDbContext> options)
        : base(options)
    {
    }

    public DbSet<Translation>? Translations { get; set; }
}