using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

public class AskQuestion : IAction
{
    public string Name { get; set; }
    public string Voice { get; set; }
    public required string Message { get; set; }
    public required string LanguageCode { get; set; }
    public required ChannelType ChannelType { get; set; }
    public VoiceChannelOverrides VoiceChannelOverrides { get; set; }
    public SMSChannelOverrides SMSChannelOverrides { get; set; }
    public bool CanRecord { get; set; }
    public bool CanRecordResponse { get; set; }
    public bool AllowInterruption { get; set; }
    public List<IAllowedInput> AllowedInputs { get; set; }
    public string Type => this.GetTypeName();
}

public interface IAllowedInput : IConvertible;

public enum VoiceAndDTMFUsage { Unknown, DTMFAllowed, VoiceAllowed, BothAllowed }

public class NumericInput : IAllowedInput
{
    public int? MinLength { get; set; }
    public int? MaxLength { get; set; }
    public bool AllowDecimal { get; set; }
    public string RegexValidation { get; set; }
    public string FinishDTMFKey { get; set; }
    public VoiceAndDTMFUsage VoiceAndDTMFUsage { get; set; }
    public string Type => this.GetTypeName();
}

public class StringInput : IAllowedInput
{
    public int? MinLength { get; set; }
    public int? MaxLength { get; set; }
    public string RegexValidation { get; set; }
    public string FinishDTMFKey { get; set; }
    public VoiceAndDTMFUsage VoiceAndDTMFUsage { get; set; }
    public string Type => this.GetTypeName();
}

public class YesNoInput : IAllowedInput
{
    public VoiceAndDTMFUsage VoiceAndDTMFUsage { get; set; }
    public string Type => this.GetTypeName();
}

public class DTMFInput : IAllowedInput
{
    public List<DTMFEntry> AllowedEntries { get; set; }
    public VoiceAndDTMFUsage VoiceAndDTMFUsage { get; set; }
    public string Type => this.GetTypeName();
}

public class DTMFEntry
{
    public required string Key { get; set; }
    public required List<string> Keywords { get; set; }
}

public class CLUInput : IAllowedInput
{
    public string ProjectName { get; set; }
    public string DeploymentName { get; set; }
    public List<string> ExpectedIntents { get; set; }
    public double MinimumConfidence { get; set; } = 0.7f;
    public string Type => this.GetTypeName();
}

public class VoiceChannelOverrides
{
    /// <summary>
    /// Time before we fail out an input due to silence.
    /// This includes the user not speaking, background noise, unintelligible speech, etc.
    /// </summary>
    public int? SilenceTimeoutInMilliseconds { get; set; }

    /// <summary>
    /// Time before we consider an input complete. For voice this means the silence between words,
    /// for SMS and chat, if this is provided, we can stitch together multiple messages from a user
    /// and treat them as a single input.
    /// </summary>
    public int? SegmentationSilenceTimeoutInMilliseconds { get; set; }
}

public class SMSChannelOverrides
{
    public int? SilenceTimeoutInSeconds { get; set; }
    public int? SegmentationSilenceInSeconds { get; set; }
}