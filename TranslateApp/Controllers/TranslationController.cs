using Microsoft.AspNetCore.Mvc;
using TranslateApp.Data;
using TranslateApp.Interfaces;
using TranslateApp.Models;
using TranslateApp.ViewModels.Request;

namespace TranslateApp.Controllers;

public class TranslationController : Controller
{
    private readonly ITranslateRepository _translateRepository;
    public TranslationController(ITranslateRepository translateRepository)
    {
        _translateRepository = translateRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // load data without refreshing the page
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            // This is an AJAX request, return JSON data
            var data = await _translateRepository.GetAll();
            return Json(new { data });
        }

        // This is a normal request, return the PartialView
        return View();
    }

}