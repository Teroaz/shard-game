using Shard.Shared.Core;

namespace Shard.Web.ImplementationAPI.Models;

public class UserModel
{
    public string Id { get; set; }

    public string Pseudo { get; set; }

    public DateTime DateOfCreation { get; set; }

    public Dictionary<ResourceKind, int> ResourcesQuantity { get; set; }

    public bool IsSharded { get; set; }

    public UserModel(string pseudo, IClock clock, bool isSharded = false) : this(Guid.NewGuid().ToString(), pseudo, clock.Now, isSharded)
    {
    }

    public UserModel(string id, string pseudo, DateTime dateOfCreation, bool isSharded = false)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = dateOfCreation;
        IsSharded = isSharded;
        ResourcesQuantity = new Dictionary<ResourceKind, int>
        {
            { ResourceKind.Aluminium, 0 },
            { ResourceKind.Carbon, isSharded ? 0 : 20 },
            { ResourceKind.Gold, 0 },
            { ResourceKind.Iron, isSharded ? 0 : 10 },
            { ResourceKind.Oxygen, isSharded ? 0 : 50 },
            { ResourceKind.Titanium, 0 },
            { ResourceKind.Water, isSharded ? 0 : 50 },
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

    public bool HasEnoughResources(Dictionary<ResourceKind, int> resources)
    {
        return resources.All(
            resource => resource.Value <= ResourcesQuantity[resource.Key]
        );
    }

    public void ConsumeResources(Dictionary<ResourceKind, int> resources)
    {
        foreach (var resource in resources)
        {
            ResourcesQuantity[resource.Key] -= resource.Value;
        }
    }

    public bool TrySubtractResources(Dictionary<ResourceKind, int> resources)
    {
        foreach (var resource in resources)
        {
            var newQuantity = ResourcesQuantity[resource.Key] - resource.Value;

            if (resource.Value > 0 && newQuantity < 0)
            {
                return false;
            }

            ResourcesQuantity[resource.Key] = newQuantity;
        }

        return true;
    }
}