using System.Text.RegularExpressions;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.IntegrationTests.Utils;

public class CommonTests
{
    private readonly ICommon _common = new Common();

    [Fact]
    public void IsIdConsistant_MatchingRegex_ReturnsTrue()
    {
        const string id = "123";
        const string regexPattern = @"^\d+$"; // match numbers only

        var result = _common.IsIdConsistant(id, regexPattern);

        Assert.True(result);
    }

    [Fact]
    public void IsIdConsistant_NotMatchingRegex_ReturnsFalse()
    {
        const string id = "ABC";
        const string regexPattern = @"^\d+$"; // match numbers only

        var result = _common.IsIdConsistant(id, regexPattern);

        Assert.False(result);
    }

    [Fact]
    public void IsIdConsistant_EmptyId_ReturnsFalse()
    {
        const string id = "";
        const string regexPattern = @"^\d+$"; // match numbers only

        var result = _common.IsIdConsistant(id, regexPattern);

        Assert.False(result);
    }

    [Fact]
    public void IsIdConsistant_InvalidRegexPattern_ThrowsException()
    {
        const string id = "123";
        const string invalidRegexPattern = @"^(\d+$"; // Invalid regex pattern

        Assert.Throws<RegexParseException>(() => _common.IsIdConsistant(id, invalidRegexPattern));
    }
}