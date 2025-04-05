using System;
using System.Linq;
using Veloquix.BotRunner.SDK.Contracts.v1;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

namespace Veloquix.BotRunner.SDK;
public class AskBuilder
{
    private readonly IConversationContext _context;

    private readonly AskQuestion _ask;

    public AskBuilder(IConversationContext context)
    {
        _context = context;
        _ask = new AskQuestion
        {
            ChannelType = _context.RequestChannel ?? ChannelType.Unknown,
            Message = string.Empty,
            LanguageCode = _context.Language,
            AllowedInputs = [],
            AllowInterruption = false,
            CanRecord = true,
            CanRecordResponse = true,
            Name = string.Empty
        };
    }

    public AskBuilder Start(string name, ChannelType? channelType = null)
    {
        _ask.Name = name;

        //Default is already set by constructor
        if (channelType == null)
        {
            return this;
        }

        if (channelType == ChannelType.Phone && _context.Request.CurrentChannels.Phone == null)
        {
            throw new Exception("No outbound phone yet, sorry!");
        }

        if (channelType == ChannelType.SMS && _context.Request.CurrentChannels.SMS == null)
        {
            if (!_context.Actions.OfType<OpenSMSChannel>().Any())
            {
                throw new Exception("You must open the SMS channel first!");
            }
        }

        _ask.ChannelType = channelType.Value;
        return this;
    }

    public AskBuilder SetMessage(string message, string languageCode = null)
    {
        if (!string.IsNullOrWhiteSpace(languageCode))
        {
            if (!SupportedLanguages.AllTheLanguages.Contains(languageCode))
            {
                throw new Exception(
                    $"Please only use the languages made available in the {nameof(SupportedLanguages)} set.");
            }

            _ask.LanguageCode = languageCode;
        }
        _ask.Message = message;
        return this;
    }

    public AskBuilder SetFlags(bool allowInterruption = true, bool canRecord = true, bool canRecordResponse = true)
    {
        _ask.AllowInterruption = allowInterruption;
        _ask.CanRecord = canRecord;
        _ask.CanRecordResponse = canRecordResponse;
        return this;
    }

    public AskBuilder AddNumericInput(VoiceAndDTMFUsage usage = VoiceAndDTMFUsage.BothAllowed, int? minLength = null, int? maxLength = null, bool allowDecimal = false, string regexValidation = null, string finishDTMFKey = null)
    {
        _ask.AllowedInputs.Add(new NumericInput
        {
            MinLength = minLength,
            MaxLength = maxLength,
            AllowDecimal = allowDecimal,
            RegexValidation = regexValidation,
            FinishDTMFKey = finishDTMFKey,
            VoiceAndDTMFUsage = usage
        });

        return this;
    }

    public AskBuilder AddStringInput(VoiceAndDTMFUsage usage = VoiceAndDTMFUsage.BothAllowed, int? minLength = null,
        int? maxLength = null, string regexValidation = null, string finishDTMFKey = null)
    {
        _ask.AllowedInputs.Add(new StringInput
        {
            MinLength = minLength,
            MaxLength = maxLength,
            RegexValidation = regexValidation,
            FinishDTMFKey = finishDTMFKey,
            VoiceAndDTMFUsage = usage
        });

        return this;
    }

    public AskBuilder AddCLUInput(string projectName, string deploymentName, double minimumConfidence = 0.7f,
        params string[] expectedIntents)
    {
        _ask.AllowedInputs.Add(new CLUInput
        {
            DeploymentName = deploymentName,
            ProjectName = projectName,
            ExpectedIntents = expectedIntents.ToList(),
            MinimumConfidence = minimumConfidence
        });

        return this;
    }

    public AskBuilder AddYesNo(VoiceAndDTMFUsage usage)
    {
        _ask.AllowedInputs.Add(new YesNoInput
        {
            VoiceAndDTMFUsage = usage
        });

        return this;
    }

    public AskBuilder AddDTMF(VoiceAndDTMFUsage usage, params DTMFEntry[] entries)
    {
        _ask.AllowedInputs.Add(new DTMFInput
        {
            VoiceAndDTMFUsage = usage,
            AllowedEntries = entries.ToList()
        });

        return this;
    }

    public AskBuilder AddInput(IAllowedInput allowedInput)
    {
        _ask.AllowedInputs.Add(allowedInput);
        return this;
    }

    public AskQuestion Build() => _ask;
}

public static class AskBuilderExtensions
{
    public static void AddAsk(this IActions actions, AskBuilder builder) 
        => actions.Add(builder.Build());

    public static void AddTo(this AskBuilder builder, IActions actions)
        => actions.Add(builder.Build());
}