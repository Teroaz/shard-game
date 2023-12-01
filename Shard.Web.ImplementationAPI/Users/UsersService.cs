using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users.Dtos;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.Web.ImplementationAPI.Users;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly ISystemsService _systemsService;
    private readonly IUnitsRepository _unitsRepository;
    private readonly ICommon _common;

    public UsersService(IUsersRepository usersRepository, ICommon common, ISystemsService systemsService,
        IUnitsRepository unitsRepository)
    {
        _usersRepository = usersRepository;
        _systemsService = systemsService;
        _unitsRepository = unitsRepository;
        _common = common;
    }

    public UserModel? GetUserById(string id)
    {
        var user = _usersRepository.GetUserById(id);
        return user;
    }

    public bool IsBodyValid(string id, UserBodyDto? userBody)
    {
        if (userBody == null || id != userBody.Id || string.IsNullOrWhiteSpace(userBody.Pseudo))
        {
            return false;
        }

        return _common.IsIdConsistant(id, "^[a-zA-Z0-9_-]+$");
    }

    public void CreateUser(UserModel userModel)
    {
        var user = _usersRepository.GetUserById(userModel.Id);
        if (user != null) throw new Exception("User already exist");

        var randomSystem = _systemsService.GetRandomSystem();
        if (randomSystem == null) throw new Exception("No system found");
        // var randomPlanet = _systemsService.GetRandomPlanet(randomSystem);
        // if (randomPlanet == null) throw new Exception("No planet found");

        var scoutUnit = new UnitModel(UnitType.Scout, randomSystem, null);
        var builderUnit = new UnitModel(UnitType.Builder, randomSystem, null);

        _usersRepository.AddUser(userModel);
        _unitsRepository.AddUnit(userModel, scoutUnit);
        _unitsRepository.AddUnit(userModel, builderUnit);
    }

    public void UpdateUser(string userId, UserModel userModel)
    {
        var user = _usersRepository.GetUserById(userId);
        if (user == null) throw new Exception("User not found");

        user.Pseudo = userModel.Pseudo;
    }
}