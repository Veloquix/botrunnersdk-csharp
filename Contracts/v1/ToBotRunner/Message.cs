using System;
using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;
public class Response
{
    public required Guid ConversationId { get; set; }
    public List<IAction> Actions { get; set; } = new();
    public List<IDataUpdate> DataUpdates { get; set; } = new();
}

public interface IDataUpdate : IConvertible
{
}

public class SetVariables : IDataUpdate
{
    public Dictionary<string, Variable> Variables { get; set; } = [];
    public string Type => this.GetTypeName();
}

public class RemoveVariables : IDataUpdate
{
    public List<string> Names { get; set; } = [];
    public string Type => this.GetTypeName();
}

public class AddTags : IDataUpdate
{
    public List<string> Tags { get; set; } = [];
    public string Type => this.GetTypeName();
}

public class RemoveTags : IDataUpdate
{
    public List<string> Tags { get; set; } = [];
    public string Type => this.GetTypeName();
}

public interface IAction : IConvertible
{
    string Name { get; }
}

public class Talk : IAction
{
    public string Name { get; set; }
    /// <summary>
    /// Only relevant for voice-based channels (currently, only Telephony). If null or empty,
    /// the value chosen when BotRunner was configured, or the Veloquix default, will be used.
    /// </summary>
    public string Voice { get; set; }
    public required string Message { get; set; }
    public required string LanguageCode { get; set; }
    public required ChannelType ChannelType { get; set; }
    public bool CanRecord { get; set; }
    public string Type => this.GetTypeName();
}
public class OpenSMSChannel : IAction
{
    public string Name { get; set; }
    public string Type => this.GetTypeName();
    /// <summary>
    /// Optional; if unspecified, number will match the existing phone channel's Bot Number.
    /// </summary>
    public string FromNumber { get; set; }
    /// <summary>
    /// Optional; if unspecified, number will be the user's number.
    /// </summary>
    public string ToNumber { get; set; }
}

public class CloseChannel : IAction
{
    public string Name { get; set; }
    public ChannelType Channel { get; set; }
    public string Type => this.GetTypeName();
}

public class EndConversation : IAction
{
    public string Name { get; set; }
    public string Type => this.GetTypeName();
}