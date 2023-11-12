namespace Shard.Web.ImplementationAPI.Utils;

public static class EnumExtensions
{
    public static string ToLowerString<TEnum>(this TEnum enumValue) where TEnum : struct, Enum
    {
        return enumValue.ToString().ToLower();
    }

    public static bool IsValidEnumValue<TEnum>(this string value) where TEnum : struct, Enum
    {
        return Enum.GetNames(typeof(TEnum)).Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
    }

    public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct, Enum
    {
        if (!value.IsValidEnumValue<TEnum>())
        {
            throw new ArgumentException($"Invalid value: {value}");
        }

        return (TEnum)Enum.Parse(typeof(TEnum), value, true);
    }
}