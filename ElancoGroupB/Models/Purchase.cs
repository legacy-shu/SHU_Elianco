namespace ElancoGroupB.Models;

public class Purchase
{
    public Clinic Clinic { get; set; } = new Clinic();
    public Pet Pet { get; set; } = new Pet();
    public string TotalAmount { get; set; }
    public string InvoiceNumber { get; set; }
    public string Date { get; set; }
}