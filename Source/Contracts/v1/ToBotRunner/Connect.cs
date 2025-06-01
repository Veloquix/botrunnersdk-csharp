namespace Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

/// <summary>
/// Joins the two Connections of a conversation together.
/// </summary>
public class Connect : IAction
{
    public string Name { get; set; }
    /// <summary>
    /// What to do if one side hangs up
    /// </summary>
    public CloseStrategy Strategy { get; set; }

    /// <summary>
    /// Not yet supported; if provided, and it differs from the language on the call, we'll run the audio through a translation service.
    /// <para>
    /// Note that this means that both sides will be speaking to a generated voice.
    /// </para>
    /// </summary>
    public string AgentLanguage { get; set; }
    public string Type => this.GetTypeName();
}

public enum CloseStrategy
{
    /// <summary>
    /// The conversation should end as soon as either side hangs up
    /// </summary>
    EndOnHangup,
    /// <summary>
    /// If one side hangs up, keep the conversation alive. We'll send back a
    /// <see cref="FromBotRunner.ConnectionClosed"/> message.
    /// </summary>
    KeepAlive,
    /// <summary>
    /// The conversation should end if the user hangs up. If the agent hangs up, we'll send back a
    /// <see cref="FromBotRunner.ConnectionClosed"/> message.
    /// </summary>
    KeepAliveForUser,
    /// <summary>
    /// The conversation should end if the agent hangs up. If the user hangs up, we'll send back a
    /// <see cref="FromBotRunner.ConnectionClosed"/> message.
    /// </summary>
    KeepAliveForAgent
}