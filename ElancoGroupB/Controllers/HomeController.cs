using System.Diagnostics;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ElancoGroupB.Models;
using Microsoft.Extensions.FileProviders;

namespace ElancoGroupB.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly INotyfService _notyf;
    public HomeController(ILogger<HomeController> logger, INotyfService notyf)
    {
        _notyf = notyf;
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
        Console.WriteLine(extractedModel.dataCount);

        var viewModel = new UserViewModel();
        viewModel.Purchase = extractedModel;
        
        switch (extractedModel.dataCount)
        {
            case 0:
                _notyf.Error("It has been failed extracting data",3);
                break;
            case 8:
                _notyf.Success("It has been succeed extracting data",3);
                break;
            default:
                _notyf.Warning("It's been succeed but some was missing",3);
                break;
        }

        return View(viewModel);
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