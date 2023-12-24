using Moq;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Systems.Models;

namespace Shard.IntegrationTests.Systems;

public class SystemsServiceTests
{
    private readonly MapGenerator _mapGenerator;
    private readonly Mock<ISystemsRepository> _mockRepo;
    private readonly SystemsService _service;
    private const string TestSeed = "testSeed";

    public SystemsServiceTests()
    {
        var mapGeneratorOptions = new MapGeneratorOptions { Seed = TestSeed };
        _mapGenerator = new MapGenerator(mapGeneratorOptions);
        var sectorSpecification = _mapGenerator.Generate();

        _mockRepo = new Mock<ISystemsRepository>();
        _mockRepo.Setup(r => r.GetAllSystems()).Returns(sectorSpecification.Systems.Select(s => new SystemModel(s)));
        _mockRepo.Setup(r => r.GetSystem(It.IsAny<string>())).Returns((string name) =>
            sectorSpecification.Systems.Where(s => s.Name == name).Select(s => new SystemModel(s)).FirstOrDefault());

        _service = new SystemsService(_mockRepo.Object);
    }

    [Fact]
    public void GetAllSystems_ReturnsAllSystemsFromRepository()
    {
        var systems = _service.GetAllSystems();

        Assert.Equal(_mapGenerator.Generate().Systems.Count, systems.Count);
    }

    [Fact]
    public void GetSystem_ReturnsCorrectSystem_WhenSystemNameIsValid()
    {
        var targetSystemName = _mapGenerator.Generate().Systems[0].Name;

        var system = _service.GetSystem(targetSystemName);

        Assert.NotNull(system);
        Assert.Equal(targetSystemName, system.Name);
    }

    [Fact]
    public void GetSystem_ReturnsNull_WhenSystemNameIsInvalid()
    {
        const string invalidSystemName = "InvalidSystemName";
        _mockRepo.Setup(r => r.GetSystem(invalidSystemName)).Returns((SystemModel?)null);

        var system = _service.GetSystem(invalidSystemName);

        Assert.Null(system);
    }
 

    [Fact]
    public void GetRandomSystem_ReturnsNull_WhenNoSystemsAreAvailable()
    {
        _mockRepo.Setup(r => r.GetAllSystems()).Returns(new List<SystemModel>());

        var randomSystem = _service.GetRandomSystem();
        Assert.Null(randomSystem);
    }
    
    [Fact]
    public void GetRandomSystem_ReturnsRandomSystem_WhenSystemsAreAvailable()
    {
        var randomSystem = _service.GetRandomSystem();
        Assert.NotNull(randomSystem);
    }
    
    [Fact]
    public void GetRandomPlanet_ReturnsNull_WhenNoPlanetsAreAvailable()
    {
        var system = _service.GetRandomSystem()!;
        system.Planets.Clear();
        _mockRepo.Setup(r => r.GetSystem(system.Name)).Returns(system);

        var randomPlanet = _service.GetRandomPlanet(system);
        Assert.Null(randomPlanet);
    }
    
    [Fact]
    public void GetRandomPlanet_ReturnsRandomPlanet_WhenPlanetsAreAvailable()
    {
        var system = _service.GetRandomSystem()!;
        var randomPlanet = _service.GetRandomPlanet(system);
        Assert.NotNull(randomPlanet);
    }
}