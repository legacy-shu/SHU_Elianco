using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ElancoGroupB.Models;

public class UserViewModel
{
    [Required]
    public IFormFile Photo { get; set; }
    
    [BindNever]
    public Purchase Purchase { get; set; }
}