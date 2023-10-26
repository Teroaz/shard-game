namespace Shard.Web.ImplementationAPI.Units;

public static class UnitTypeExtensions
{
    public static string ToLowerString(this UnitType unitType)
    {
        return unitType.ToString().ToLower();
    }

    public static bool IsValidUnitType(this string type)
    {
        return Enum.GetNames(typeof(UnitType)).Any(x => x.Equals(type, StringComparison.OrdinalIgnoreCase));
    }

    public static UnitType ToUnitType(this string type)
    {
        if (!type.IsValidUnitType())
        {
            throw new ArgumentException($"Invalid unit type: {type}");
        }

        return (UnitType)Enum.Parse(typeof(UnitType), type, true);
    }
}