using System.Text.RegularExpressions;
using Shard.Web.ImplementationAPI.Model;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Services;
using Shard.Web.ImplementationAPI.Systems;
using Shard.Web.ImplementationAPI.Units;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.Web.ImplementationAPI.Users;

public class UsersService : IUserService
{
    
    private readonly IUsersRepository _usersRepository; 
    private readonly ISystemsService _systemsService;
    private readonly IUnitsRepository _unitsRepository;
    private readonly ICommon _common;
    
    public UsersService(IUsersRepository usersRepository, ICommon common, ISystemsService systemsService, IUnitsRepository unitsRepository)
    {
        _usersRepository = usersRepository;
        _systemsService = systemsService;
        _unitsRepository = unitsRepository;
        _common = common;
    }

    public Boolean IsIdConsistant(string id)
    {
        var regex = new Regex("^[a-zA-Z0-9_-]+$");
        
        return regex.IsMatch(id) && !string.IsNullOrWhiteSpace(id);
    }
    
    public UserDto? GetUserById(string id)
    {
        var user = _usersRepository.GetUserById(id);
        
        return user == null ? null : new UserDto(user);
    }
    
    public Boolean IsBodyValid(string id, UserBodyDto? userBody)
    {
        if (
            userBody == null || 
            id != userBody.Id ||
            string.IsNullOrWhiteSpace(userBody.Pseudo)
            )
        {
            return false;
        }

        return _common.IsIdConsistant(id, "^[a-zA-Z0-9_-]+$");
    }

    public UserDto CreateUpdateUser(string id, UserBodyDto userBody)
    {
        var user = _usersRepository.GetUserById(id);
        
        if (user == null)
        {
            user = new UserModel(userBody.Id, userBody.Pseudo, DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffK"));

            var random = new Random();
            
            var systems = _systemsService.GetAllSystems();
            var randomSystem = systems[random.Next(systems.Count)];
            var randomPlanet = randomSystem.Planets[random.Next(randomSystem.Planets.Count)];
            
            _usersRepository.AddUser(user);
            _unitsRepository.AddUnit(new UnitsModel(Guid.NewGuid().ToString(), "scout", randomSystem.Name, randomPlanet.Name, user.Id));
            
        }
        else
        {
            user.Pseudo = userBody.Pseudo;
        }
        
        return new UserDto(user);
    }
    
}