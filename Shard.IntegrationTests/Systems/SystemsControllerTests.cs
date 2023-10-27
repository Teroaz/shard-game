using Microsoft.AspNetCore.Mvc;
using Moq;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Systems.DTOs;

namespace Shard.IntegrationTests.Systems;

public class SystemsControllerTests
{
    private readonly Mock<ISystemsService> _mockService;
    private readonly SystemsController _controller;
    private readonly MapGenerator _mapGenerator;
    private const string TestSeed = "testSeed";

    public SystemsControllerTests()
    {
        _mockService = new Mock<ISystemsService>();
        _controller = new SystemsController(_mockService.Object);
        _mapGenerator = new MapGenerator(new MapGeneratorOptions { Seed = TestSeed });
    }

    [Fact]
    public void Get_ReturnsAllSystems()
    {
        var sector = _mapGenerator.Generate();
        var systemModels = sector.Systems.Select(s => new SystemModel(s)).ToList();
        _mockService.Setup(service => service.GetAllSystems()).Returns(systemModels);

        var result = _controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedSystems = Assert.IsType<List<SystemDto>>(okResult.Value);
        Assert.Equal(systemModels.Count, returnedSystems.Count);

        Assert.NotNull(returnedSystems[0].Planets);
    }

    [Fact]
    public void Get_WithSystemName_ReturnsCorrectSystem()
    {
        var sector = _mapGenerator.Generate();
        var targetSystem = sector.Systems[0];
        _mockService.Setup(service => service.GetSystem(targetSystem.Name)).Returns(new SystemModel(targetSystem));

        var result = _controller.Get(targetSystem.Name);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedSystem = Assert.IsType<SystemDto>(okResult.Value);
        Assert.Equal(targetSystem.Name, returnedSystem.Name);
    }

    [Fact]
    public void Get_WithInvalidSystemName_ReturnsNotFound()
    {
        _mockService.Setup(service => service.GetSystem(It.IsAny<string>())).Returns((SystemModel)null);

        var result = _controller.Get("InvalidSystemName");

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void GetPlanets_WithValidSystemName_ReturnsPlanets()
    {
        var sector = _mapGenerator.Generate();
        var targetSystem = sector.Systems[0];
        _mockService.Setup(service => service.GetSystem(targetSystem.Name)).Returns(new SystemModel(targetSystem));

        var result = _controller.GetPlanets(targetSystem.Name);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPlanets = Assert.IsType<List<PlanetDto>>(okResult.Value);

        Assert.Equal(targetSystem.Planets.Count, returnedPlanets.Count);
        Assert.NotNull(returnedPlanets[0].Size);
    }

    [Fact]
    public void GetPlanets_WithInvalidSystemName_ReturnsNotFound()
    {
        _mockService.Setup(service => service.GetSystem(It.IsAny<string>())).Returns((SystemModel)null);

        var result = _controller.GetPlanets("InvalidSystemName");

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void GetPlanet_WithValidSystemAndPlanetName_ReturnsPlanet()
    {
        var sector = _mapGenerator.Generate();
        var targetSystem = sector.Systems[0];
        var targetPlanet = targetSystem.Planets[0];
        _mockService.Setup(service => service.GetSystem(targetSystem.Name)).Returns(new SystemModel(targetSystem));

        var result = _controller.GetPlanet(targetSystem.Name, targetPlanet.Name);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPlanet = Assert.IsType<PlanetDto>(okResult.Value);
        Assert.Equal(targetPlanet.Name, returnedPlanet.Name);
    }

    [Fact]
    public void GetPlanet_WithInvalidSystemName_ReturnsNotFound()
    {
        _mockService.Setup(service => service.GetSystem(It.IsAny<string>())).Returns((SystemModel)null);

        var result = _controller.GetPlanet("InvalidSystemName", "ValidPlanetName");

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void GetPlanet_WithValidSystemAndInvalidPlanetName_ReturnsNotFound()
    {
        var sector = _mapGenerator.Generate();
        var targetSystem = sector.Systems[0];
        _mockService.Setup(service => service.GetSystem(targetSystem.Name)).Returns(new SystemModel(targetSystem));

        var result = _controller.GetPlanet(targetSystem.Name, "InvalidPlanetName");

        Assert.IsType<NotFoundResult>(result.Result);
    }
}