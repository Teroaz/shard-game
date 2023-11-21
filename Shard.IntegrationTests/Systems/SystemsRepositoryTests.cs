using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems;

namespace Shard.IntegrationTests.Systems;

public class SystemsRepositoryTests
{
    private readonly MapGenerator _mapGenerator;
    private readonly SystemsRepository _repo;
    private const string TestSeed = "testSeed";

    public SystemsRepositoryTests()
    {
        var options = new MapGeneratorOptions { Seed = TestSeed };
        _mapGenerator = new MapGenerator(options);
        _repo = new SystemsRepository(_mapGenerator);
    }

    [Fact]
    public void GetAllSystems_ReturnsAllSystems()
    {
        var systems = _repo.GetAllSystems();

        Assert.Equal(10, systems.Count());
    }

    [Fact]
    public void GetSystem_ReturnsCorrectSystem_WhenSystemNameIsValid()
    {
        var expectedSystemName = _mapGenerator.Generate().Systems[0].Name;

        var system = _repo.GetSystem(expectedSystemName);

        Assert.NotNull(system);
        Assert.Equal(expectedSystemName, system.Name);
    }

    [Fact]
    public void GetSystem_ReturnsNull_WhenSystemNameIsInvalid()
    {
        const string invalidSystemName = "InvalidSystemName";

        var system = _repo.GetSystem(invalidSystemName);

        Assert.Null(system);
    }
}