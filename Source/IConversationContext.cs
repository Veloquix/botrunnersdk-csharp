using System;
using Veloquix.BotRunner.SDK.Contracts.v1;
using Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

namespace Veloquix.BotRunner.SDK;

public interface IConversationContext
{
    Guid ConversationId { get; }
    WebhookRequest Request { get; }
    string Language { get; set; }
    /// <summary>
    /// The name of the last <see cref="IAction"/> taken by BotRunner.
    /// </summary>
    string LastAction { get; }
    /// <summary>
    /// The current channel that BotRunner is reaching out about. The user answer a question via SMS, voice has finished speaking, etc.
    /// <para>
    /// If this is null, that means the message is agnostic ("Ready" and "Stalled" mean that all channels are idle), and
    /// the conversation is currently multichannel. That means you'll have to select the channel to respond on rather than
    /// rely on this property.
    /// </para>
    /// </summary>
    ChannelType? RequestChannel { get; }
    ITags Tags { get; }
    IVariables Variables { get; }
    IActions Actions { get; }
    IMessageSource Messages { get; }
    Response BuildResponse();
}