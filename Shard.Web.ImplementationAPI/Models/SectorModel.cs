using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Models;

public class SectorModel
{
    public List<SystemModel> Systems { get; }

    public SectorModel(SectorSpecification sectorSpecification)
    {
        Systems = sectorSpecification.Systems.Select(system => new SystemModel(system)).ToList();
    }
}