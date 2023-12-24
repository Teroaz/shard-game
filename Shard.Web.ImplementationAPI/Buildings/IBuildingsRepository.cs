using Shard.Web.ImplementationAPI.Buildings.Models;
using Shard.Web.ImplementationAPI.Users.Models;

namespace Shard.Web.ImplementationAPI.Buildings;

public interface IBuildingsRepository
{
    List<BuildingModel> GetBuildingsByUser(UserModel user);

    BuildingModel? GetBuildingByIdAndUser(UserModel user, string id);

    void AddBuilding(UserModel user, BuildingModel building);

    void RemoveBuilding(UserModel user, BuildingModel building);

    void UpdateBuilding(UserModel user, BuildingModel building);
}