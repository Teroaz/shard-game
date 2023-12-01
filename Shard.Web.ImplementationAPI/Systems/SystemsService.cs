using Shard.Web.ImplementationAPI.Models;

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

    public SystemModel? GetRandomSystem()
    {
        var systems = _systemsRepository.GetAllSystems().ToList();
        if (systems.Count == 0)
        {
            return null;
        }

        var randomSystem = systems.ElementAt(new Random().Next(systems.Count));
        return randomSystem;
    }

    public PlanetModel? GetRandomPlanet(SystemModel system)
    {
        var planets = system.Planets.ToList();
        if (planets.Count == 0)
        {
            return null;
        }

        var randomPlanet = planets.ElementAt(new Random().Next(planets.Count));
        return randomPlanet;
    }
}