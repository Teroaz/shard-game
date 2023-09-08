using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.systems.dto;

public record PlanetDto
{
    public string Name { get; init; }
    public int Size { get; init; }

    public PlanetDto(PlanetSpecification planetSpecification)
    {
        Name = planetSpecification.Name;
        Size = planetSpecification.Size;
    }
}