using System.Collections.ObjectModel;

namespace TranslateApp.Helpers;

public static class ADSRoles
{
    public const string Admin = nameof(Admin);
    public const string Member = nameof(Member);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Admin,
        Member
    });

    public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
}