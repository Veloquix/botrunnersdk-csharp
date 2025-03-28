using System;
using System.Linq;
using Veloquix.BotRunner.SDK.Contracts.v1;
using Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;
using Veloquix.BotRunner.SDK.Conversation;

namespace Veloquix.BotRunner.SDK.Variables;

public static class VariableConverters
{
    public static bool TryToDateTime(this Variable variable, out DateTimeOffset dateTime)
    {

        if (string.IsNullOrWhiteSpace(variable?.Value))
        {
            dateTime = DateTimeOffset.MinValue;
            return false;
        }

        return DateTimeOffset.TryParse(variable.Value, out dateTime);
    }
}


public static class AnswerConverters
{
    public static bool TryToDateTime(this IState state, string entityName, out DateTimeOffset dateTimeOffset, bool takeFirst = false)
    {
        dateTimeOffset = DateTimeOffset.MinValue;
        switch (state)
        {
            case CLUAnswerReceived cluAnswerReceived:
                var entityBase = cluAnswerReceived.Prediction.Entities
                    .Where(e => e.Name.Equals(entityName, StringComparison.InvariantCultureIgnoreCase));
                var entity = takeFirst
                    ? entityBase.FirstOrDefault()
                    : entityBase.LastOrDefault();

                if (entity is null)
                {
                    return false;
                }

                var dateTimeStr = entity.Resolutions.OfType<DateTimeResolution>()
                    .FirstOrDefault()?.Value;

                if (string.IsNullOrWhiteSpace(dateTimeStr))
                {
                    return false;
                }

                return DateTimeOffset.TryParse(dateTimeStr, out dateTimeOffset);
            case NumericAnswerReceived numericAnswerReceived:
                return DateTimeOffset.TryParse(numericAnswerReceived.Value.ToString(), out dateTimeOffset);
            case StringAnswerReceived stringAnswerReceived:
                return DateTimeOffset.TryParse(stringAnswerReceived.Value, out dateTimeOffset);
            case YesNoAnswerReceived yesNoAnswerReceived:
            case DTMFAnswerReceived dtmfAnswerReceived:
            case SMSReceived smsReceived:
            case CallReceived callReceived:
            case ChannelTransferred channelTransferred:
            case ConversationEnded conversationEnded:
            case InvalidAnswerReceived invalidAnswerReceived:
            case Ready ready:
            case Stalled stalled:
            default:
                return false;
        }
    }
}
