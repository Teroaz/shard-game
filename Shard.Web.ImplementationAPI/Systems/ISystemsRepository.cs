using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Systems;

public interface ISystemsRepository
{
    IEnumerable<SystemModel> GetAllSystems();
    SystemModel? GetSystem(string systemName);
}