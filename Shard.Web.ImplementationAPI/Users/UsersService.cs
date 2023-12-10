using Shard.Web.ImplementationAPI.Enums;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Units.Models;
using Shard.Web.ImplementationAPI.Users.Dtos;
using Shard.Web.ImplementationAPI.Utils;

namespace Shard.Web.ImplementationAPI.Users;

public class UsersService : IUsersService
{
    // 89
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

    public bool IsBodyValid(string id, UserBodyDto? userBody, bool isAdmin = false)
    {
        if (userBody == null || (id != userBody.Id && !isAdmin) || string.IsNullOrWhiteSpace(userBody.Pseudo))
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

        var scoutUnit = new ScoutUnitModel(userModel, randomSystem, null);
        var builderUnit = new BuilderUnitModel(userModel, randomSystem, null);

        _usersRepository.AddUser(userModel);
        _unitsRepository.AddUnit(userModel, scoutUnit);
        _unitsRepository.AddUnit(userModel, builderUnit);
    }

    public void UpdateUser(string userId, UserModel userModel)
    {
        var user = _usersRepository.GetUserById(userId);
        if (user == null) throw new Exception("User not found");

        user.Pseudo = userModel.Pseudo;
        user.DateOfCreation = userModel.DateOfCreation;
        user.ResourcesQuantity.Clear();
        foreach (var (key, value) in userModel.ResourcesQuantity)
        {
            user.ResourcesQuantity[key] = value;
        }
    }
}