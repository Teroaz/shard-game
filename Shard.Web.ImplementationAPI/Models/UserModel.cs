using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Models;

public class UserModel
{
    public string Id { get; set; }

    public string Pseudo { get; set; }

    public DateTime DateOfCreation { get; set; }

    public Dictionary<ResourceKind, int> ResourcesQuantity { get; }


    public UserModel(string pseudo) : this(Guid.NewGuid().ToString(), pseudo)
    {
    }

    public UserModel(string id, string pseudo) : this(id, pseudo, DateTime.Now)
    {
    }

    public UserModel(string id, string pseudo, DateTime dateOfCreation)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = dateOfCreation;
        ResourcesQuantity = new Dictionary<ResourceKind, int>
        {
            { ResourceKind.Aluminium, 0 },
            { ResourceKind.Carbon, 20 },
            { ResourceKind.Gold, 0 },
            { ResourceKind.Iron, 10 },
            { ResourceKind.Oxygen, 50 },
            { ResourceKind.Titanium, 0 },
            { ResourceKind.Water, 50 },
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj is UserModel user)
        {
            return Id == user.Id &&
                   Pseudo == user.Pseudo &&
                   DateOfCreation == user.DateOfCreation &&
                   ResourcesQuantity.Equals(user.ResourcesQuantity);
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Pseudo, DateOfCreation, ResourcesQuantity);
    }
}