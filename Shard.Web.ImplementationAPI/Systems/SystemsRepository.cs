using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems.Models;

namespace Shard.Web.ImplementationAPI.Systems;

public class SystemsRepository : ISystemsRepository
{
    private readonly SectorSpecification _sectorSpecification;

    public SystemsRepository(MapGenerator mapGenerator)
    {
        _sectorSpecification = mapGenerator.Generate();
    }

    public IEnumerable<SystemModel> GetAllSystems()
    {
        return _sectorSpecification.Systems.Select(system => new SystemModel(system)).ToList();
    }

    public SystemModel? GetSystem(string systemName)
    {
        var system = _sectorSpecification.Systems.FirstOrDefault(system => system.Name == systemName);
        return system == null ? null : new SystemModel(system);
    }
}