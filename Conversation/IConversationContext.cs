using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Veloquix.BotRunner.SDK.Contracts.v1;
using Incoming = Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner.WebhookRequest;
using Outgoing = Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner.Response;

namespace Veloquix.BotRunner.SDK.Conversation;


// GetChannel based on incoming message (IAnswer), with fallback to checking the message type
// for incoming calls, etc.
// Make clear that Channels is about the active channels, not the one from incoming
// generic "I didn't get that" handling
// generic "fail conversation" handler



public interface IConversationContext
{
    Incoming Request { get; }
    Variable GetVariable(string name);
    void SetLanguage(string language);
    Guid ConversationId { get; }
    string Language { get; }
    IMessageSource Messages { get; }
    IResponseBuilder Response { get; }
}

public interface IResponseBuilder
{
    ResponseBuilder AddTalk(string message);
    ResponseBuilder AddTalk(IMessage message);
    ResponseBuilder SetVariable(string name, Variable variable);
    ResponseBuilder RemoveVariables(params string[] names);

    Outgoing Build();
}

public class ResponseBuilder(IConversationContext ctx)  : IResponseBuilder
{
    
    public ResponseBuilder AddTalk(string message)
    {
        return this;
    }

    public ResponseBuilder AddTalk(IMessage message)
    {
        return this;
    }

    public ResponseBuilder SetVariable(string name, Variable variable)
    {
        return this;
    }

    public ResponseBuilder RemoveVariables(params string[] names)
    {
        return this;
    }

    public Outgoing Build()
    {
        return new Outgoing()
        {
            ConversationId = ctx.ConversationId
        };
    }
}

public interface IMessages
{
    IMessageSource Source { get; }
}

internal class ConversationContext : IConversationContext
{
    private readonly Incoming _request;

    public ConversationContext(Incoming request, IMessageSource messageSource)
    {
        _request = request;
        Messages = messageSource;
        Response = new ResponseBuilder(this);
    }

    public Incoming Request { get; }
    public Variable GetVariable(string name)
    {
        throw new NotImplementedException();
    }

    public void SetLanguage(string language)
    {
        throw new NotImplementedException();
    }

    public Guid ConversationId { get; }
    public string Language => _request.Variables[Constants.LanguageName].Value;
    public IMessageSource Messages { get; }

    public IResponseBuilder Response { get; }
}

public interface IRouting
{
    Func<IConversationContext, Task<Outgoing>> Route(string lastActionName);
}

public interface IWebHookHandler
{
    Task<Outgoing> HandleAsync(IConversationContext context);
}

public interface IRoutingWithMain : IRouting
{
}

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

public static class ServiceExtensions
{
    public static BotRunnerService StartBotRunner(this IServiceCollection services, IMessageSource source)
    {
        var br = new BotRunnerService(services);
        
        services.AddSingleton(source);
        return br;
    }
}

public interface IMessageSource
{
    string Get(string languageCode, string key);
}

public interface IMessage
{
    string Name { get; }
    Dictionary<string, string> ByLanguageCode { get; set; }
}


public static class HttpExtensions
{
    public static WebApplication UseBotRunnerHook(this WebApplication app, string route)
    {
        app.MapPost(route, async ctx =>
        {
            var scoped = app.Services.CreateScope();

            
            var incoming = await ctx.Request.ReadFromJsonAsync<Incoming>(Contracts.v1.Constants.Options);
            var routing = scoped.ServiceProvider.GetService<IRouting>();
            var handler = scoped.ServiceProvider.GetService<IWebHookHandler>();
            IMessageSource messages;
            try
            {
                messages = scoped.ServiceProvider.GetRequiredService<IMessageSource>();
            }
            catch (Exception ex)
            {
                throw new VeloquixException(
                    $"A valid implementation of {nameof(IMessageSource)} is required to handle a BotRunner message.");
            }

            if (routing is null && handler is null)
            {
                throw new VeloquixException(
                    $"Either a {nameof(IWebHookHandler)} or a {nameof(IRouting)} needs to be registered. Methods are available when you call {nameof(ServiceExtensions.StartBotRunner)} to register one of the two.");
            }


            Outgoing response = null;
            var context = new ConversationContext(incoming, messages);

            try
            {
                if (routing is not null)
                {
                    var route = routing.Route(incoming.LastAction);

                    response = await route(context);
                }
                else
                {
                    response = await handler.HandleAsync(context);
                }

                ctx.Response.StatusCode = (int)HttpStatusCode.OK;
                var writer = new StreamWriter(ctx.Response.Body);

                var json = JsonSerializer.Serialize(response, Contracts.v1.Constants.Options);

                await writer.WriteAsync(json);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unexpected Error while processing a BotRunner webhook request.\n\tMessage: {ex.Message}\n\tSource: {ex.Source}\n\tStack:\n\t\t{ex.StackTrace}");
                throw;
            }
        });

        return app;
    }
}


/*
 * 
 class Message(string Id)
{
    void AddTranslation(string languageCode, string message)l

}



$"Would you like to get {VAR.Name("Flavor")} or {VAR.Flavor}

*/