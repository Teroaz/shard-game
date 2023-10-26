namespace Shard.Web.ImplementationAPI.Models;

public class UserModel
{
    public string Id { get; set; }

    public string Pseudo { get; set; }

    public DateTime DateOfCreation { get; set; }

    public UserModel(string pseudo) : this(Guid.NewGuid().ToString(), pseudo)
    {
    }

    public UserModel(string id, string pseudo)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = DateTime.Now;
    }

    public UserModel(string id, string pseudo, DateTime dateOfCreation)
    {
        Id = id;
        Pseudo = pseudo;
        DateOfCreation = dateOfCreation;
    }

    public override bool Equals(object? obj)
    {
        if (obj is UserModel user)
        {
            return Id == user.Id &&
                   Pseudo == user.Pseudo &&
                   DateOfCreation == user.DateOfCreation;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Pseudo, DateOfCreation);
    }
}