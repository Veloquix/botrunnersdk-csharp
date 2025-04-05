using System.Collections.Generic;

namespace Veloquix.BotRunner.SDK;
public interface IMessageSource
{
    IMessage Get(string key);
}

public interface IMessage
{
    string Name { get; }
    Dictionary<string, string> ByLanguageCode { get; set; }
}
