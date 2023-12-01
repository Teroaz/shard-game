using Shard.Web.ImplementationAPI.Models;
using Shard.Web.ImplementationAPI.Users.Dtos;

namespace Shard.IntegrationTests.Users;

public class UserDtoTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        var id = "123";
        var pseudo = "John Doe";
        var dateOfCreation = DateTimeOffset.Now.ToString("yyyy-MM-dd-HH:mm:ss");
        var userModel = new UserModel(id, pseudo, DateTime.Now);

        // Act
        var userDto = new UserDto(userModel);

        // Assert
        Assert.Equal(id, userDto.Id);
        Assert.Equal(pseudo, userDto.Pseudo);
        Assert.Equal(dateOfCreation, userDto.DateOfCreation);
    }
}