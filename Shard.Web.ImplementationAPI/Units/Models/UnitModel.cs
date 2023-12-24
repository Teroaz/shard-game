using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Systems.Models;
using Shard.Web.ImplementationAPI.Users.Models;
using Shard.Web.ImplementationAPI.Wormholes;

namespace Shard.Web.ImplementationAPI.Units.Models;

public abstract class UnitModel
{
    public string Id { get; }
    
    public UserModel User { get; }
    
    public abstract UnitType Type { get; }

    public SystemModel System { get; set; }

    public PlanetModel? Planet { get; set; }
    
    public SystemModel? DestinationSystem { get; set; }

    public PlanetModel? DestinationPlanet { get; set; }

    public DateTime EstimatedArrivalTime { get; private set; }

    public Task? MoveTask { get; private set; }
    
    public Dictionary<ResourceKind, int>? ResourcesQuantity { get; set; }

    protected UnitModel(UserModel user, SystemModel system, PlanetModel? planet)
        : this(Guid.NewGuid().ToString(), user, system, planet)
    {
    }

    protected UnitModel(string id, UserModel user, SystemModel system, PlanetModel? planet)
    {
        Id = id;
        User = user;
        System = system;
        Planet = planet;
        DestinationSystem = system;
        DestinationPlanet = planet;
    }

    public void Move(IClock clock, SystemModel destinationSystem, PlanetModel? destinationPlanet)
    {
        var now = clock.Now;
        var timeToMove = UnitTravelTime.TimeToLeavePlanet;

        if (Planet?.Name != destinationPlanet?.Name)
        {
            timeToMove = timeToMove.Add(UnitTravelTime.TimeToEnterPlanet);
        }

        if (System?.Name != destinationSystem.Name)
        {
            timeToMove = timeToMove.Add(UnitTravelTime.TimeToChangeSystem);
        }

        DestinationSystem = destinationSystem;
        DestinationPlanet = destinationPlanet;
        EstimatedArrivalTime = now.Add(timeToMove);

        MoveTask = clock.Delay(timeToMove).ContinueWith(t =>
        {
            System = DestinationSystem;
            Planet = DestinationPlanet;
            MoveTask = null; // Réinitialiser MoveTask une fois le déplacement terminé
        });
    }
}