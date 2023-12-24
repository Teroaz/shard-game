using Shard.Web.ImplementationAPI.Systems.Models;

namespace Shard.Web.ImplementationAPI.Systems.DTOs;

public record SystemDto
{
    public string Name { get; }
    public List<PlanetDto> Planets { get; }

    public SystemDto(SystemModel systemSpecification)
    {
        Name = systemSpecification.Name;
        Planets = systemSpecification.Planets.Select(planet => new PlanetDto(planet)).ToList();
    }
}