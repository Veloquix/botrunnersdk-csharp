using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;

public interface IStatus : IConvertible;

public class CallReceived : IHaveChannelType
{
    public string Type => this.GetTypeName();
    public ChannelType Channel => ChannelType.Phone;
    public Dictionary<string, string> SIPHeaders { get; set; } = [];
}

/// <summary>
/// SMSReceived means a *new* SMS to the BotNumber, either from a number we're already conversing with, or a brand new conversation.
/// Any SMSs received during a known conversation in that channel should generally be handled as Answers.
/// </summary>
public class SMSReceived : IHaveChannelType
{
    public ChannelType Channel => ChannelType.SMS;
    public string Type => this.GetTypeName();
}

/// <summary>
/// Ready for more activity - no communication active
/// </summary>
public class Ready : IState
{
    public string Type => this.GetTypeName();
}

public interface IHaveChannelType : IState
{
    ChannelType Channel { get; }
}

/// <summary>
/// DrivingChannel is the most active channel (intrinsically, not per the conversation).
/// Currently that's phone, then SMS. When a conversation is inactive (not listening for a response,
/// not talking to the user), it will be ended when we reach the MaxIdleSeconds value.
/// </summary>
public class Stalled : IHaveChannelType
{
    public ChannelType Channel { get; set; }
    public int IdleForSeconds { get; set; }
    public int MaxIdleSeconds { get; set; }
    public string Type => this.GetTypeName();
}

public class ConversationEnded : IState
{
    public bool ByUser { get; set; }
    public string Type => this.GetTypeName();
}

public class ChannelTransferred : IHaveChannelType
{
    public ChannelType Channel { get; set; }
    public string Type => this.GetTypeName();
}