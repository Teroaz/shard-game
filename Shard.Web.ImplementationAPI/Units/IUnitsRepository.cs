using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units;

public interface IUnitsRepository
{
    UnitModel? GetUnitByIdAndUser(string id, string userId);
    
    List<UnitModel> GetUnitsByUser(string userId);
    
    void AddUnit(UnitModel unit);
}