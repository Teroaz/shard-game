using Moq;
using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Systems;

namespace Shard.IntegrationTests.Buildings;

public class BuildingsRepositoryTests
{
    
    private readonly Mock<ISystemsService> _mockSystemsService;
    
    public BuildingsRepositoryTests()
    {
        _mockSystemsService = new Mock<ISystemsService>();
        _mockSystemsService.Setup(m => m.GetRandomSystem()); 
        _mockSystemsService.Setup(m => m.GetRandomPlanet(It.IsAny<SystemModel>()));
    }
    
    [Fact]
        public void CanAddAndGetBuilding()
        {
            // Arrange
            var repo = new BuildingsRepository();
            var user = new UserModel("JohnDoe");
            var system =  _mockSystemsService.Object.GetRandomSystem()!;
            var building = new BuildingModel("1", BuildingType.Mine, system!, _mockSystemsService.Object.GetRandomPlanet(system)!);

            // Act
            repo.AddBuilding(user, building);
            var result = repo.GetBuildingsByUser(user);

            // Assert
            Assert.Single(result);
            Assert.Equal(building, result.First());
        }

        [Fact]
        public void CanGetBuildingByIdAndUser()
        {
            // Arrange
            var repo = new BuildingsRepository();
            var user = new UserModel("JohnDoe");
            var system =  _mockSystemsService.Object.GetRandomSystem()!;
            var building1 = new BuildingModel("1", BuildingType.Mine, system, _mockSystemsService.Object.GetRandomPlanet(system)!);
            var building2 = new BuildingModel("2", BuildingType.Mine, system, _mockSystemsService.Object.GetRandomPlanet(system)!);
            repo.AddBuilding(user, building1);
            repo.AddBuilding(user, building2);

            // Act
            var result = repo.GetBuildingByIdAndUser(user, "2");

            // Assert
            Assert.Equal(building2, result);
            
            // Act
            var fakeUser = new UserModel("FakeUser");
            var fakeResult = repo.GetBuildingByIdAndUser(fakeUser, "2");
            
            // Assert
            Assert.Null(fakeResult);
        }

        [Fact]
        public void CanRemoveBuilding()
        {
            // Arrange
            var repo = new BuildingsRepository();
            var user = new UserModel("JohnDoe");
            var system =  _mockSystemsService.Object.GetRandomSystem()!;
            var building = new BuildingModel("1", BuildingType.Mine, system, _mockSystemsService.Object.GetRandomPlanet(system)!);
            repo.AddBuilding(user, building);

            // Act
            repo.RemoveBuilding(user, building);
            var result = repo.GetBuildingsByUser(user);

            // Assert
            Assert.Empty(result);
            
            // Act
            var fakeUser = new UserModel("FakeUser");
            repo.RemoveBuilding(fakeUser, building);
        }

        [Fact]
        public void CanUpdateBuilding()
        {
            // Arrange
            var repo = new BuildingsRepository();
            var user = new UserModel("JohnDoe");
            var system =  _mockSystemsService.Object.GetRandomSystem()!;
            var building = new BuildingModel("1", BuildingType.Mine, system, _mockSystemsService.Object.GetRandomPlanet(system)!);
            var updatedBuilding = new BuildingModel("1", BuildingType.Mine, system, _mockSystemsService.Object.GetRandomPlanet(system)!);
            repo.AddBuilding(user, building);

            // Act
            repo.UpdateBuilding(user, updatedBuilding);
            var result = repo.GetBuildingByIdAndUser(user, "1");

            // Assert
            Assert.Equal(BuildingType.Mine, result?.Type);
            
            // Act
            var fakeUser = new UserModel("FakeUser");
            repo.UpdateBuilding(fakeUser, updatedBuilding);
            
            // Assert 
            // Will do nothing if user does not exist
            
        }
}