using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Models;

public class PlanetModel
{
    public string Name { get; }
    public int Size { get; }
    public Dictionary<ResourceKind, int> ResourceQuantity { get; }

    public PlanetModel(PlanetSpecification planetSpecification)
    {
        Name = planetSpecification.Name;
        Size = planetSpecification.Size;
        ResourceQuantity = new Dictionary<ResourceKind, int>(planetSpecification.ResourceQuantity);
    }
}