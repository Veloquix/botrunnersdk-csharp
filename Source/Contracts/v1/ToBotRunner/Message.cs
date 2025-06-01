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

public interface IConnectionAction : IAction
{
    Guid? ConnectionId { get; set; }
}

public class PlayAudio : IConnectionAction
{
    public Guid? ConnectionId { get; set; }
    public string Type => this.GetTypeName();
    public string Name { get; set; }
    public AudioFormat Format { get; set; }
    public string AudioAsBase64 { get; set; }
    public required ChannelType ChannelType { get; set; }
}

public enum AudioFormat { Unknown, Raw16bit8kPCM }

public class OpenSMSChannel : IConnectionAction
{
    public Guid? ConnectionId { get; set; }
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

public class CloseChannel : IConnectionAction
{
    public Guid? ConnectionId { get; set; }
    public string Name { get; set; }
    public ChannelType Channel { get; set; }
    public string Type => this.GetTypeName();
}

public class EndConversation : IAction
{
    public Guid? ConnectionId { get; set; }
    public string Name { get; set; }
    public string Type => this.GetTypeName();
}