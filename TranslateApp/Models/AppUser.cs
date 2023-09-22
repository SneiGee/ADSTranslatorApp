using Microsoft.AspNetCore.Identity;

namespace TranslateApp.Models;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}