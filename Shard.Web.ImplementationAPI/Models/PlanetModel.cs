using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Models;

public class PlanetModel
{
    public string Name { get; }
    public int Size { get; }
    public IReadOnlyDictionary<ResourceKind, int> ResourceQuantity { get; }

    public PlanetModel(PlanetSpecification planetSpecification)
    {
        Name = planetSpecification.Name;
        Size = planetSpecification.Size;
        ResourceQuantity = planetSpecification.ResourceQuantity;
    }
}