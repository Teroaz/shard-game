using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Units;

namespace Shard.IntegrationTests.Units;

public class UnitsRepositoryTests
{
    private readonly UnitsRepository _repository = new();

    [Fact]
    public void GetUnitByIdAndUser_WhenUnitDoesNotExist_ReturnsNull()
    {
        var result = _repository.GetUnitByIdAndUser("nonexistentId", "testUserId");

        Assert.Null(result);
    }

    [Fact]
    public void GetUnitByIdAndUser_WhenUnitExistsForGivenUser_ReturnsUnit()
    {
        var unit = new UnitModel("testUnitId", "scout", "testSystemId", "testPlanetId", "testUserId");
        _repository.AddUnit(unit);

        var result = _repository.GetUnitByIdAndUser("testUnitId", "testUserId");

        Assert.NotNull(result);
        Assert.Equal(unit, result);
    }

    [Fact]
    public void AddUnit_ShouldAddUnitToList()
    {
        var unit = new UnitModel("newTestUnitId", "warrior", "testSystemId2", "testPlanetId2", "testUserId2");
        _repository.AddUnit(unit);

        var result = _repository.GetUnitByIdAndUser("newTestUnitId", "testUserId2");

        Assert.NotNull(result);
        Assert.Equal(unit, result);
    }

    [Fact]
    public void GetUnitsByUser_ReturnsAllUnitsForGivenUser()
    {
        var unit1 = new UnitModel("unitId1", "scout", "testSystemId", "testPlanetId", "testUserId3");
        var unit2 = new UnitModel("unitId2", "warrior", "testSystemId", "testPlanetId", "testUserId3");
        _repository.AddUnit(unit1);
        _repository.AddUnit(unit2);

        var result = _repository.GetUnitsByUser("testUserId3");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(unit1, result);
        Assert.Contains(unit2, result);
    }
}