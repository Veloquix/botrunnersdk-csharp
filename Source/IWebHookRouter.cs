using System;
using System.Threading.Tasks;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

namespace Veloquix.BotRunner.SDK;

public interface IWebHookRouter
{
    Func<IConversationContext, Task<Response>> Route(string lastActionName);
}