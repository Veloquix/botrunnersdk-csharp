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


public class Message : IMessage
{
    public string Name { get; }
    public Dictionary<string, string> ByLanguageCode { get; set; }
}