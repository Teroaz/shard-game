using Shard.Web.ImplementationAPI.Models;

namespace Shard.Web.ImplementationAPI.Users.Dtos;

public record UserDto
{
    
    public string Id { get; init; }
    
    public string Pseudo { get; init; }
    
    public string DateOfCreation { get; init; }
    
    public UserDto(UserModel userModel)
    {
        Id = userModel.Id;
        Pseudo = userModel.Pseudo;
        DateOfCreation = userModel.DateOfCreation;
    }
}