using TranslateApp.ViewModels.Response;

namespace TranslateApp.Interfaces;

public interface IUsersRepository
{
    Task<List<UserInRoleModel>> GetAllUsersAsync();
}
