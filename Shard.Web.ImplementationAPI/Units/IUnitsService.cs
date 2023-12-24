using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems.Models;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Units.Fighting.Models;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Units;

public interface IUnitsService
{
    UnitModel? GetUnitByIdAndUser(UserModel user, string id);

    bool IsBodyValid(string id, UnitsBodyDto? unitsBodyDto);

    public List<UnitModel> GetUnitsByUser(UserModel user);

    void AddUnit(UserModel user, UnitModel unit);

    void RemoveUnit(UserModel user, UnitModel unit);

    void UpdateUnit(UserModel user, UnitModel unit);

    UnitModel ConstructSpecificUnit(UnitType unitType, UserModel user, string unitId, SystemModel system, PlanetModel? planet, Dictionary<ResourceKind, int>? resourcesQuantity = null);

    UnitModel ConstructSpecificUnit(UnitType unitType, UserModel user, SystemModel system, PlanetModel? planet, Dictionary<ResourceKind, int>? resourcesQuantity = null);

    public List<FightingUnitModel> GetFightingUnits();
}