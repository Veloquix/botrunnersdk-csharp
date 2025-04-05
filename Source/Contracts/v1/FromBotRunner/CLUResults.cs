using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;

public class Prediction
{
    public string TopIntent { get; set; }
    public List<Intent> Intents { get; set; }
    public List<Entity> Entities { get; set; }
}

public class Intent
{
    public string Name { get; set; }
    public double ConfidenceScore { get; set; }
}

public class Entity
{
    public string Name { get; set; }
    public string Text { get; set; }
    public List<Resolution> Resolutions { get; set; } = new();
    public List<ExtraInformation> ExtraInformation { get; set; } = new();
}

public class ExtraInformation
{
    public ExtraInformationKind ExtraInformationKind { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string RegexPattern { get; set; }
}

public enum ExtraInformationKind
{
    ListKey, EntitySubtype, RegexKey
}

[TypePropertyName(nameof(ResolutionKind))]
public abstract class Resolution : ICustomConvertible
{
    public abstract string ResolutionKind { get; }
}

public class BooleanResolution : Resolution
{
    public bool Value { get; set; }
    public override string ResolutionKind => nameof(BooleanResolution);
}

public class TemporalSpanResolution : Resolution
{
    public string Begin { get; set; }
    public string End { get; set; }
    public string Timex { get; set; }
    public string Duration { get; set; }
    public override string ResolutionKind => nameof(TemporalSpanResolution);
}

public class DateTimeResolution : Resolution
{
    public string Value { get; set; }
    public string Timex { get; set; }
    public DateTimeSubKind DateTimeSubKind { get; set; }
    public override string ResolutionKind => nameof(DateTimeResolution);
}

public class NumberResolution : Resolution
{
    public NumberKind NumberKind { get; set; }
    public decimal Value { get; set; }
    public override string ResolutionKind => nameof(NumberResolution);
}

public enum NumberKind
{
    Integer,
    Decimal
}

public enum DateTimeSubKind
{
    Date,
    Time,
    DateTime
}