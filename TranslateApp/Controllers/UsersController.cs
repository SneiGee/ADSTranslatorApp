using Microsoft.AspNetCore.Mvc;
using TranslateApp.Interfaces;

namespace TranslateApp.Controllers;

public class UsersController : Controller
{
    private readonly IUsersRepository _usersRepository;
    public UsersController(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // retrieve all users with their role
        // load data without refreshing the page
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            // This is an AJAX request, return JSON data
            var data = await _usersRepository.GetAllUsersAsync();
            return Json(new { data });
        }

        // This is a normal request, return the PartialView
        return View();
    }
}
