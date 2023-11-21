using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems.DTOs;

namespace Shard.Web.ImplementationAPI.Systems;

public class SystemsService : ISystemsService
{
    private readonly ISystemsRepository _systemsRepository;

    public SystemsService(ISystemsRepository systemsRepository)
    {
        _systemsRepository = systemsRepository;
    }

    public IReadOnlyList<SystemModel> GetAllSystems()
    {
        var systems = _systemsRepository.GetAllSystems();
        return systems.ToList();
    }

    public SystemModel? GetSystem(string systemName)
    {
        return _systemsRepository.GetSystem(systemName);
    }
    
}