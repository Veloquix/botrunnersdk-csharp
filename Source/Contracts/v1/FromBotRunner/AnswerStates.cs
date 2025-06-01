using System;
using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;

public class InvalidAnswerReceived : IConnectionState
{
    public Guid ConnectionId { get; set; }
    public string QuestionName { get; set; }
    public ChannelType ChannelType { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string RawText { get; set; }
    public InvalidType InvalidType { get; set; }
    public string Type => this.GetTypeName();
}

public enum InvalidType
{
    Unknown,
    /// <summary>
    /// Couldn't recognize speech
    /// </summary>
    Noise,
    /// <summary>
    /// User didn't speak within the timeout
    /// </summary>
    Silence,
    /// <summary>
    /// Input was not a match - DTMF Menu didn't have a match to the voice/DTMF input from the user, the
    /// CLU Intent returned isn't the one requested, etc.
    /// </summary>
    NoMatch
}

public abstract class BaseAnswer : IHaveChannelType
{
    public Guid ConnectionId { get; set; }
    public string QuestionName { get; set; }
    public string RawText { get; set; }
    public ChannelType Channel { get; set; }
    public List<ValidationFailures> ValidationFailures { get; set; } = new();
    public bool UsedDTMF { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public abstract string Type { get; }
}

public class NumericAnswerReceived : BaseAnswer
{
    public decimal Value { get; set; } = -1;
    public override string Type => this.GetTypeName();
}

public class StringAnswerReceived : BaseAnswer
{
    public string Value { get; set; }
    public override string Type => this.GetTypeName();
}

public class YesNoAnswerReceived : BaseAnswer
{
    public bool Value { get; set; }
    public override string Type => this.GetTypeName();
}

public class DTMFAnswerReceived : BaseAnswer
{
    public string Key { get; set; }
    public override string Type => this.GetTypeName();
}

public class CLUAnswerReceived : BaseAnswer
{
    public Prediction Prediction { get; set; }
    public override string Type => this.GetTypeName();
}

public enum ValidationFailures
{
    Unknown,
    BelowMinLength,
    AboveMaxLength,
    FailedRegex,
    NoDecimalsAllowed,
    BelowMinValue,
    AboveMaxValue
}