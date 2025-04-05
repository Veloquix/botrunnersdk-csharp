using System.Collections.Generic;
using System.Linq;

namespace Veloquix.BotRunner.SDK;

public interface ITags : IList<string>
{
    IReadOnlyList<string> OriginalState { get; }

    /// <summary>
    /// Resets the Tags back to the values in <see cref="OriginalState"/>; no data mods will
    /// go back to BotRunner in this case.
    /// </summary>
    void Revert();
}

internal class Tags : List<string>, ITags
{
    internal Tags(IEnumerable<string> originalTags)
    {
        OriginalState = originalTags.ToList().AsReadOnly();

    }
    public IReadOnlyList<string> OriginalState { get; }
    public void Revert()
    {
        Clear();
        AddRange(OriginalState);
    }
}
