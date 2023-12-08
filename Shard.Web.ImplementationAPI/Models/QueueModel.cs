using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Units;

namespace Shard.Web.ImplementationAPI.Models;

public class QueueModel
{
    public UnitType Type { get; set; }

    public QueueModel(UnitType type)
    {
        Type = type;
    }
}