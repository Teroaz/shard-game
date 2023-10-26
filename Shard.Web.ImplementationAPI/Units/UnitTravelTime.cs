namespace Shard.Web.ImplementationAPI.Units;

public static class UnitTravelTime
{
    public static readonly TimeSpan TimeToChangeSystem = new(0, 0, 0, 60);
    public static readonly TimeSpan TimeToEnterPlanet = new(0, 0, 0, 15);
    public static readonly TimeSpan TimeToLeavePlanet = new(0, 0, 0, 0);
}