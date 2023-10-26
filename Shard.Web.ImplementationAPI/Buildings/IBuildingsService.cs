using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Buildings;

public interface IBuildingsService
{
    bool IsBuildingTypeValid(string buildingType);

    BuildingModel? GetBuildingByIdAndUser(UserModel user, string id);

    List<BuildingModel> GetBuildingsByUser(UserModel user);

    void AddBuilding(UserModel user, BuildingModel building);

    void RemoveBuilding(UserModel user, BuildingModel building);

    void UpdateBuilding(UserModel user, BuildingModel building);
}