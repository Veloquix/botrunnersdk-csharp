using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veloquix.BotRunner.SDK.Contracts;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;
using Veloquix.BotRunner.SDK.Contracts.v1;

namespace Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;
public class Talk : IConnectionAction
{
    public Guid? ConnectionId { get; set; }
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