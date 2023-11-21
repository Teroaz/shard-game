using System.Text.RegularExpressions;

namespace Shard.Web.ImplementationAPI.Utils;

public class Common : ICommon
{
    public bool IsIdConsistant(string id, string r)
    {
        var regex = new Regex(r);

        return regex.IsMatch(id) && !string.IsNullOrWhiteSpace(id);
    }
}