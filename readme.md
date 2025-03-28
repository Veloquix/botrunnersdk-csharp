# BotRunner SDK (C# Edition)
This package assists your team with standing up support for a BotRunner webhook.
> What's BotRunner, you say? Learn more [here](https://www.veloquix.com/botrunner)!

## Usage
Using this SDK is mostly about defining a path by which we can send you an incoming request. We provide a `Use` extension
where you can specify the path, and a way of standing up everything you need via normal dependency injection.

```csharp
var messageSource = services.StartBotRunner()
	.WithHandler<MyMessageHandler>()

...

app.UseBotRunnerHook("v1/botrunner/hook");

```

## IMessageSource

We provide a basic singleton option for loading up Messages. Messages are intended to be
a way of supporting i18n in your bot. If you don't want to use this pattern, that's ok; `IConversationContext.Messages` 
will just be an empty implementation.

If you do, you can either populate the one we provide with `StaticMessageSource.Instance.Load(Dictionary<string, IMessage>)`,
where the key is the name of the message so you can find it later, or you can implement and supply your own version of `IMessageSource`.


## Handler vs. Routing

We provide two ways of processing incoming webhook messages. `IWebHookHandler` specifies a single Handle method that will be expected to
return the response to BotRunner. `IRouting` allows you to route the message to different functions that can handle the inbound request
and return the response, based on the last action sent to BotRunner.

Either is effective; we use both in our own applications. If you have a more complex bot with a lot of branching, `IRouting` is
probably the right choice for you.