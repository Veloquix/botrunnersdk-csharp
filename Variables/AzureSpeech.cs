using Veloquix.BotRunner.SDK.Contracts.v1;

namespace Veloquix.BotRunner.SDK.Variables;

public enum SpeakType
{
    Name,
    Date,
    Time,
    DateTime,
    SpellOut,
    Address,
    Phone
}

public static class AzureSpeech
{
    public static string ToSpeech(this Variable variable, SpeakType type)
    {
        if (string.IsNullOrWhiteSpace(variable?.Value))
        {
            return string.Empty;
        }

        var interpret = type switch
        {
            SpeakType.Date => "date",
            SpeakType.Time => "time",
            SpeakType.Name => "name",
            SpeakType.DateTime => "datetime",
            SpeakType.SpellOut => "characters",
            SpeakType.Address => "address",
            SpeakType.Phone => "telephone",
            _ => null
        };

        return interpret is null
            ? variable.Value
            : $"<say-as interpret-as=\"{interpret}\">{variable.Value}</say-as>";
    }
}
