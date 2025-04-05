using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK;

public class StaticMessageSource : IMessageSource
{
    private Dictionary<string, IMessage> _messages = [];
    public static StaticMessageSource Instance = new();

    public void Load(Dictionary<string, IMessage> messages)
    {
        _messages = messages;
    }

    public IMessage Get(string key)
        => _messages.GetValueOrDefault(key);
}
