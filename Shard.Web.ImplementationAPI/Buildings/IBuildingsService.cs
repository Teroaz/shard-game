using Shard.Web.ImplementationAPI.Buildings.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Buildings;

public interface IBuildingsService
{
    BuildingModel? GetBuildingByIdAndUser(UserModel user, string id);

    List<BuildingModel> GetBuildingsByUser(UserModel user);

    void AddBuilding(UserModel user, BuildingModel building);

    void RemoveBuilding(UserModel user, BuildingModel building);

    void UpdateBuilding(UserModel user, BuildingModel building);
}