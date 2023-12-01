using Shard.Web.ImplementationAPI.Units;

namespace Shard.IntegrationTests.Units;

public class UnitTravelTimeTests
{
    [Fact]
    public void TestTimeToChangeSystem()
    {
        var expectedTime = new TimeSpan(0, 0, 0, 60); // 60 seconds
        Assert.Equal(expectedTime, UnitTravelTime.TimeToChangeSystem);
    }

    [Fact]
    public void TestTimeToEnterPlanet()
    {
        var expectedTime = new TimeSpan(0, 0, 0, 15); // 15 seconds
        Assert.Equal(expectedTime, UnitTravelTime.TimeToEnterPlanet);
    }

    [Fact]
    public void TestTimeToLeavePlanet()
    {
        var expectedTime = new TimeSpan(0, 0, 0, 0); // 0 seconds
        Assert.Equal(expectedTime, UnitTravelTime.TimeToLeavePlanet);
    }
}