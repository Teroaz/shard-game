using Moq;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.IntegrationTests.Units;

public class UnitsServiceTests
{
    private readonly Mock<IUnitsRepository> _mockUnitsRepository;
    private readonly Mock<ICommon> _mockCommon;
    private readonly UnitsService _unitsService;
    private readonly Mock<ISystemsService> _mockSystemsService;

    public UnitsServiceTests()
    {
        _mockUnitsRepository = new Mock<IUnitsRepository>();
        _mockCommon = new Mock<ICommon>();
        _unitsService = new UnitsService(_mockUnitsRepository.Object, _mockCommon.Object);
        _mockSystemsService = new Mock<ISystemsService>();
        _mockSystemsService.Setup(m => m.GetRandomSystem());
    }

    [Fact]
    public void AddUnit_ShouldCallRepository()
    {
        // Arrange
        var user = new UserModel("TestUser");
        var unit = new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);

        // Act
        _unitsService.AddUnit(user, unit);

        // Assert
        _mockUnitsRepository.Verify(repo => repo.AddUnit(user, unit), Times.Once);
    }

    [Fact]
    public void IsBodyValid_ShouldReturnFalseForInvalidData()
    {
        // Arrange
        var invalidDto = new UnitsBodyDto { Id = "invalid_id", Type = null, System = null };

        // Act
        var result = _unitsService.IsBodyValid("some_id", invalidDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RemoveUnit_ShouldCallRepository()
    {
        // Arrange
        var user = new UserModel("TestUser");
        var unit = new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);

        // Act
        _unitsService.RemoveUnit(user, unit);

        // Assert
        _mockUnitsRepository.Verify(repo => repo.RemoveUnit(user, unit), Times.Once);
    }

    [Fact]
    public void UpdateUnit_ShouldCallRepository()
    {
        // Arrange
        var user = new UserModel("TestUser");
        var unit = new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);

        // Act
        _unitsService.UpdateUnit(user, unit);

        // Assert
        _mockUnitsRepository.Verify(repo => repo.UpdateUnit(user, unit), Times.Once);
    }

    [Fact]
    public void GetUnitsByUser_ShouldCallRepository()
    {
        // Arrange
        var user = new UserModel("TestUser");
        var unitList = new List<UnitModel>
            { new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null) };
        _mockUnitsRepository.Setup(repo => repo.GetUnitsByUser(user)).Returns(unitList);

        // Act
        var result = _unitsService.GetUnitsByUser(user);

        // Assert
        _mockUnitsRepository.Verify(repo => repo.GetUnitsByUser(user), Times.Once);
        Assert.Equal(unitList, result);
    }

    [Fact]
    public void GetUnitByIdAndUser_ShouldCallRepository()
    {
        // Arrange
        var user = new UserModel("TestUser");
        var unit = new UnitModel("TestUnit", UnitType.Scout, _mockSystemsService.Object.GetRandomSystem()!, null);
        _mockUnitsRepository.Setup(repo => repo.GetUnitByIdAndUser(user, "TestUnit")).Returns(unit);

        // Act
        var result = _unitsService.GetUnitByIdAndUser(user, "TestUnit");

        // Assert
        _mockUnitsRepository.Verify(repo => repo.GetUnitByIdAndUser(user, "TestUnit"), Times.Once);
        Assert.Equal(unit, result);
    }
}