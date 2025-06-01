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
    /// <summary>
    /// Seconds to try the connection before giving up. Should be kept short to avoid voicemail.
    /// </summary>
    public int Timeout { get; init; }
    public string Type => this.GetTypeName();
}
