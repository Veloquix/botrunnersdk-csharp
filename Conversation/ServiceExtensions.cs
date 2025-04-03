using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Veloquix.BotRunner.SDK.Authentication;

namespace Veloquix.BotRunner.SDK.Conversation;

public static class ServiceExtensions
{
    public static BotRunnerService StartBotRunner(this IServiceCollection services, string accountId, string applicationId, IMessageSource source = null)
    {
        var br = new BotRunnerService(services);

        JwtAuth.Setup(accountId, applicationId);
        services.AddSingleton(source ?? StaticMessageSource.Instance);
        return br;
    }

    [Experimental("VELOQUIX_ONLY")]
    public static BotRunnerService StartBotRunnerForEnvironment(this IServiceCollection services, string accountId, string applicationId, 
        string environmentSuffix, IMessageSource source = null)
    {
        var br = new BotRunnerService(services);
        JwtAuth.Setup(accountId, applicationId, environmentSuffix);
        services.AddSingleton(source ?? StaticMessageSource.Instance);
        return br;
    }
}