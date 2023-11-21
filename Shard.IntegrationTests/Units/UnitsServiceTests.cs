using Moq;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.IntegrationTests.Units;

public class UnitsServiceTests
{
    private readonly UnitsService _service;
    private readonly Mock<IUnitsRepository> _mockUnitsRepo;
    private readonly Mock<ISystemsService> _mockSystemsService;
    private readonly Mock<ICommon> _mockCommon;
    private const string TestSeed = "testSeed";
    private readonly SystemSpecification systemSpecification;

    public UnitsServiceTests()
    {
        _mockUnitsRepo = new Mock<IUnitsRepository>();
        _mockSystemsService = new Mock<ISystemsService>();
        _mockCommon = new Mock<ICommon>();
        var options = new MapGeneratorOptions { Seed = TestSeed };
        var mapGenerator = new MapGenerator(options);
        systemSpecification = mapGenerator.Generate().Systems[0];
        _service = new UnitsService(_mockUnitsRepo.Object, _mockCommon.Object, _mockSystemsService.Object);
    }

    [Fact]
    public void GetUnitByIdAndUser_ReturnsCorrectUnit()
    {
        var unit = new UnitModel("testUnitId", "scout", "testSystemId", "testPlanetId", "testUserId");
        _mockUnitsRepo.Setup(r => r.GetUnitByIdAndUser("testUnitId", "testUserId")).Returns(unit);

        var result = _service.GetUnitByIdAndUser("testUnitId", "testUserId");

        Assert.NotNull(result);
        Assert.Equal(unit, result);
    }

    /*[Fact]
    public void CreateUpdateUnits_CreatesNewUnit_WhenNotExisting()
    {
        // Simulating non-existence of the unit and the system
        _mockUnitsRepo.Setup(r => r.GetUnitByIdAndUser(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((UnitModel)null);
        _mockSystemsService.Setup(s => s.GetSystem(It.IsAny<string>())).Returns((SystemModel)null);

        _mockSystemsService.Setup(s => s.GetAllSystems()).Returns(new List<SystemModel>
        {
            new(systemSpecification)
        });

        var unitsBodyDto = new UnitsBodyDto { Id = "testUnitId", Type = "scout", System = "nonexistentSystem" };

        var result = _service.CreateUpdateUnits("testUnitId", "testUserId", unitsBodyDto);

        Assert.NotNull(result);
        Assert.Equal("testUnitId", result.Id);
    }*/

    [Fact]
    public void CreateUpdateUnits_UpdatesExistingUnit()
    {
        var unit = new UnitModel("testUnitId", "scout", "testSystemId", "testPlanetId", "testUserId");
        _mockUnitsRepo.Setup(r => r.GetUnitByIdAndUser("testUnitId", "testUserId")).Returns(unit);

        var unitsBodyDto = new UnitsBodyDto { Id = "testUnitId", Type = "warrior", System = "newSystemId" };

        var result = _service.CreateUpdateUnits("testUnitId", "testUserId", unitsBodyDto);

        Assert.NotNull(result);
        Assert.Equal("warrior", result.Type);
    }

    [Fact]
    public void CreateUpdateUnits_UnitDoesNotExist_SystemAndPlanetSpecified_ShouldCreateUnit()
    {
        var mockUnitsRepository = new Mock<IUnitsRepository>();
        mockUnitsRepository.Setup(r => r.GetUnitByIdAndUser(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((UnitModel)null);

        var mockSystemModel = new SystemModel(systemSpecification);
        var mockSystemsService = new Mock<ISystemsService>();
        mockSystemsService.Setup(s => s.GetSystem(It.IsAny<string>())).Returns(mockSystemModel);

        var mockCommon = new Mock<ICommon>();

        var service = new UnitsService(mockUnitsRepository.Object, mockCommon.Object, mockSystemsService.Object);

        var result = service.CreateUpdateUnits("testId", "testUserId",
            new UnitsBodyDto { Type = "type", System = "system", Planet = "planet" });

        Assert.NotNull(result);
        Assert.Equal("type", result.Type);
        Assert.Equal("system", result.System);
        Assert.Equal("planet", result.Planet);
    }

    [Fact]
    public void CreateUpdateUnits_UnitDoesNotExist_OnlySystemSpecified_ShouldCreateUnitWithRandomPlanet()
    {
        var mockUnitsRepository = new Mock<IUnitsRepository>();
        mockUnitsRepository.Setup(r => r.GetUnitByIdAndUser(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((UnitModel)null);

        var mockSystemModel = new SystemModel(systemSpecification);
        var mockSystemsService = new Mock<ISystemsService>();
        mockSystemsService.Setup(s => s.GetSystem(It.IsAny<string>())).Returns(mockSystemModel);

        var mockCommon = new Mock<ICommon>();

        var service = new UnitsService(mockUnitsRepository.Object, mockCommon.Object, mockSystemsService.Object);

        var result = service.CreateUpdateUnits("testId", "testUserId",
            new UnitsBodyDto { Type = "type", System = "system" });

        Assert.NotNull(result);
        Assert.Equal("type", result.Type);
        Assert.Equal("system", result.System);
    }

    /*[Fact]
    public void CreateUpdateUnits_UnitDoesNotExist_NoSystemOrPlanetSpecified_ShouldCreateUnitWithRandomSystemAndPlanet()
    {
        var mockUnitsRepository = new Mock<IUnitsRepository>();
        mockUnitsRepository.Setup(r => r.GetUnitByIdAndUser(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((UnitModel)null);

        var systemsList = new List<SystemModel>
        {
            new(systemSpecification)
        };
        var mockSystemsService = new Mock<ISystemsService>();
        mockSystemsService.Setup(s => s.GetSystem(It.IsAny<string>())).Returns((SystemModel)null);
        mockSystemsService.Setup(s => s.GetAllSystems()).Returns(systemsList);

        var mockCommon = new Mock<ICommon>();

        var service = new UnitsService(mockUnitsRepository.Object, mockCommon.Object, mockSystemsService.Object);

        var result = service.CreateUpdateUnits("testId", "testUserId", new UnitsBodyDto { Type = "type" });

        Assert.NotNull(result);
        Assert.Equal("type", result.Type);
        Assert.Equal("system", result.System);
    }*/

    [Fact]
    public void CreateUpdateUnits_UnitExists_ShouldUpdateUnit()
    {
        var existingUnit = new UnitModel("testId", "type", "system", "planet", "testUserId");
        var mockUnitsRepository = new Mock<IUnitsRepository>();
        mockUnitsRepository.Setup(r => r.GetUnitByIdAndUser(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(existingUnit);

        var mockCommon = new Mock<ICommon>();
        var mockSystemsService = new Mock<ISystemsService>();

        var service = new UnitsService(mockUnitsRepository.Object, mockCommon.Object, mockSystemsService.Object);

        var updatedUnitBody = new UnitsBodyDto
            { Id = "testId", Type = "newType", System = "newSystem", Planet = "newPlanet" };
        var result = service.CreateUpdateUnits("testId", "testUserId", updatedUnitBody);

        Assert.NotNull(result);
        Assert.Equal("newType", result.Type);
        Assert.Equal("newSystem", result.System);
        Assert.Equal("newPlanet", result.Planet);
    }

    /*[Fact]
    public void CreateUpdateUnits_SystemNotPresent_ShouldFetchRandomSystem()
    {
        var mockUnitsRepository = new Mock<IUnitsRepository>();
        mockUnitsRepository.Setup(r => r.GetUnitByIdAndUser(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((UnitModel)null);

        var systemsList = new List<SystemModel>
        {
            new(systemSpecification)
        };
        var mockSystemsService = new Mock<ISystemsService>();
        mockSystemsService.Setup(s => s.GetSystem(It.IsAny<string>())).Returns((SystemModel)null);
        mockSystemsService.Setup(s => s.GetAllSystems()).Returns(systemsList);

        var mockCommon = new Mock<ICommon>();

        var service = new UnitsService(mockUnitsRepository.Object, mockCommon.Object, mockSystemsService.Object);

        var result = service.CreateUpdateUnits("testId", "testUserId",
            new UnitsBodyDto { Type = "type", System = "nonExistentSystem" });

        Assert.NotNull(result);
        Assert.Equal("type", result.Type);
        Assert.Equal("system", result.System);
        Assert.Equal("planet", result.Planet);
    }
    */


    [Fact]
    public void IsBodyValid_UnitsBodyDtoIsNull_ReturnsFalse()
    {
        var mockCommon = new Mock<ICommon>();
        var service = new UnitsService(null, mockCommon.Object, null);

        var result = service.IsBodyValid("testId", "testUserId", null);

        Assert.False(result);
    }

    [Fact]
    public void IsBodyValid_IdMismatch_ReturnsFalse()
    {
        var mockCommon = new Mock<ICommon>();
        var service = new UnitsService(null, mockCommon.Object, null);

        var body = new UnitsBodyDto { Id = "differentId", Type = "type", System = "system" };
        var result = service.IsBodyValid("testId", "testUserId", body);

        Assert.False(result);
    }

    [Fact]
    public void IsBodyValid_TypeIsNullOrWhitespace_ReturnsFalse()
    {
        var mockCommon = new Mock<ICommon>();
        var service = new UnitsService(null, mockCommon.Object, null);

        var body = new UnitsBodyDto { Id = "testId", Type = "", System = "system" };
        var result = service.IsBodyValid("testId", "testUserId", body);

        Assert.False(result);
    }

    [Fact]
    public void IsBodyValid_SystemIsNullOrWhitespace_ReturnsFalse()
    {
        var mockCommon = new Mock<ICommon>();
        var service = new UnitsService(null, mockCommon.Object, null);

        var body = new UnitsBodyDto { Id = "testId", Type = "type", System = "" };
        var result = service.IsBodyValid("testId", "testUserId", body);

        Assert.False(result);
    }

    [Fact]
    public void IsBodyValid_IdIsNotConsistent_ReturnsFalse()
    {
        var mockCommon = new Mock<ICommon>();
        mockCommon.Setup(c => c.IsIdConsistant(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        var service = new UnitsService(null, mockCommon.Object, null);

        var body = new UnitsBodyDto { Id = "testId@", Type = "type", System = "system" };
        var result = service.IsBodyValid("testId@", "testUserId", body);

        Assert.False(result);
    }

    [Fact]
    public void IsBodyValid_AllParametersAreValid_ReturnsTrue()
    {
        var mockCommon = new Mock<ICommon>();
        mockCommon.Setup(c => c.IsIdConsistant(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var service = new UnitsService(null, mockCommon.Object, null);

        var body = new UnitsBodyDto { Id = "testId", Type = "type", System = "system" };
        var result = service.IsBodyValid("testId", "testUserId", body);

        Assert.True(result);
    }

    [Fact]
    public void GetUnitsByUser_ReturnsAllUnitsForGivenUser()
    {
        var units = new List<UnitModel>
        {
            new UnitModel("unitId1", "scout", "testSystemId", "testPlanetId", "testUserId3"),
            new UnitModel("unitId2", "warrior", "testSystemId", "testPlanetId", "testUserId3")
        };
        _mockUnitsRepo.Setup(r => r.GetUnitsByUser("testUserId3")).Returns(units);

        var result = _service.GetUnitsByUser("testUserId3");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}