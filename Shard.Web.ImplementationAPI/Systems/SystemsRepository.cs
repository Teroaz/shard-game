using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Systems;

public interface ISystemsRepository
{
    IReadOnlyList<SystemSpecification> GetAllSystems();
    SystemSpecification? GetSystem(string systemName);
}

public class SystemsRepository : ISystemsRepository
{
    private readonly SectorSpecification _sectorSpecification;

    public SystemsRepository(MapGenerator mapGenerator)
    {
        _sectorSpecification = mapGenerator.Generate();
    }

    public IReadOnlyList<SystemSpecification> GetAllSystems()
    {
        return _sectorSpecification.Systems;
    }

    public SystemSpecification? GetSystem(string systemName)
    {
        return _sectorSpecification.Systems.FirstOrDefault(system => system.Name == systemName);
    }
}