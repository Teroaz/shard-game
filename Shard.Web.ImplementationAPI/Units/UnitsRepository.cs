using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units;

public class UnitsRepository : IUnitsRepository
{
    private readonly List<UnitModel> _units = new List<UnitModel>();
    
    public UnitModel? GetUnitByIdAndUser(string id, string userId)
    {
        return _units.FirstOrDefault(unit => unit.Id == id && unit.UserId == userId);
    }
    
    public void AddUnit(UnitModel unit)
    {
        _units.Add(unit);
    }
    
    public  List<UnitModel> GetUnitsByUser(string userId)
    {
        return _units.Where(unit => unit.UserId == userId).ToList();
    }
}