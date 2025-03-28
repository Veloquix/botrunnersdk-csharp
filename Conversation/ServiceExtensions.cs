using Microsoft.Extensions.DependencyInjection;

namespace Veloquix.BotRunner.SDK.Conversation;

public static class ServiceExtensions
{
    public static BotRunnerService StartBotRunner(this IServiceCollection services, IMessageSource source = null)
    {
        var br = new BotRunnerService(services);
        
        services.AddSingleton(source ?? StaticMessageSource.Instance);
        return br;
    }
}