using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Models;

public class SystemModel
{
    public string Name { get; }
    public List<PlanetModel> Planets { get; }

    public SystemModel(SystemSpecification systemSpecification)
    {
        Name = systemSpecification.Name;
        Planets = systemSpecification.Planets.Select(planet => new PlanetModel(planet)).ToList();
    }
}