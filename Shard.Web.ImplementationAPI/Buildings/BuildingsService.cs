using Shard.Web.ImplementationAPI.Buildings.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Buildings;

public class BuildingsService : IBuildingsService
{
    private readonly IBuildingsRepository _buildingsRepository;

    public BuildingsService(IBuildingsRepository buildingsRepository)
    {
        _buildingsRepository = buildingsRepository;
    }
    
    public BuildingModel? GetBuildingByIdAndUser(UserModel user, string id)
    {
        return _buildingsRepository.GetBuildingByIdAndUser(user, id);
    }

    public List<BuildingModel> GetBuildingsByUser(UserModel user)
    {
        return _buildingsRepository.GetBuildingsByUser(user);
    }

    public void AddBuilding(UserModel user, BuildingModel building)
    {
        _buildingsRepository.AddBuilding(user, building);
    }

    public void RemoveBuilding(UserModel user, BuildingModel building)
    {
        _buildingsRepository.RemoveBuilding(user, building);
    }

    public void UpdateBuilding(UserModel user, BuildingModel building)
    {
        _buildingsRepository.UpdateBuilding(user, building);
    }
}