using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Veloquix.BotRunner.SDK;
public class Variable
{
    private static JsonSerializerOptions _options;

    static Variable()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new JsonStringEnumConverter());
    }

    public static void SetOptions(JsonSerializerOptions options)
    {
        _options = options;
    }

    internal Variable(Contracts.v1.Variable contract)
    {
        Value = contract.Value;
        IsSensitive = contract.IsSensitive;
    }

    internal Variable() { }

    public string Value { get; init; }
    public bool IsSensitive { get; init; }

    public static implicit operator string(Variable v) => v.Value;
    public static implicit operator Variable(string val) => Create(val, false);

    public T To<T>() => JsonSerializer.Deserialize<T>(Value, _options);
    public static Variable From<T>(T source, bool isSensitive = false) => Create(JsonSerializer.Serialize(source, _options), isSensitive);

    public Contracts.v1.Variable ToContract()
        => new ()
        {
            IsSensitive = IsSensitive,
            Value = Value
        };

    public static Variable Create(string value, bool isSensitive)
    {
        return new Variable
        {
            Value = value,
            IsSensitive = isSensitive
        };
    }
}

public interface IVariables : IEnumerable<KeyValuePair<string, Variable>>
{
    /// <summary>
    /// This is the state of the variables as they came in with the request.
    /// </summary>
    IReadOnlyDictionary<string, Variable> OriginalState { get; }
    
    Variable this[string key] { get; set; }
    bool ContainsKey(string key);
    void Add(string key, Variable variable);
    void Remove(params string[] keys);

    /// <summary>
    /// Reset the variables collection back to the contents of <see cref="OriginalState"/>; no data mods will go back to BotRunner in this case.
    /// <para>
    /// WARNING: Setting includeStandardVariables to true will clear out things like the current language setting.
    /// You probably don't want to set that to true.
    /// </para>
    /// </summary>
    void Revert(bool includeStandardVariables = false);
}

internal class Variables : IVariables
{
    private readonly Dictionary<string, Variable> _currentState = [];

    internal Variables(Dictionary<string, Contracts.v1.Variable> originalVariables)
    {
        OriginalState = originalVariables.ToDictionary(k => k.Key, v => new Variable(v.Value));
        Revert(true);
    }

    public IReadOnlyDictionary<string, Variable> OriginalState { get; }

    public Variable this[string key]
    {
        get => _currentState[key];
        set
        {
            if (key.Equals(StandardVariables.CurrentLanguage))
            {
                if (!SupportedLanguages.AllTheLanguages.Contains(value.Value))
                {
                    throw new Exception(
                        $"Use only supported languages. The available languages can be found in {nameof(SupportedLanguages)}");
                }

                // In case they tried to be weird and create the language variable, but set it to sensitive.
                // Thought about throwing an exception, but really it feels too minor to bother.
                _currentState[key] = value.Value;
            }

            _currentState[key] = value;
        }
    }

    public bool ContainsKey(string key)
        => _currentState.ContainsKey(key);

    public void Add(string key, Variable variable)
        => _currentState[key] = variable;

    public void Remove(params string[] keys)
    {
        foreach (var k in keys)
        {
            _currentState.Remove(k);
        }
    }
    
    public void Revert(bool includeStandardVariables)
    {
        foreach (var kvp in _currentState)
        {
            if (!includeStandardVariables && StandardVariables.All.Contains(kvp.Key))
            {
                continue;
            }

            _currentState.Remove(kvp.Key);
        }
        _currentState.Clear();
        var originalVars = !includeStandardVariables
            ? OriginalState.Where(o => !StandardVariables.All.Contains(o.Key))
            : OriginalState;
        foreach (var kvp in originalVars)
        {
            this[kvp.Key] = Variable.Create(kvp.Value.Value, kvp.Value.IsSensitive);
        }
    }

    public IEnumerator<KeyValuePair<string, Variable>> GetEnumerator()
        => _currentState.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
