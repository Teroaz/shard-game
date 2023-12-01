namespace Shard.Web.ImplementationAPI.Units.DTOs;

public class UnitsBodyDto
{
    public string Id { get; init; }

    public string Type { get; init; }

    public string System { get; init; }

    public string? Planet { get; init; }

    public string DestinationSystem { get; init; }

    public string? DestinationPlanet { get; init; }
}