using System.Collections.Generic;
using System.Linq;
using Veloquix.BotRunner.SDK.Contracts.v1;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

namespace Veloquix.BotRunner.SDK;

public interface IActions : IList<IAction>
{
    IActions AddSay(string name, string message, ChannelType? channelType = null);
    /// <summary>
    /// As you can't ask multiple questions in one turn, AddAsk returns the parent context so you can build and return the response in one line.
    /// </summary>
    IConversationContext AddAsk(string name, string message, IEnumerable<IAllowedInput> allowedInputs, bool allowInterruption = false);
    AskBuilder CreateBuilder(string name, ChannelType? channelType = null);
    IActions AddOpenSMSChannel(string name, string fromNumber = null, string toNumber = null);
    /// <summary>
    /// As you can't take actions after ending the conversation, AddEndConversation returns the parent context so you can build and return the response in one line.
    /// </summary>
    IConversationContext AddEndConversation(string name);
}

internal class Actions(IConversationContext ctx) : List<IAction>, IActions
{
    public IActions AddSay(string name, string message, ChannelType? channelType = null)
    {
        Add(new Talk
        {
            Name = name,
            LanguageCode = ctx.Language,
            Message = message,
            ChannelType = channelType ?? ctx.RequestChannel ?? ChannelType.Unknown
        });
        return this;
    }


    public IConversationContext AddAsk(string name, string message, IEnumerable<IAllowedInput> allowedInputs,
        bool allowInterruption = true)
    {
        Add(new AskQuestion
        {
            Name = name,
            LanguageCode = ctx.Language,
            AllowInterruption = allowInterruption,
            ChannelType = ctx.RequestChannel ?? ChannelType.Unknown,
            AllowedInputs = allowedInputs.ToList(),
            CanRecord = true,
            CanRecordResponse = true,
            Message = message
        });
        return ctx;
    }

    public AskBuilder CreateBuilder(string name, ChannelType? channelType = null)
        => new AskBuilder(ctx).Start(name, channelType);

    public IActions AddOpenSMSChannel(string name, string fromNumber = null, string toNumber = null)
    {
        Add(new OpenSMSChannel
        {
            Name = name,
            FromNumber = fromNumber ?? ctx.Request.CurrentChannels.Phone.BotNumber,
            ToNumber = toNumber ?? ctx.Request.CurrentChannels.Phone.UserNumber
        });
        return this;
    }

    public IConversationContext AddEndConversation(string name)
    {
        Add(new EndConversation { Name = name });
        return ctx;
    }
}
