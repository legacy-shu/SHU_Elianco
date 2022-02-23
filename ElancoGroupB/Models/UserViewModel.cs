using System.ComponentModel.DataAnnotations;

namespace ElancoGroupB.Models;

public class UserViewModel
{
    [Required(ErrorMessage = "Please select a file.")]
    [DataType(DataType.Upload)]
    public IFormFile Photo { get; set; }
    public Purchase Purchase { get; set; }
}