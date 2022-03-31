namespace ElancoGroupB.Models;

public class Rebate
{
    private Dictionary<string, string> _rebatelist;

    public Rebate()
    {
        _rebatelist = new Dictionary<string, string>()
        {
            {"AdvantageMulti","CRD2022"},
            {"Credelio","CAT2022"},
            {"CredelioCAT","INT2022"},
            {"Deramaxx","TFX2022"},
            {"DermatologyandSupplements","SER2022"},
            {"Elura","MULTI2022"},
            {"Entyce","GPT2022"},
            {"Galliprant","DMX2022"},
            {"Interceptor","QUE2022"},
            {"InterceptorPlus","ELU2022"},
            {"quellin","ENT2022"},
            {"Seresto","DVM2022"},
            {"Trifexis","DVM2022"},
        };
    }

    public string getRebateCode(string? product)
    {
        if (product == null) return null;
        var s = product.Trim().ToLower().Substring(0,5);
        List<string> keys = _rebatelist.Keys.ToList();
        foreach (string key in keys)
        {
            string _key = key;
            if (_key.Trim().ToLower().Contains(s))
            {
                return _rebatelist[key];
            }
        }
        return null;
    }
}