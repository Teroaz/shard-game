using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Units.DTOs;

namespace Shard.Web.ImplementationAPI.Units;

public interface IUnitsService
{
    UnitsModel? GetUnitByIdAndUser(string id, string userId);
    UnitsModel CreateUpdateUnits(string id, string userId, UnitsBodyDto unitsBodyDto);
    
    Boolean IsBodyValid(string id, string userId, UnitsBodyDto? unitsBodyDto);
    
    public  List<UnitsDto> GetUnitsByUser(string userId);
}