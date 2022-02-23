using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ElancoGroupB.Models;

namespace ElancoGroupB.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment Environment;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment e)
    {
        _logger = logger;
        Environment = e;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(IFormFile imagepath)
    {
        
        if (imagepath == null)
        {
            return View();
        }
        
        var fileName = Path.Combine(Environment.WebRootPath, Path.GetFileName(imagepath.FileName));
        imagepath.CopyTo(new FileStream(fileName, FileMode.Create));
        var path = "/" + Path.GetFileName(imagepath.FileName);
        Console.WriteLine(fileName);
        var extractedModel =  await Service.RequestAnalyzeDocumentAsync(fileName);
        System.IO.File.Delete(path);
        Console.WriteLine($"Clinic Name: '{extractedModel.Clinic.Name}': ");
        Console.WriteLine($"Clinic Address: '{extractedModel.Clinic.Address}': ");
        Console.WriteLine($"Clinic Phone: '{extractedModel.Clinic.Phone}': ");
        Console.WriteLine($"Clinic Zip: '{extractedModel.Clinic.Zip}': ");
        Console.WriteLine($"Pet Name: '{extractedModel.Pet.Name}': ");
        Console.WriteLine($"Invoice: '{extractedModel.InvoiceNumber}': ");
        Console.WriteLine($"Total Amount: '{extractedModel.TotalAmount}': ");
        Console.WriteLine($"Date: '{extractedModel.Date}': ");
        var model = new UserViewModel();
        model.Purchase = extractedModel;
        return View(model);
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


    public IActionResult Instructions()
    {
        return View(); 
    }
}