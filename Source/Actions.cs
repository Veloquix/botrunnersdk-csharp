using System.Collections.Generic;
using System.Linq;
using Veloquix.BotRunner.SDK.Contracts.v1;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

namespace Veloquix.BotRunner.SDK;

public interface IActions: IList<IAction>
{
    void AddSay(string name, string message, ChannelType? channelType = null);
    void AddAsk(string name, string message, IEnumerable<IAllowedInput> allowedInputs, bool allowInterruption = false);
    AskBuilder CreateBuilder(string name, ChannelType? channelType = null);
    void AddEndConversation(string name);
}

internal class Actions(IConversationContext ctx) : List<IAction>, IActions
{
    public void AddSay(string name, string message, ChannelType? channelType = null) =>
        Add(new Talk
        {
            Name = name,
            LanguageCode = ctx.Language,
            Message = message,
            ChannelType = channelType ?? ctx.RequestChannel ?? ChannelType.Unknown
        });

    public void AddAsk(string name, string message, IEnumerable<IAllowedInput> allowedInputs, bool allowInterruption = true) =>
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

    public AskBuilder CreateBuilder(string name, ChannelType? channelType = null) 
        => new AskBuilder(ctx).Start(name, channelType);

    public void AddOpenSMSChannel(string name, string fromNumber = null, string toNumber = null)
        => Add(new OpenSMSChannel
        {
            Name = name,
            FromNumber = fromNumber ?? ctx.Request.CurrentChannels.Phone.BotNumber,
            ToNumber = toNumber ?? ctx.Request.CurrentChannels.Phone.UserNumber
        });

    public void AddEndConversation(string name) => Add(new EndConversation { Name = name });
}
