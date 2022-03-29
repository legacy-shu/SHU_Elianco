namespace ElancoGroupB.Models;

public class Purchase
{
    
    public Dictionary<string, string>? Address { get; set; }
    public Dictionary<string, string>? Zip { get; set; }
    public Dictionary<string, string>? Phone { get; set; }
    public Dictionary<string, string>? ClinicName { get; set; }
    public Dictionary<string, string>? PetName { get; set; }
    public Dictionary<string, string>? TotalAmount { get; set; }
    public Dictionary<string, string>? InvoiceNumber { get; set; }
    public Dictionary<string, string>? Date { get; set; }
    public int dataCount { get; set; }

}