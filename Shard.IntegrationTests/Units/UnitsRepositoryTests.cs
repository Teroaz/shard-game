using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Units;

namespace Shard.IntegrationTests.Units;

public class UnitsRepositoryTests
{
    private readonly UnitsRepository _repository = new();

    // [Fact]
    // public void GetUnitByIdAndUser_WhenUnitDoesNotExist_ReturnsNull()
    // {
    //     var user = new UserModel("testUserId", "testUsername");
    //     var result = _repository.GetUnitByIdAndUser(user, "testUserId");
    //
    //     Assert.Null(result);
    // }
    //
    // [Fact]
    // public void GetUnitByIdAndUser_WhenUnitExistsForGivenUser_ReturnsUnit()
    // {
    //     var user = new UserModel("testUserId", "testUsername");
    //     var unit = new UnitModel("testUnitId", "scout", "testSystemId", "testPlanetId");
    //     _repository.AddUnit(user, unit);
    //
    //     var result = _repository.GetUnitByIdAndUser(user, "testUserId");
    //
    //     Assert.NotNull(result);
    //     Assert.Equal(unit, result);
    // }
    //
    // [Fact]
    // public void AddUnit_ShouldAddUnitToList()
    // {
    //     var user = new UserModel("testUserId", "testUsername");
    //     var unit = new UnitModel("newTestUnitId", "warrior", "testSystemId2", "testPlanetId2");
    //     _repository.AddUnit(user, unit);
    //
    //     var result = _repository.GetUnitByIdAndUser(user, "testUserId2");
    //
    //     Assert.NotNull(result);
    //     Assert.Equal(unit, result);
    // }
    //
    // [Fact]
    // public void GetUnitsByUser_ReturnsAllUnitsForGivenUser()
    // {
    //     var user = new UserModel("testUserId", "testUsername");
    //     var unit1 = new UnitModel("unitId1", "scout", "testSystemId", "testPlanetId");
    //     var unit2 = new UnitModel("unitId2", "warrior", "testSystemId", "testPlanetId");
    //     _repository.AddUnit(user, unit1);
    //     _repository.AddUnit(user, unit2);
    //
    //     var result = _repository.GetUnitsByUser(user);
    //
    //     Assert.NotNull(result);
    //     Assert.Equal(2, result.Count);
    //     Assert.Contains(unit1, result);
    //     Assert.Contains(unit2, result);
    // }
}