namespace Veloquix.BotRunner.SDK;

// Still determining best way to help with translating data and providing helpers for managing messages. Commented out, but this is useful code.
//public enum SpeakType
//{
//    Name,
//    Date,
//    Time,
//    DateTime,
//    SpellOut,
//    Address,
//    Phone
//}

//public static class AzureSpeech
//{
//    public static string ToSpeech(this Variable variable, SpeakType type)
//    {
//        if (string.IsNullOrWhiteSpace(variable?.Value))
//        {
//            return string.Empty;
//        }

//        var interpret = type switch
//        {
//            SpeakType.Date => "date",
//            SpeakType.Time => "time",
//            SpeakType.Name => "name",
//            SpeakType.DateTime => "datetime",
//            SpeakType.SpellOut => "characters",
//            SpeakType.Address => "address",
//            SpeakType.Phone => "telephone",
//            _ => null
//        };

//        return interpret is null
//            ? variable.Value
//            : $"<say-as interpret-as=\"{interpret}\">{variable.Value}</say-as>";
//    }
//}
