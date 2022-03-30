namespace ElancoGroupB.Models;

public class Rebate
{
    private Dictionary<string, string> _rebatelist;
    public Rebate()
    {
        _rebatelist = new Dictionary<string, string>()
        {
            {"credellolotilaner","CRD2022"},            
            {"credellocatlotilaner","CAT2022"},
            {"interceptor","INT2022"},
            {"trifexis","TFX2022"},
            {"seresto","SER2022"},
            {"advantagemulti","MULTI2022"},
            {"galliprant","GPT2022"},
            {"deramaxx","DMX2022"},
            {"quellin","QUE2022"},
            {"elura","ELU2022"},
            {"entyce","ENT2022"},
            {"alenza","DVM2022"},
            {"freeform","DVM2022"},
            {"synovig3","DVM2022"},
            {"chlorhexiderm","DVM2022"},
            {"hylyt","DVM2022"},
            {"synovig4","DVM2022"},
            {"dvmdailysoftchews","DVM2022"},
            {"lactoquil","DVM2022"},
            {"t8keto","DVM2022"},
            {"dvmfelinejointgel","DVM2022"},
            {"otirinse","DVM2022"},
            {"endurosyn","DVM2022"},
            {"relief","DVM2022"},

        };
    }
    public string getRebate(string productName)
    {
        if (_rebatelist.ContainsKey(productName.ToLower()))
        {
            return _rebatelist[productName];
        }

        return null;
    }
}