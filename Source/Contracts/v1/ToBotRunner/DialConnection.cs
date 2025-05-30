using System;

namespace Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;
/// <summary>
/// This is to allow a BotRunner to start up a new conversation
/// with another caller, with the possibility of joining the two
/// calls together afterwards.
/// </summary>
public class DialConnection : IAction
{
    public string Name { get; init; }
    public string To { get; init; }
    public int Timeout { get; init; }
    /// <summary>
    /// NOT the parent. This is the new conversation ID. We ask the upstream system to define it
    /// so we have a point of reference when messaging them about this conversation.
    /// </summary>
    public Guid ConversationId { get; set; }
    public Guid ParentConversationId { get; init; } = Guid.Empty;
    public string Type => this.GetTypeName();
}
