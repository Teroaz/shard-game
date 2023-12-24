namespace Shard.Web.ImplementationAPI.Buildings.DTOs;

public class QueueDto
{
    public string Type { get; }

    public QueueDto(string type)
    {
        Type = type;
    }
}