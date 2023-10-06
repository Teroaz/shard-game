using System.Text.RegularExpressions;

namespace Shard.Web.ImplementationAPI.Services;

public class Common : ICommon
{
    public Boolean IsIdConsistant(string id, string r)
    {
        var regex = new Regex(r);
        
        return regex.IsMatch(id) && !string.IsNullOrWhiteSpace(id);
    }
}