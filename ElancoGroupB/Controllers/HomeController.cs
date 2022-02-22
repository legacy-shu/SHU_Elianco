using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ElancoGroupB.Models;
using HttpClient = System.Net.Http.HttpClient;

namespace ElancoGroupB.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string imagepath)
    {
        var extractedModel =  await Service.RequestAnalyzeDocumentAsync(imagepath);
        return View(extractedModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
    
    
}