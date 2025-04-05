namespace Veloquix.BotRunner.SDK;

/// <summary>
/// Various 
/// </summary>
public static class GeneralExtensions
{
    public static string VAR(this string str)
        => $"{{VAR.{str}}}";

    public static string Get(this IMessage msg, IConversationContext ctx)
    {
        if (!msg.ByLanguageCode.TryGetValue(ctx.Language, out var text))
        {
            throw new VeloquixException($"Language {ctx.Language} is not defined for Message {msg.Name}");
        }

        return text;
    }
}
