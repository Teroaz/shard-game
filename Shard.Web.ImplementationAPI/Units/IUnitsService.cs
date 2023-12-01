using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Units.DTOs;

namespace Shard.Web.ImplementationAPI.Units;

public interface IUnitsService
{
    UnitModel? GetUnitByIdAndUser(UserModel user, string id);
    
    bool IsBodyValid(string id, UnitsBodyDto? unitsBodyDto);
    
    public List<UnitModel> GetUnitsByUser(UserModel user);
    
    void AddUnit(UserModel user, UnitModel unit);

    void RemoveUnit(UserModel user, UnitModel unit);

    void UpdateUnit(UserModel user, UnitModel unit);
}