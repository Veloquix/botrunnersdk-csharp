using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK.Conversation;
public interface IMessageSource
{
    IMessage Get(string key);
}

public interface IMessage
{
    string Name { get; }
    Dictionary<string, string> ByLanguageCode { get; set; }
}
