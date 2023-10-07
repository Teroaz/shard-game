using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Systems.DTOs;

public record PlanetDto
{
    public string Name { get; }
    public int Size { get; }

    public PlanetDto(PlanetModel planetModel)
    {
        Name = planetModel.Name;
        Size = planetModel.Size;
    }
}