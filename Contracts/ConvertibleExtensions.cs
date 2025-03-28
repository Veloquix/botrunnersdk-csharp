namespace Veloquix.BotRunner.SDK.Contracts;

public static class ConvertibleExtensions
{
    public static string GetTypeName(this IConvertible convertible)
        => convertible.GetType().Name;
}