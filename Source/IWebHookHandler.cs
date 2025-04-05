using System.Threading.Tasks;
using Outgoing = Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner.Response;

namespace Veloquix.BotRunner.SDK;

public interface IWebHookHandler
{
    Task<Outgoing> HandleAsync(IConversationContext context);
}
