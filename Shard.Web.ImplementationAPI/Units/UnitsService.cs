using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units.DTOs;
using Shard.Web.ImplementationAPI.Users;
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
            string.IsNullOrWhiteSpace(unitsBodyDto.Type) ||
            string.IsNullOrWhiteSpace(unitsBodyDto.System)
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
}