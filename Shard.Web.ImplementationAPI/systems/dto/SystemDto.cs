using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.systems.dto;

public record SystemDto
{
    public string Name { get; init; }
    public List<PlanetDto> Planets { get; init; }

    public SystemDto(SystemSpecification systemSpecification)
    {
        Name = systemSpecification.Name;
        Planets = systemSpecification.Planets.Select(planet => new PlanetDto(planet)).ToList();
    }
}