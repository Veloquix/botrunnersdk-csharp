using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

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

    /// <summary>
    /// Very useful when interacting with a static class or struct that holds a set of const strings that represent a group of similar things.
    /// <para>
    /// Internally, we use this to help us hold a bunch of ActionNames in a single static to group them into a
    /// HashSet, to allow us to quickly check if <see cref="IConversationContext.LastAction"/> is one of those
    /// action names.
    /// </para>
    /// </summary>
    public static HashSet<string> GetConstStrings(this Type type, params string[] excludes)
    {
        var typeInfo = (TypeInfo)type;

        return typeInfo.DeclaredMembers
            .OfType<FieldInfo>().Where(fi => !excludes.Contains(fi.Name)).Select(s => s.GetValue(null) as string).ToHashSet();
    }
}
