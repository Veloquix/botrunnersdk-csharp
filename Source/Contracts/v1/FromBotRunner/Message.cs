using System;
using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;
public class WebhookRequest
{
    public Guid ConversationId { get; set; }
    public Channels CurrentChannels { get; set; }
    public Dictionary<string, Variable> Variables { get; set; }
    public List<string> Tags { get; set; }
    public string LastAction { get; set; } = Constants.NoAction;
    public IState State { get; set; }
}

public class Channels
{
    public PhoneChannel Phone { get; set; }
    public SMSChannel SMS { get; set; }
}

public interface IState : IConvertible;

public class PhoneChannel
{
    public string UserNumber { get; set; }
    public string BotNumber { get; set; }
    public bool IsOutbound { get; set; }
    public DateTimeOffset ChannelStarted { get; set; }
    public DateTimeOffset? ChannelEnded { get; set; }
}

public class SMSChannel
{
    public string UserNumber { get; set; }
    public string BotNumber { get; set; }
    public bool IsOutbound { get; set; }
    public DateTimeOffset ChannelStarted { get; set; }
    public DateTimeOffset? ChannelEnded { get; set; }
}