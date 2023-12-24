using Shard.Web.ImplementationAPI.Units.Fighting.Models;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Units;

public interface IUnitsRepository
{
    UnitModel? GetUnitByIdAndUser(UserModel user, string userId);

    List<UnitModel> GetUnitsByUser(UserModel user);

    void AddUnit(UserModel user, UnitModel unit);

    void RemoveUnit(UserModel user, UnitModel unit);

    void UpdateUnit(UserModel user, UnitModel unit);
    
    List<FightingUnitModel> GetFightingUnits();
}