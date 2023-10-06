using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Units;

public class UnitsRepository : IUnitsRepository
{
    private readonly List<UnitsModel> _units = new List<UnitsModel>();
    
    public UnitsModel? GetUnitByIdAndUser(string id, string userId)
    {
        return _units.FirstOrDefault(unit => unit.Id == id && unit.UserId == userId);
    }
    
    public void AddUnit(UnitsModel unit)
    {
        _units.Add(unit);
    }
    
    public  List<UnitsModel> GetUnitsByUser(string userId)
    {
        return _units.Where(unit => unit.UserId == userId).ToList();
    }
}