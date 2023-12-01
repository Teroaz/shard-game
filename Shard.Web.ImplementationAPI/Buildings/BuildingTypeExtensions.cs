namespace Shard.Web.ImplementationAPI.Buildings;

public static class BuildingTypeExtensions
{
    public static string ToLowerString(this BuildingType buildingType)
    {
        return buildingType.ToString().ToLower();
    }

    public static bool IsValidBuildingType(this string type)
    {
        return Enum.GetNames(typeof(BuildingType)).Any(x => x.Equals(type, StringComparison.OrdinalIgnoreCase));
    }

    public static BuildingType ToBuildingType(this string type)
    {
        if (!type.IsValidBuildingType())
        {
            throw new ArgumentException($"Invalid unit type: {type}");
        }

        return (BuildingType)Enum.Parse(typeof(BuildingType), type, true);
    }
}