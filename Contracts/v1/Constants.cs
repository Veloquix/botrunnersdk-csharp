using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

namespace Veloquix.BotRunner.SDK.Contracts.v1;
public static class Constants
{
    static Constants()
    {
        Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        Converters.ForEach(Options.Converters.Add);
    }
    public const string NoAction = null;

    // Need to be kept in sync with DTMFService's const
    public const string EndDTMF = "END";

    public static List<JsonConverter> Converters =
    [
        new JsonStringEnumConverter(),
        new TypedConverter<IStatus>(),
        new TypedConverter<IState>(),
        new TypedConverter<IAction>(),
        new TypedConverter<IAllowedInput>(),
        new TypedConverter<IDataUpdate>(),
        new TypedConverter<Resolution>()
    ];

    public static JsonSerializerOptions Options { get; }
}
