using System.Text.RegularExpressions;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.Web.ImplementationAPI.Units;

public class UnitsService : IUnitsService
{
    
    private readonly IUnitsRepository _unitsRepository;
    
    private readonly ISystemsService _systemsService;
    
    private readonly ICommon _common;
    
    public UnitsService(IUnitsRepository unitsRepository, ICommon common, ISystemsService systemsService)
    {
        _unitsRepository = unitsRepository; 
        _systemsService = systemsService;
        _common = common;
    }
    
    public UnitModel? GetUnitByIdAndUser(string id, string userId)
    {
        return _unitsRepository.GetUnitByIdAndUser(id, userId);
    }
    
    public UnitModel CreateUpdateUnits(string id, string userId, UnitsBodyDto unitsBodyDto)
    {
        var unit = _unitsRepository.GetUnitByIdAndUser(id, userId);
        
        if (unit == null)
        {
            // Create new unit
            var random = new Random();
            var planet = unitsBodyDto.Planet;
            var system = _systemsService.GetSystem(unitsBodyDto.System);
            
            if (system == null)
            {
                var systems = _systemsService.GetAllSystems();
                var randomSystem = systems[random.Next(systems.Count)];
                system = _systemsService.GetSystem(randomSystem.Name);
            }
            
            if (unitsBodyDto.Planet == null)
            {
                planet = system!.Planets[random.Next(system.Planets.Count)].Name;
            }
            
            
            unit = new UnitModel(id, unitsBodyDto.Type, unitsBodyDto.System, planet!, userId);
            
            _unitsRepository.AddUnit(unit);
        }
        else
        {
            // Update existing unit
            unit.Id = unitsBodyDto.Id;
            unit.Type = unitsBodyDto.Type;
            unit.System = unitsBodyDto.System;
            if (unitsBodyDto.Planet != null) unit.Planet = unitsBodyDto.Planet;
        }
        
        return unit;
    }

    public bool IsBodyValid(string id, string userId, UnitsBodyDto? unitsBodyDto)
    {
        if (
            unitsBodyDto == null || 
            id != unitsBodyDto.Id ||
            string.IsNullOrWhiteSpace(unitsBodyDto.Type) ||
            string.IsNullOrWhiteSpace(unitsBodyDto.System)
        )
        {
            return false;
        }

        return _common.IsIdConsistant(id, "^[a-zA-Z0-9_-]+$");
    }
    
    public  List<UnitsDto> GetUnitsByUser(string userId)
    {
        return _unitsRepository.GetUnitsByUser(userId).Select(unit => new UnitsDto(unit)).ToList();
    }
}