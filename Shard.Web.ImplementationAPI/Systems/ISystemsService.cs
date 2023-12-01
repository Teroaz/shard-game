using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Systems;

public interface ISystemsService
{
    IReadOnlyList<SystemModel> GetAllSystems();
    SystemModel? GetSystem(string systemName);
    SystemModel? GetRandomSystem();
    PlanetModel? GetRandomPlanet(SystemModel system);
}