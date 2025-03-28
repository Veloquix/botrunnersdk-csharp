using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veloquix.BotRunner.SDK.Contracts.v1;
using Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;
using Incoming = Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner.WebhookRequest;
using Outgoing = Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner.Response;

namespace Veloquix.BotRunner.SDK.Conversation;

public interface IConversationContext
{
    Incoming Request { get; }
    Variable GetVariable(string name);
    bool HasTag(string tag);
    void SetLanguage(string language);
    Guid ConversationId { get; }
    string Language { get; }
    ChannelType RequestChannel { get; }
    IMessageSource Messages { get; }
    IResponseBuilder Response { get; }
}

internal class ConversationContext : IConversationContext
{
    private readonly Incoming _request;

    public ConversationContext(Incoming request, IMessageSource messageSource)
    {
        _request = request;
        Messages = messageSource;
        Response = new ResponseBuilder(this);
    }

    public Incoming Request => _request;

    public Variable GetVariable(string name)
    {
        return _request.Variables.GetValueOrDefault(name);
    }

    public bool HasTag(string tag)
    {
        return _request.Tags.Any(t => t.Equals(tag, StringComparison.InvariantCultureIgnoreCase));
    }

    public void SetLanguage(string language)
    {
        Response.SetVariable(Constants.LanguageName, language);
        var var = new Variable { Value = language };
        _request.Variables[Constants.LanguageName] = var;
    }

    public Guid ConversationId => _request.ConversationId;
    public string Language => _request.Variables.GetValueOrDefault(Constants.LanguageName)?.Value ?? "en-US";

    public ChannelType RequestChannel
    {
        get
        {
            if (Request.CurrentChannels.Phone == null && Request.CurrentChannels.SMS != null)
            {
                return ChannelType.SMS;
            }

            if (Request.CurrentChannels.SMS == null && Request.CurrentChannels.Phone != null)
            {
                return ChannelType.Phone;
            }

            if (Request.State is IHaveChannelType haveChannel)
            {
                return haveChannel.Channel;
            }

            return ChannelType.Unknown;
        }
    }
    public IMessageSource Messages { get; }

    public IResponseBuilder Response { get; }
}

public interface IRouting
{
    Func<IConversationContext, Task<Outgoing>> Route(string lastActionName);
}

public interface IWebHookHandler
{
    Task<Outgoing> HandleAsync(IConversationContext context);
}