namespace Veloquix.BotRunner.SDK.Contracts.v1.ToBotRunner;
public class Connect : IAction
{
    public string Name { get; set; }
    public string Type => this.GetTypeName();
}
