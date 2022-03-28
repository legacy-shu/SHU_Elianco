namespace ElancoGroupB.Models;

public class Clinic
{
    public Dictionary<string, string?> Address { get; set; }
    public Dictionary<string, string?> Zip { get; set; }
    public Dictionary<string, string?> Phone { get; set; }
    public Dictionary<string, string?> Name { get; set; }
}