using Shard.Web.ImplementationAPI.Systems.DTOs;

namespace Shard.Web.ImplementationAPI.Systems;

public class SystemsService : ISystemsService
{
    private readonly ISystemsRepository _systemsRepository;

    public SystemsService(ISystemsRepository systemsRepository)
    {
        _systemsRepository = systemsRepository;
    }

    public IReadOnlyList<SystemDto> GetAllSystems()
    {
        var systems = _systemsRepository.GetAllSystems();
        return systems.Select(system => new SystemDto(system)).ToList();
    }

    public SystemDto? GetSystem(string systemName)
    {
        var system = _systemsRepository.GetSystem(systemName);
        return system == null ? null : new SystemDto(system);
    }
    
}