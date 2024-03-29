using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems.Models;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Units.Fighting.Models;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users.Models;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.Web.ImplementationAPI.Units;

public class UnitsService : IUnitsService
{
    private readonly IUnitsRepository _unitsRepository;
    private readonly ICommon _common;

    public UnitsService(IUnitsRepository unitsRepository, ICommon common)
    {
        _unitsRepository = unitsRepository;
        _common = common;
    }

    public UnitModel? GetUnitByIdAndUser(UserModel user, string id)
    {
        return _unitsRepository.GetUnitByIdAndUser(user, id);
    }

    public void AddUnit(UserModel user, UnitModel unit)
    {
        _unitsRepository.AddUnit(user, unit);
    }

    public void RemoveUnit(UserModel user, UnitModel unit)
    {
        _unitsRepository.RemoveUnit(user, unit);
    }

    public void UpdateUnit(UserModel user, UnitModel unit)
    {
        _unitsRepository.UpdateUnit(user, unit);
    }

    public bool IsBodyValid(string id, UnitsBodyDto? unitsBodyDto)
    {
        if (
            unitsBodyDto == null ||
            id != unitsBodyDto.Id ||
            string.IsNullOrWhiteSpace(unitsBodyDto.Type)
        )
        {
            return false;
        }

        return _common.IsIdConsistant(id, "^[a-zA-Z0-9_-]+$");
    }

    public List<UnitModel> GetUnitsByUser(UserModel user)
    {
        return _unitsRepository.GetUnitsByUser(user);
    }

    public UnitModel ConstructSpecificUnit(UnitType unitType, UserModel user, string unitId, SystemModel system, PlanetModel? planet, Dictionary<ResourceKind, int>? resourcesQuantity)
    {
        return unitType switch
        {
            UnitType.Builder => new BuilderUnitModel(unitId, user, system, planet),
            UnitType.Scout => new ScoutUnitModel(unitId, user, system, planet),
            UnitType.Bomber => new BomberUnitModel(unitId, user, system, planet),
            UnitType.Cruiser => new CruiserUnitModel(unitId, user, system, planet),
            UnitType.Fighter => new FighterUnitModel(unitId, user, system, planet),
            UnitType.Cargo => new CargoUnitModel(unitId, user, system, planet, resourcesQuantity),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public UnitModel ConstructSpecificUnit(UnitType unitType, UserModel user, SystemModel system, PlanetModel? planet, Dictionary<ResourceKind, int>? resourcesQuantity)
    {
        return ConstructSpecificUnit(unitType, user, Guid.NewGuid().ToString(), system, planet, resourcesQuantity);
    }

    public List<FightingUnitModel> GetFightingUnits()
    {
        return _unitsRepository.GetFightingUnits();
    }
    
}