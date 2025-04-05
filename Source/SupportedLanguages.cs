using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Veloquix.BotRunner.SDK;
public static class SupportedLanguages
{
    internal static readonly HashSet<string> AllTheLanguages;
    static SupportedLanguages()
    {
        var type = typeof(SupportedLanguages);
        var groups = ((TypeInfo)type).DeclaredMembers.OfType<TypeInfo>().ToList();

        AllTheLanguages =
        [
            ..groups.SelectMany(g => g.DeclaredMembers)
                .OfType<FieldInfo>().Select(s => s.GetValue(null) as string)
        ];
    }
    public static class Afrikaans
    {
        public const string Common = "af-ZA";
    }

    public static class Albanian
    {
        public const string Common = "sq-AL";
    }

    public static class Amharic
    {
        public const string Common = "am-ET";
    }

    public static class Arabic
    {
        // We choose Egyptian Arabic as the generic/common variant.
        public const string Common = "ar-EG";
        public const string UnitedArabEmirates = "ar-AE";
        public const string Bahrain = "ar-BH";
        public const string Algeria = "ar-DZ";
        public const string Iraq = "ar-IQ";
        public const string Jordan = "ar-JO";
        public const string Israel = "ar-IL";
        public const string Lebanon = "ar-LB";
        public const string Libya = "ar-LY";
        public const string Morocco = "ar-MA";
        public const string Oman = "ar-OM";
        public const string Qatar = "ar-QA";
        public const string SaudiArabia = "ar-SA";
        public const string Syria = "ar-SY";
        public const string Tunisia = "ar-TN";
        public const string Yemen = "ar-YE";
        public const string Palestine = "ar-PS";
    }

    public static class Assamese
    {
        public const string Common = "as-IN";
    }

    public static class Azerbaijani
    {
        public const string Common = "az-AZ";
    }

    public static class Bengali
    {
        // For Bengali we assume Bangladesh is the common variant.
        public const string Common = "bn-BD";
        public const string India = "bn-IN";
    }

    public static class Bosnian
    {
        public const string Common = "bs-BA";
    }

    public static class Bulgarian
    {
        public const string Common = "bg-BG";
    }

    public static class Catalan
    {
        public const string Common = "ca-ES";
    }

    public static class Cantonese
    {
        public const string Common = "yue-CN";
    }

    public static class Chinese
    {
        // Here we use "zh-CN" (Mainland China) as the common locale.
        public const string Common = "zh-CN";
        public const string Guangxi = "zh-CN-GUANGXI";
        public const string Henan = "zh-CN-henan";
        public const string Liaoning = "zh-CN-liaoning";
        public const string Shaanxi = "zh-CN-shaanxi";
        public const string Shandong = "zh-CN-shandong";
        public const string Sichuan = "zh-CN-sichuan";
        public const string HongKong = "zh-HK";
        public const string Taiwan = "zh-TW";
    }

    public static class Croatian
    {
        public const string Common = "hr-HR";
    }

    public static class Czech
    {
        public const string Common = "cs-CZ";
    }

    public static class Danish
    {
        public const string Common = "da-DK";
    }

    public static class Dutch
    {
        // Using the Netherlands variant as the common one.
        public const string Common = "nl-NL";
        public const string Belgium = "nl-BE";
    }

    public static class English
    {
        // For English we list each variant without a "Common" property.
        public const string Australia = "en-AU";
        public const string Canada = "en-CA";
        public const string UnitedKingdom = "en-GB";
        public const string India = "en-IN";
        public const string Kenya = "en-KE";
        public const string Nigeria = "en-NG";
        public const string NewZealand = "en-NZ";
        public const string Philippines = "en-PH";
        public const string Singapore = "en-SG";
        public const string Tanzania = "en-TZ";
        public const string UnitedStates = "en-US";
        public const string Ghana = "en-GH";
        public const string SouthAfrica = "en-ZA";
    }

    public static class Estonian
    {
        public const string Common = "et-EE";
    }

    public static class Filipino
    {
        public const string Common = "fil-PH";
    }

    public static class Finnish
    {
        public const string Common = "fi-FI";
    }

    public static class French
    {
        // Use France as the common variant.
        public const string Common = "fr-FR";
        public const string Belgium = "fr-BE";
        public const string Canada = "fr-CA";
        public const string Switzerland = "fr-CH";
    }

    public static class Galician
    {
        public const string Common = "gl-ES";
    }

    public static class Georgian
    {
        public const string Common = "ka-GE";
    }

    public static class German
    {
        // Using Germany as the common locale.
        public const string Common = "de-DE";
        public const string Austria = "de-AT";
        public const string Switzerland = "de-CH";
    }

    public static class Greek
    {
        public const string Common = "el-GR";
    }

    public static class Gujarati
    {
        public const string Common = "gu-IN";
    }

    public static class Hebrew
    {
        public const string Common = "he-IL";
    }

    public static class Hindi
    {
        public const string Common = "hi-IN";
    }

    public static class Hungarian
    {
        public const string Common = "hu-HU";
    }

    public static class Icelandic
    {
        public const string Common = "is-IS";
    }

    public static class Indonesian
    {
        public const string Common = "id-ID";
    }

    public static class Inuktitut
    {
        // We use the Latin script variant as the common one.
        public const string Common = "iu-LATN-CA";
        public const string Cans = "iu-CANS-CA";
    }

    public static class Italian
    {
        // Italy is taken as the common locale.
        public const string Common = "it-IT";
        public const string Switzerland = "it-CH";
    }

    public static class Japanese
    {
        public const string Common = "ja-JP";
    }

    public static class Javanese
    {
        public const string Common = "jv-ID";
    }

    public static class Kazakh
    {
        public const string Common = "kk-KZ";
    }

    public static class Khmer
    {
        public const string Common = "km-KH";
    }

    public static class Kannada
    {
        public const string Common = "kn-IN";
    }

    public static class Lao
    {
        public const string Common = "lo-LA";
    }

    public static class Latvian
    {
        public const string Common = "lv-LV";
    }

    public static class Lithuanian
    {
        public const string Common = "lt-LT";
    }

    public static class Macedonian
    {
        public const string Common = "mk-MK";
    }

    public static class Malay
    {
        public const string Common = "ms-MY";
    }

    public static class Malayalam
    {
        public const string Common = "ml-IN";
    }

    public static class Maltese
    {
        public const string Common = "mt-MT";
    }

    public static class Marathi
    {
        public const string Common = "mr-IN";
    }

    public static class Mongolian
    {
        public const string Common = "mn-MN";
    }

    public static class Nepali
    {
        public const string Common = "ne-NP";
    }

    public static class Norwegian
    {
        public const string Common = "nb-NO";
    }

    public static class Odia
    {
        public const string Common = "or-IN";
    }

    public static class Punjabi
    {
        public const string Common = "pa-IN";
    }

    public static class Persian
    {
        public const string Common = "fa-IR";
    }

    public static class Polish
    {
        public const string Common = "pl-PL";
    }

    public static class Portuguese
    {
        // We use Portugal as the generic variant.
        public const string Common = "pt-PT";
        public const string Brazil = "pt-BR";
    }

    public static class Pashto
    {
        public const string Common = "ps-AF";
    }

    public static class Romanian
    {
        public const string Common = "ro-RO";
    }

    public static class Russian
    {
        public const string Common = "ru-RU";
    }

    public static class Sinhala
    {
        public const string Common = "si-LK";
    }

    public static class Serbian
    {
        // Here we choose the version without script subtags as the common one.
        public const string Common = "sr-RS";
        public const string Latin = "sr-LATN-RS";
    }

    public static class Slovak
    {
        public const string Common = "sk-SK";
    }

    public static class Slovenian
    {
        public const string Common = "sl-SI";
    }

    public static class Somali
    {
        public const string Common = "so-SO";
    }

    public static class Spanish
    {
        // For Spanish we choose Spain (es-ES) as the common locale.
        public const string Common = "es-ES";
        public const string Argentina = "es-AR";
        public const string Bolivia = "es-BO";
        public const string Chile = "es-CL";
        public const string Colombia = "es-CO";
        public const string CostaRica = "es-CR";
        public const string Cuba = "es-CU";
        public const string DominicanRepublic = "es-DO";
        public const string Ecuador = "es-EC";
        public const string EquatorialGuinea = "es-GQ";
        public const string Guatemala = "es-GT";
        public const string Honduras = "es-HN";
        public const string Mexico = "es-MX";
        public const string Nicaragua = "es-NI";
        public const string Panama = "es-PA";
        public const string Peru = "es-PE";
        public const string PuertoRico = "es-PR";
        public const string Paraguay = "es-PY";
        public const string ElSalvador = "es-SV";
        public const string UnitedStates = "es-US";
        public const string Uruguay = "es-UY";
        public const string Venezuela = "es-VE";
    }

    public static class Swedish
    {
        public const string Common = "sv-SE";
    }

    public static class Sundanese
    {
        public const string Common = "su-ID";
    }

    public static class Swahili
    {
        // Choose Kenya as the common swahili locale.
        public const string Common = "sw-KE";
        public const string Tanzania = "sw-TZ";
    }

    public static class Tamil
    {
        // Using India as the common Tamil locale.
        public const string Common = "ta-IN";
        public const string SriLanka = "ta-LK";
        public const string Malaysia = "ta-MY";
        public const string Singapore = "ta-SG";
    }

    public static class Telugu
    {
        public const string Common = "te-IN";
    }

    public static class Thai
    {
        public const string Common = "th-TH";
    }

    public static class Turkish
    {
        public const string Common = "tr-TR";
    }

    public static class Ukrainian
    {
        public const string Common = "uk-UA";
    }

    public static class Urdu
    {
        // We choose Pakistan as the common Urdu locale.
        public const string Common = "ur-PK";
        public const string India = "ur-IN";
    }

    public static class Uzbek
    {
        public const string Common = "uz-UZ";
    }

    public static class Vietnamese
    {
        public const string Common = "vi-VN";
    }

    public static class Wu
    {
        public const string Common = "wuu-CN";
    }

    public static class Welsh
    {
        public const string Common = "cy-GB";
    }

    public static class Zulu
    {
        public const string Common = "zu-ZA";
    }
}