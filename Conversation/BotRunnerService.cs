using Microsoft.Extensions.DependencyInjection;

namespace Veloquix.BotRunner.SDK.Conversation;

public class BotRunnerService(IServiceCollection services)
{
    private bool _hasRegistration = false;
    public BotRunnerService WithRouting<TRouting>() where TRouting : class, IRouting
    {
        if (_hasRegistration)
        {
            throw new VeloquixException("A router or handler has already been specified!");
        }

        _hasRegistration = true;
        services.AddScoped<IRouting, TRouting>();
        return this;
    }

    public BotRunnerService WithHandler<THandler>() where THandler : class, IWebHookHandler
    {
        if (_hasRegistration)
        {
            throw new VeloquixException("A router or handler has already been specified!");
        }

        _hasRegistration = true;
        services.AddScoped<IWebHookHandler, THandler>();
        return this;
    }

    public IServiceCollection Finish()
    {
        if (!_hasRegistration)
        {
            throw new VeloquixException("Neither a router nor a handler has been specified!");
        }

        return services;
    }
}