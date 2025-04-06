using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK;
/// <summary>
/// This is a list of special variable names in BotRunner. Be careful
/// in setting variables by these names; it's not restricted, but it's
/// easy to break weird things.
/// </summary>
public static class StandardVariables
{
    public static HashSet<string> All = typeof(StandardVariables).GetConstStrings();
    public const string CurrentLanguage = "CurrentLanguage";
    public const string FailureCount = "FailureCount";
}
