using System;
using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;

public interface IStatus : IConvertible;

public class CallReceived : IHaveChannelType
{
    public string Type => this.GetTypeName();
    /// <summary>
    /// For most calls, can be ignored and is not required to be passed back in an
    /// <see cref="Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner.IAction"/>.
    ///
    /// If you have dialed a second connection, you will then need to use this to specify the original connection vs.
    /// the second one.
    /// </summary>
    public Guid ConnectionId { get; set; }
    public bool MayBeSpam { get; set; }
    public ChannelType Channel => ChannelType.Phone;
    public Dictionary<string, string> SIPHeaders { get; set; } = [];
}

/// <summary>
/// SMSReceived means a *new* SMS to the BotNumber, either from a number we're already conversing with, or a brand new conversation.
/// Any SMSs received during a known conversation in that channel should generally be handled as Answers.
/// </summary>
public class SMSReceived : IHaveChannelType
{
    public Guid ConnectionId { get; set; }
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

/// <summary>
/// A state that has a relationship to a specific connection.
/// </summary>
public interface IConnectionState : IState
{
    public Guid ConnectionId { get; set; }
}

public interface IHaveChannelType : IConnectionState
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
    public Guid ConnectionId { get; set; }
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

/// <summary>
/// Unable to connect to the dialed connection; no Id provided as it can only be the second connection.
/// </summary>
public class ConnectionMissed : IState
{
    public string Type => this.GetTypeName();
}

public class ConnectionMade : IState
{
    public Guid ConnectionId { get; set; }
    public string Type => this.GetTypeName();
}

public class ConnectionClosed : IConnectionState
{
    public Guid ConnectionId { get; set; }
    public string Type => this.GetTypeName();
}

/// <summary>
/// No audio detected on the other end for the first 5 seconds - possible autodialer
/// </summary>
public class NoAudioDetected : IConnectionState
{
    public Guid ConnectionId { get; set; }
    public string Type => this.GetTypeName();

}