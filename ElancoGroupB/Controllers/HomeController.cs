using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ElancoGroupB.Models;
using Microsoft.Extensions.FileProviders;

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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([Bind("Photo")]UserViewModel model)
    {
        
        if (model.Photo == null)
        {
            return View();
        }

        // Combines two strings into a path.
        var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")).Root + $@"\{model.Photo.FileName}";
        Console.WriteLine(filepath);
        using (FileStream fs = System.IO.File.Create(filepath))
        {
            model.Photo.CopyTo(fs);
            fs.Flush();
        }
        
        var extractedModel = await Service.RequestAnalyzeDocumentAsync(filepath);
        System.IO.File.Delete(filepath);
        Console.WriteLine($"Clinic Name: '{extractedModel.Clinic.Name}': ");
        Console.WriteLine($"Clinic Address: '{extractedModel.Clinic.Address}': ");
        Console.WriteLine($"Clinic Phone: '{extractedModel.Clinic.Phone}': ");
        Console.WriteLine($"Clinic Zip: '{extractedModel.Clinic.Zip}': ");
        Console.WriteLine($"Pet Name: '{extractedModel.Pet.Name}': ");
        Console.WriteLine($"Invoice: '{extractedModel.InvoiceNumber}': ");
        Console.WriteLine($"Total Amount: '{extractedModel.TotalAmount}': ");
        Console.WriteLine($"Date: '{extractedModel.Date}': ");
        var viewModel = new UserViewModel();
        viewModel.Purchase = extractedModel;
        return View(viewModel);
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