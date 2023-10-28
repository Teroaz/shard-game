using Moq;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;

namespace Shard.IntegrationTests.Units;

public class UnitsRepositoryTests
{
    
    private readonly Mock<ISystemsService> _mockSystemsService;
    
    public UnitsRepositoryTests()
    {
        _mockSystemsService = new Mock<ISystemsService>();
        _mockSystemsService.Setup(m => m.GetRandomSystem()); 
    }
    
    [Fact]
    public void AddUnit_ShouldAddUnitToRepository()
    {
        // Arrange
        var repository = new UnitsRepository();
        var user = new UserModel("TestUser");
        var unit = new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);

        // Act
        repository.AddUnit(user, unit);

        // Assert
        var retrievedUnit = repository.GetUnitByIdAndUser(user, "TestUnit");
        Assert.Equal(unit, retrievedUnit);
    }

    [Fact]
    public void GetUnitsByUser_ShouldReturnAllUnitsForUser()
    {
        // Arrange
        var repository = new UnitsRepository();
        var user = new UserModel("TestUser");
        var unit1 = new UnitModel("TestUnit1", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);
        var unit2 = new UnitModel("TestUnit2", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);
        repository.AddUnit(user, unit1);
        repository.AddUnit(user, unit2);

        // Act
        var units = repository.GetUnitsByUser(user);

        // Assert
        Assert.Equal(2, units.Count);
        Assert.Contains(unit1, units);
        Assert.Contains(unit2, units);
    }

    [Fact]
    public void RemoveUnit_ShouldRemoveUnitFromRepository()
    {
        // Arrange
        var repository = new UnitsRepository();
        var user = new UserModel("TestUser");
        var unit = new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);
        repository.AddUnit(user, unit);

        // Act
        repository.RemoveUnit(user, unit);

        // Assert
        var retrievedUnit = repository.GetUnitByIdAndUser(user, "TestUnit");
        Assert.Null(retrievedUnit);
    }

    [Fact]
    public void UpdateUnit_ShouldUpdateUnitInRepository()
    {
        // Arrange
        var repository = new UnitsRepository();
        var user = new UserModel("TestUser");
        var unit = new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);
        repository.AddUnit(user, unit);

        // Act
        var updatedUnit = new UnitModel("TestUnit", UnitType.Builder, _mockSystemsService.Object.GetRandomSystem()!, null);
        repository.UpdateUnit(user, updatedUnit);

        // Assert
        var retrievedUnit = repository.GetUnitByIdAndUser(user, "TestUnit");
        Assert.Equal(UnitType.Builder, retrievedUnit?.Type);
    }
}