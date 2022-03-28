namespace ElancoGroupB.Models;

public class Purchase
{
    public Clinic Clinic { get; set; } = new Clinic();
    public Pet Pet { get; set; } = new Pet();
    public Dictionary<string, string?> TotalAmount { get; set; }
    public Dictionary<string, string?> InvoiceNumber { get; set; }
    public Dictionary<string, string?> Date { get; set; }

    public int dataCount { get; set; }

}