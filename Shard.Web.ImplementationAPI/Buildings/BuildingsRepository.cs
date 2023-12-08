using Shard.Web.ImplementationAPI.Buildings.Models;
using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Buildings;

public class BuildingsRepository : IBuildingsRepository
{
    private readonly Dictionary<UserModel, List<BuildingModel>> _buildings = new();

    public List<BuildingModel> GetBuildingsByUser(UserModel user)
    {
        if (!_buildings.ContainsKey(user)) return new List<BuildingModel>();
        
        return _buildings[user];
    }

    public BuildingModel? GetBuildingByIdAndUser(UserModel user, string id)
    {
        if (!_buildings.ContainsKey(user)) return null;

        return _buildings[user].FirstOrDefault(building => building.Id == id);
    }

    public void AddBuilding(UserModel user, BuildingModel building)
    {
        if (!_buildings.ContainsKey(user))
        {
            _buildings.Add(user, new List<BuildingModel> { building });
        }
        else
        {
            _buildings[user].Add(building);
        }
    }

    public void RemoveBuilding(UserModel user, BuildingModel building)
    {
        if (!_buildings.ContainsKey(user)) return;

        _buildings[user].Remove(building);
    }

    public void UpdateBuilding(UserModel user, BuildingModel building)
    {
        if (!_buildings.ContainsKey(user)) return;

        var index = _buildings[user].FindIndex(b => b.Id == building.Id);
        _buildings[user][index] = building;
    }
}