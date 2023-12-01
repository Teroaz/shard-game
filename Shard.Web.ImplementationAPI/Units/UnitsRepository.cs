using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units;

public class UnitsRepository : IUnitsRepository
{
    private readonly Dictionary<UserModel, List<UnitModel>> _units = new();

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
    }

    public void RemoveUnit(UserModel user, UnitModel unit)
    {
        if (!_units.ContainsKey(user)) return;

        _units[user].Remove(unit);
    }

    public void UpdateUnit(UserModel user, UnitModel unit)
    {
        if (!_units.ContainsKey(user)) return;

        var index = _units[user].FindIndex(u => u.Id == unit.Id);
        _units[user][index] = unit;
    }
}