using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems.DTOs;

namespace Shard.Web.ImplementationAPI.Systems;

public interface ISystemsService
{
    IReadOnlyList<SystemModel> GetAllSystems();
    SystemModel? GetSystem(string systemName);
}