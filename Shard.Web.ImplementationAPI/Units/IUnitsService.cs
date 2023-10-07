using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Units.DTOs;

namespace Shard.Web.ImplementationAPI.Units;

public interface IUnitsService
{
    UnitModel? GetUnitByIdAndUser(string id, string userId);
    UnitModel CreateUpdateUnits(string id, string userId, UnitsBodyDto unitsBodyDto);
    
    Boolean IsBodyValid(string id, string userId, UnitsBodyDto? unitsBodyDto);
    
    public  List<UnitsDto> GetUnitsByUser(string userId);
}