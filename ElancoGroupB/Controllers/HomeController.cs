using System.Diagnostics;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ElancoGroupB.Models;
using Microsoft.Extensions.FileProviders;

namespace ElancoGroupB.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;
    private readonly string _endpoint;
    private readonly ILogger<HomeController> _logger;
    private readonly INotyfService _notyf;
    public HomeController(ILogger<HomeController> logger, INotyfService notyf, IConfiguration configuration)
    {
        _notyf = notyf;
        _logger = logger;
        _configuration = configuration;
        _apiKey = _configuration["API:apiKey"];
        _endpoint = _configuration["API:endpoint"];
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
        
        using (FileStream fs = System.IO.File.Create(filepath))
        {
            model.Photo.CopyTo(fs);
            fs.Flush();
        }

        var service = new Service(_apiKey, _endpoint);
        
        var receiptCustomModel1 = await service.RequestCustom1ModelAsync(filepath);
        var items = await service.RequestReceiptModelAsync(filepath);
        
        System.IO.File.Delete(filepath);
        
        // Console.WriteLine($"Clinic Name: '{extractedModel.Clinic.Name["value"]}' confidence: {extractedModel.Clinic.Name["confidence"]} ");
        // Console.WriteLine($"Clinic Address: '{extractedModel.Clinic.Address["value"]}' confidence: {extractedModel.Clinic.Name["confidence"]} ");
        // Console.WriteLine($"Clinic Phone: '{extractedModel.Clinic.Phone["value"]}' confidence: {extractedModel.Clinic.Name["confidence"]} ");
        // Console.WriteLine($"Clinic Zip: '{extractedModel.Clinic.Zip["value"]}' confidence: {extractedModel.Clinic.Name["confidence"]} ");
        // Console.WriteLine($"Pet Name: '{extractedModel.Pet.Name["value"]}' confidence: {extractedModel.Clinic.Name["confidence"]} ");
        // Console.WriteLine($"Invoice: '{extractedModel.InvoiceNumber["value"]}' confidence: {extractedModel.Clinic.Name["confidence"]} ");
        // Console.WriteLine($"Total Amount: '{extractedModel.TotalAmount["value"]}' confidence: {extractedModel.Clinic.Name["confidence"]} ");
        // Console.WriteLine($"Date: '{extractedModel.Date["value"]}' confidence: {extractedModel.Clinic.Name["confidence"]} ");
        // Console.WriteLine(extractedModel.dataCount);

        var viewModel = new UserViewModel();
        viewModel.Purchase = receiptCustomModel1;
        viewModel.Products = items;
        
        switch (receiptCustomModel1.dataCount)
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