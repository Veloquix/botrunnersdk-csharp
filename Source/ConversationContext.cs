using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Veloquix.BotRunner.SDK.Contracts.v1;
using Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;
using Incoming = Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner.WebhookRequest;
using Outgoing = Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner.Response;

namespace Veloquix.BotRunner.SDK;

public class ConversationContext: IConversationContext
{
    public ConversationContext(Incoming incoming, IMessageSource messages)
    {
        ConversationId = incoming.ConversationId;
        Request = incoming;
        Variables = new SDK.Variables(incoming.Variables);
        Tags = new Tags(incoming.Tags);
        Actions = new Actions(this);
        if (incoming.State is IHaveChannelType ct)
        {
            RequestChannel = ct.Channel;
        } 
        else if (incoming.CurrentChannels.Phone is not null && incoming.CurrentChannels.SMS is null)
        {
            RequestChannel = ChannelType.Phone;
        }
        else if (incoming.CurrentChannels.SMS is not null && incoming.CurrentChannels.Phone is null)
        {
            RequestChannel = ChannelType.SMS;
        }
        LastAction = incoming.LastAction;
        Language = Variables[StandardVariables.CurrentLanguage] ?? SupportedLanguages.English.UnitedStates;
        Messages = messages;
    }
    public Guid ConversationId { get; }
    public Incoming Request { get; }
    public JsonSerializerOptions SerializerOptions { get; }

    public string Language
    {
        get
        {
            if (!Variables.ContainsKey(StandardVariables.CurrentLanguage))
            {
                Variables[StandardVariables.CurrentLanguage] = SupportedLanguages.English.UnitedStates;
            }

            return SupportedLanguages.English.UnitedStates;
        }
        set => Variables[StandardVariables.CurrentLanguage] = value;
    }

    public int Failures
    {
        get
        {
            if (!Variables.ContainsKey(StandardVariables.FailureCount))
            {
                return 0;
            }

            var failures = Convert.ToInt32(Variables[StandardVariables.FailureCount]);
            return Convert.ToInt32(failures);
        }

        set => Variables[StandardVariables.FailureCount] = value.ToString();
    }

    public string LastAction { get; }
    public ChannelType? RequestChannel { get; }
    public ITags Tags { get; }
    public IVariables Variables { get; }
    public IActions Actions { get; }
    public IMessageSource Messages { get; }

    public Outgoing BuildResponse()
    {
        var outgoing = new Outgoing
        {
            ConversationId = Request.ConversationId,
            Actions = Actions.ToList()
        };

        ProcessData(outgoing);
        return outgoing;
    }

    private void ProcessData(Outgoing outgoing)
    {
        var deletedVariables = Request.Variables.Keys.Where(k => !Variables.ContainsKey(k)).ToList();
        var existingVariables = Request.Variables.Keys.Where(k => Variables.ContainsKey(k)).ToHashSet();

        if (deletedVariables.Count > 0)
        {
            outgoing.DataUpdates.Add(new RemoveVariables { Names = deletedVariables });
        }

        var updates = new Dictionary<string, Contracts.v1.Variable>();

        foreach (var variable in Variables)
        {
            try
            {
                // if the variable wasn't previously defined, or it has changed, we add it to the SetVariables call.
                if (!existingVariables.Contains(variable.Key) ||
                    !variable.Value.Value.SequenceEqual(Request.Variables[variable.Key].Value))
                {
                    updates.Add(variable.Key, variable.Value.ToContract());
                    continue;
                }
            }
            catch (Exception e)
            {
                int test = 9;
            }
        }

        if (updates.Count > 0)
        {
            outgoing.DataUpdates.Add(new SetVariables
            {
                Variables = updates
            });
        }

        var originals = Tags.OriginalState.GroupBy(s => s);

        var newTags = new List<string>();
        var deletedTags = new List<string>();

        foreach (var original in originals)
        {
            var amountInTags = Tags.Count(t => t.Equals(original.Key));

            var adds = amountInTags - original.Count();

            if (adds == 0)
            {
                continue;
            }

            if (adds < 0)
            {
                for (var d = 0; d < Math.Abs(adds); d++)
                {
                    deletedTags.Add(original.Key);
                }
            }
            else if (adds > 0)
            {
                for (var a = 0; a < adds; a++)
                {
                    newTags.Add(original.Key);
                }
            }
        }

        var oHash = Tags.OriginalState.ToHashSet();

        foreach (var tag in Tags.Where(t => !oHash.Contains(t)))
        {
            newTags.Add(tag);
        }

        if (newTags.Count > 0)
        {
            outgoing.DataUpdates.Add(new AddTags
            {
                Tags = newTags
            });
        }

        if (deletedTags.Count > 0)
        {
            outgoing.DataUpdates.Add(new RemoveTags
            {
                Tags = deletedTags
            });
        }
    }
}