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
    void Remove(string key);

    /// <summary>
    /// Reset the variables collection back to the contents of <see cref="OriginalState"/>; no data mods will go back to BotRunner in this case.
    /// </summary>
    void Revert();
}

internal class Variables : IVariables
{
    private readonly Dictionary<string, Variable> _currentState = [];

    internal Variables(Dictionary<string, Contracts.v1.Variable> originalVariables)
    {
        OriginalState = originalVariables.ToDictionary(k => k.Key, v => new Variable(v.Value));
        Revert();
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

    public void Remove(string key)
        => _currentState.Remove(key);
    
    public void Revert()
    {
        _currentState.Clear();
        foreach (var kvp in OriginalState)
        {
            this[kvp.Key] = Variable.Create(kvp.Value.Value, kvp.Value.IsSensitive);
        }
    }

    public IEnumerator<KeyValuePair<string, Variable>> GetEnumerator()
        => _currentState.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
