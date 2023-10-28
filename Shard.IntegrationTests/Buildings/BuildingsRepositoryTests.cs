using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Buildings;
using Shard.Web.ImplementationAPI.Models;

namespace Shard.IntegrationTests.Buildings;

public class BuildingsRepositoryTests
{
    private readonly BuildingsRepository _repository;
    private readonly UserModel _testUser;
    private readonly SystemModel _testSystem;
    private readonly PlanetModel _testPlanet;

    // public BuildingsRepositoryTests()
    // {
    //     _repository = new BuildingsRepository();
    //     _testUser = new UserModel("TestUser");
    //     
    //     var systemSpecification = new SystemSpecification();
    //     _testSystem = new SystemModel(systemSpecification);
    //     _testPlanet = _testSystem.Planets.First();
    // }
    //
    // [Fact]
    // public void GetBuildingsByUser_ShouldReturnBuildingsForUser()
    // {
    //     // Arrange
    //     var building = new BuildingModel("TestBuilding", BuildingType.Mine, _testSystem, _testPlanet);
    //     _repository.AddBuilding(_testUser, building);
    //
    //     // Act
    //     var result = _repository.GetBuildingsByUser(_testUser);
    //
    //     // Assert
    //     Assert.Single(result);
    //     Assert.Equal(building, result.First());
    // }
}