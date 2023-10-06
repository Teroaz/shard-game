using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units;

public class UnitsRepository : IUnitsRepository
{
    private readonly List<UnitsModel> _units = new List<UnitsModel>();
}