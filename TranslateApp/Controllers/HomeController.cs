using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TranslateApp.Data;
using TranslateApp.Interfaces;
using TranslateApp.Models;
using TranslateApp.ViewModels.Request;

namespace TranslateApp.Controllers;

public class HomeController : Controller
{
    private readonly ITranslationService _translationService;
    private readonly TranslateDbContext _context;
    private readonly ILogger<HomeController> _logger;
    public HomeController(ITranslationService translationService, TranslateDbContext context, ILogger<HomeController> logger)
    {
        _logger = logger;
        _context = context;
        _translationService = translationService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(TranslationRequestViewModel model)
    {
        if (ModelState.IsValid)
        {
            string translated = await _translationService.TranslateAsync(model.Text!);

            // Log the translation.
            var translation = new Translation
            {
                Text = model.Text,
                Translated = translated
            };

            _context.Translations!.Add(translation);
            await _context.SaveChangesAsync();

            ViewBag.Translated = translated;
            return View("Index");
        }
        else
        {
            ModelState.AddModelError("", "Error saving translation to the database.");
        }

        return View("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
