using Shard.Web.ImplementationAPI.Systems.DTOs;

namespace Shard.Web.ImplementationAPI.Systems;

public interface ISystemsService
{
    IReadOnlyList<SystemDto> GetAllSystems();
    SystemDto? GetSystem(string systemName);
}