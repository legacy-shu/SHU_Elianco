using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ElancoGroupB.Models;

public class UserViewModel
{
    [Required]
    public IFormFile? Photo { get; set; }
    
    public Purchase Purchase { get; set; }

    public IEnumerable<Item> Products { get; set; }

    public Product Product { get; set; }

    public string photoType { get; set; }

}