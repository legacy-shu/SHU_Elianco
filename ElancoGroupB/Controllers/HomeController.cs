using System.Diagnostics;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ElancoGroupB.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;

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
    public async Task<IActionResult> Index(UserViewModel model)
    {
        
        if (model.Photo == null)
        {
            return View();
        }
        
        // Combines two strings into a path.
        var filePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")).Root + $@"\{model.Photo.FileName}";
        using (FileStream fs = System.IO.File.Create(filePath))
        {
            model.Photo.CopyTo(fs);
            fs.Flush();
        }
        
        var service = new Service(_apiKey, _endpoint);
        var viewModel = new UserViewModel();

        if (model.photoType == "receipt")
        {
            var receiptCustomModel1 = await service.RequestCustom1ModelAsync(filePath);
            var items = await service.RequestReceiptModelAsync(filePath);
            viewModel.Purchase = receiptCustomModel1;
            viewModel.Products = items;
            
            if (receiptCustomModel1.dataCount == 8)
            {
                _notyf.Success("It has been succeed extracting data",3);
            }
            else if (receiptCustomModel1.dataCount == 0)
            {
                _notyf.Error("It has been failed extracting data",3);
            }
            else
            {
                _notyf.Custom("It has been failed extracting data",3, backgroundColor:"yellow");
            }
            
            // Console.WriteLine(receiptCustomModel1.ClinicName?["value"] + ":" + receiptCustomModel1.ClinicName?["confidence"]);
            // Console.WriteLine(receiptCustomModel1.Zip?["value"] + ":" + receiptCustomModel1.Zip?["confidence"]);
            // Console.WriteLine(receiptCustomModel1.Address?["value"] + ":" + receiptCustomModel1.Address?["confidence"]);
            // Console.WriteLine(receiptCustomModel1.Date?["value"] + ":" + receiptCustomModel1.Date?["confidence"]);
            // Console.WriteLine(receiptCustomModel1.TotalAmount?["value"] + ":" + receiptCustomModel1.TotalAmount?["confidence"]);
            // Console.WriteLine(receiptCustomModel1.PetName?["value"] + ":" + receiptCustomModel1.PetName?["confidence"]);
            // Console.WriteLine(receiptCustomModel1.Phone?["value"] + ":" + receiptCustomModel1.Phone?["confidence"]);
            // Console.WriteLine(receiptCustomModel1.InvoiceNumber["value"] + ":" + receiptCustomModel1.InvoiceNumber["confidence"]);
            // Console.WriteLine("DataCount: " + receiptCustomModel1.dataCount);

            foreach (var item in items)
            {
                string? code = null;
                if (item.Description?["value"].Length > 5)
                {
                    code = new Rebate().getRebateCode(item.Description?["value"]);
                }

                item.Code = code;
            }
        }
        else
        {
            var product = await service.RequestproductModelAsync(filePath);
            
            if (product.Name?["value"].Length > 4)
            {
                product.Code = new Rebate().getRebateCode(product.Name["value"]);
                if (product.Code != null)
                {
                    _notyf.Success("Here you go",3);
                }
                else
                {
                    _notyf.Custom("There is no rebate offer for you",3, backgroundColor:"yellow");
                }
            }
            else
            {
                _notyf.Error("It has been failed",3);
            }
            viewModel.Product = product;
            Console.WriteLine($"CODE:{product.Code}");
        }
        
        System.IO.File.Delete(filePath);
        
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