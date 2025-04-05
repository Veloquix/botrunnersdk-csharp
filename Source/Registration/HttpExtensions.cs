using System;
using System.IO;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Veloquix.BotRunner.SDK.Authentication;
using Veloquix.BotRunner.SDK.Contracts.v1.FromBotRunner;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

namespace Veloquix.BotRunner.SDK.Registration;

public static class HttpExtensions
{
    public static WebApplication UseBotRunnerHook(this WebApplication app, string route)
    {
        app.MapPost(route, async ctx =>
        {
            
            if (!await ctx.ValidateToken())
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await ctx.Response.CompleteAsync();
                return;
            }

            var scoped = app.Services.CreateScope();
            var incoming = await ctx.Request.ReadFromJsonAsync<WebhookRequest>(Contracts.v1.Constants.Options);
            var routing = scoped.ServiceProvider.GetService<IWebHookRouter>();
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
                    $"Either a {nameof(IWebHookHandler)} or a {nameof(IWebHookRouter)} needs to be registered. Methods are available when you call {nameof(ServiceExtensions.StartBotRunner)} to register one of the two.");
            }


            Response response = null;
            var context = new ConversationContext(incoming);

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
                await writer.FlushAsync();
                await ctx.Response.CompleteAsync();
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