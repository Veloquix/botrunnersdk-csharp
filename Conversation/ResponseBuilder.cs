using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Veloquix.BotRunner.SDK.Contracts.v1;
using Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;

namespace Veloquix.BotRunner.SDK.Conversation;


public interface IResponseBuilder
{
    ResponseBuilder AddTalk(string message, bool canRecord = true, ChannelType fallbackChannel = ChannelType.Phone);
    ResponseBuilder AddTalk(IMessage message, bool canRecord = true, ChannelType fallbackChannel = ChannelType.Phone);
    ResponseBuilder SetVariable(string name, string variable);
    ResponseBuilder SetVariable(string name, Variable variable);
    ResponseBuilder RemoveVariables(params string[] names);
    ResponseBuilder AddTag(string tag);
    ResponseBuilder RemoveTag(string tag);

    Response Build();
}

public class ResponseBuilder(IConversationContext ctx)  : IResponseBuilder
{
    private readonly List<IAction> _actions = [];
    private readonly ConcurrentBag<(string name, Variable variable)> _sets = [];
    private readonly ConcurrentBag<string> _removes = [];
    private readonly ConcurrentBag<string> _newTags = [];
    private readonly ConcurrentBag<string> _removedTags = [];

    public ResponseBuilder AddTalk(string message, bool canRecord, ChannelType fallbackChannel = ChannelType.Phone)
    {
        var channelType = ctx.RequestChannel;

        if (channelType == ChannelType.Unknown)
        {
            channelType = fallbackChannel;
        }
        _actions.Add(new Talk
        {
            LanguageCode = ctx.Language,
            Message = message,
            CanRecord = canRecord,
            ChannelType = channelType
        });

        return this;
    }

    public ResponseBuilder AddTalk(IMessage message, bool canRecord = true, ChannelType fallbackChannel = ChannelType.Phone)
    {
        var text = message.ByLanguageCode.GetValueOrDefault(ctx.Language);

        return AddTalk(text, canRecord, fallbackChannel);
    }

    public ResponseBuilder SetVariable(string name, string variable)
    {
        _sets.Add((name, new Variable { Value = variable, IsSensitive = false }));
        return this;
    }

    public ResponseBuilder SetVariable(string name, Variable variable)
    {
        _sets.Add((name, variable));
        return this;
    }

    public ResponseBuilder RemoveVariables(params string[] names)
    {
        foreach (var name in names)
        {
            _removes.Add(name);
        }

        return this;
    }

    public ResponseBuilder AddTag(string tag)
    {
        _newTags.Add(tag);
        return this;
    }

    public ResponseBuilder RemoveTag(string tag)
    {
        _removedTags.Add(tag);
        return this;
    }

    public Response Build()
    {
        var update = new SetVariables();
        foreach (var (key, var) in _sets)
        {
            update.Variables[key] = var;
        }

        var delete = new RemoveVariables();
        foreach (var key in _removes)
        {
            delete.Names.Add(key);
        }

        var addTags = new AddTags()
        {
            Tags = _newTags.Distinct().ToList() 
        };

        var removeTags = new RemoveTags
        {
            Tags = _removedTags.Distinct().ToList()
        };


        return new Response()
        {
            ConversationId = ctx.ConversationId,
            Actions = _actions,
            DataUpdates = [ update, delete, addTags, removeTags ]
        };
    }
}