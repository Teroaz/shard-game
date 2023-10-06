using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units;

public interface IUnitsRepository
{
    UnitsModel? GetUnitByIdAndUser(string id, string userId);
    
    List<UnitsModel> GetUnitsByUser(string userId);
    
    void AddUnit(UnitsModel unit);
}