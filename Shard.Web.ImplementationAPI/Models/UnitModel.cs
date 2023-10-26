using Shard.Shared.Core;
using Shard.Web.ImplementationAPI.Units;

namespace Shard.Web.ImplementationAPI.Models;

public class UnitModel
{
    public string Id { get; set; }

    public UnitType Type { get; set; }

    public SystemModel System { get; set; }

    public PlanetModel? Planet { get; set; }

    public SystemModel DestinationSystem { get; set; }

    public PlanetModel? DestinationPlanet { get; set; }

    public DateTime EstimatedArrivalTime { get; set; }

    public UnitModel(UnitType type, SystemModel system, PlanetModel? planet)
        : this(Guid.NewGuid().ToString(), type, system, planet)
    {
    }

    public UnitModel(string id, UnitType type, SystemModel system, PlanetModel? planet)
    {
        Id = id;
        Type = type;
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

        if (System.Name != destinationSystem.Name)
        {
            timeToMove = timeToMove.Add(UnitTravelTime.TimeToChangeSystem);
        }

        DestinationSystem = destinationSystem;
        DestinationPlanet = destinationPlanet;
        EstimatedArrivalTime = now.Add(timeToMove);

        clock.CreateTimer(state =>
        {
            var unit = (UnitModel)state!;
            System = unit.DestinationSystem;
            Planet = unit.DestinationPlanet;
        }, this, timeToMove, new TimeSpan());
    }
}