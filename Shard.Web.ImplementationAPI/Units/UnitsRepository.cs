using Shard.Web.ImplementationAPI.Units.Fighting.Models;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Units;

public class UnitsRepository : IUnitsRepository
{
    private readonly Dictionary<UserModel, List<UnitModel>> _units = new();
    private readonly List<FightingUnitModel> _allFightingUnits = new();

    public UnitModel? GetUnitByIdAndUser(UserModel user, string id)
    {
        if (!_units.ContainsKey(user)) return null;
        return _units[user].FirstOrDefault(unit => unit.Id == id);
    }

    public List<UnitModel> GetUnitsByUser(UserModel user) => !_units.ContainsKey(user) ? new List<UnitModel>() : _units[user];


    public void AddUnit(UserModel user, UnitModel unit)
    {
        if (!_units.ContainsKey(user))
        {
            _units.Add(user, new List<UnitModel>());
        }

        _units[user].Add(unit);

        if (unit is FightingUnitModel fightingUnit)
        {
            _allFightingUnits.Add(fightingUnit);
        }
    }

    public void RemoveUnit(UserModel user, UnitModel unit)
    {
        if (!_units.ContainsKey(user)) return;

        _units[user].Remove(unit);

        if (unit is FightingUnitModel fightingUnit)
        {
            _allFightingUnits.Remove(fightingUnit);
        }
    }

    public void UpdateUnit(UserModel user, UnitModel unit)
    {
        if (!_units.ContainsKey(user)) return;

        var index = _units[user].FindIndex(u => u.Id == unit.Id);
        _units[user][index] = unit;

        if (unit is FightingUnitModel fightingUnit)
        {
            var fightingUnitIndex = _allFightingUnits.FindIndex(u => u.Id == unit.Id);
            _allFightingUnits[fightingUnitIndex] = fightingUnit;
        }
    }

    public List<FightingUnitModel> GetFightingUnits()
    {
        return _allFightingUnits;
    }
}