using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

namespace Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;
/// <summary>
/// Allows you to enable/disable passing DTMF from one Connection to another.
/// <para>Only available when two Connections are joined.</para>
/// </summary>
public class SetDTMFStrategy : IConnectionAction
{
    public Guid? ConnectionId { get; set; }
    public string Name { get; set; }
    public bool AllowDTMF { get; set; }
    public string Type => this.GetTypeName();
}
