using System.Text.RegularExpressions;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Services;
using Shard.Web.ImplementationAPI.Units.DTOs;

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
    
    public UnitsModel? GetUnitByIdAndUser(string id, string userId)
    {
        return _unitsRepository.GetUnitByIdAndUser(id, userId);
    }
    
    public UnitsModel CreateUpdateUnits(string id, string userId, UnitsBodyDto unitsBodyDto)
    {
        var unit = _unitsRepository.GetUnitByIdAndUser(id, userId);
        
        if (unit == null)
        {
            unit = new UnitsModel(id, unitsBodyDto.Type, unitsBodyDto.System, unitsBodyDto.Planet, userId);
            
            _unitsRepository.AddUnit(unit);
        }
        
        return unit;
    }

    public Boolean IsBodyValid(string id, string userId, UnitsBodyDto? unitsBodyDto)
    {
        if (
            unitsBodyDto == null || 
            id != unitsBodyDto.Id ||
            string.IsNullOrWhiteSpace(unitsBodyDto.Type) ||
            string.IsNullOrWhiteSpace(unitsBodyDto.System) ||
            string.IsNullOrWhiteSpace(unitsBodyDto.Planet)
        )
        {
            return false;
        }

        return _common.IsIdConsistant(id, "^[a-zA-Z0-9_-]+$") && 
               _common.IsIdConsistant(userId, "^[a-zA-Z0-9_-]+$");
    }
    
    public  List<UnitsModel> GetUnitsByUser(string userId)
    {
        return _unitsRepository.GetUnitsByUser(userId);
    }
}