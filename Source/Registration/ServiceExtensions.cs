using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Veloquix.BotRunner.SDK.Authentication;

namespace Veloquix.BotRunner.SDK.Registration;

public static class ServiceExtensions
{
    public static BotRunnerService StartBotRunner(this IServiceCollection services, string accountId, string applicationId, 
        IMessageSource source = null, JsonSerializerOptions variableSerializerOptions = null)
    {

        if (variableSerializerOptions is not null)
        {
            Variable.SetOptions(variableSerializerOptions);
        }

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