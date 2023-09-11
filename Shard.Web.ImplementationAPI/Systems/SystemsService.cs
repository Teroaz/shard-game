using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems.Dto;

namespace Shard.Web.ImplementationAPI.Systems;

public class SystemsService
{
    private readonly SectorSpecification _sectorSpecification;

    public SystemsService(MapGenerator mapGenerator)
    {
        // var random = new Random();
        //
        // var options = new MapGeneratorOptions
        // {
        //     Seed = random.Next().ToString()
        // };
        //
        // var mapGenerator = new MapGenerator(options);
        _sectorSpecification = mapGenerator.Generate();
    }

    public List<SystemDto> GetAllSystems()
    {
        return _sectorSpecification.Systems.Select(system => new SystemDto(system)).ToList();
    }

    public SystemDto? GetSystem(string systemName)
    {
        var system = _sectorSpecification.Systems.FirstOrDefault(system => system.Name == systemName);
        return system == null ? null : new SystemDto(system);
    }
}