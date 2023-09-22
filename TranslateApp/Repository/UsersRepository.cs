using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TranslateApp.Interfaces;
using TranslateApp.Models;
using TranslateApp.ViewModels.Response;

namespace TranslateApp.Repository;

public class UsersRepository : IUsersRepository
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public UsersRepository(RoleManager<AppRole> roleManager,
        UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<List<UserInRoleModel>> GetAllUsersAsync()
    {
        // implement and display all users with their role..
        var userList = new List<UserInRoleModel>();
        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            string roleName = userRoles.Count() == 0 ? "No role" : userRoles[0];

            userList.Add(new UserInRoleModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = roleName
            });
        }

        return userList!;
    }
}