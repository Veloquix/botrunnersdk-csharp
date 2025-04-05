using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Veloquix.BotRunner.SDK.Contracts;

/// <summary>
/// Used to prevent converting a specific child of a base class. It will not get checked when running through available types.
/// </summary>
public class ForbidConversionAttribute : Attribute { }

public class TypePropertyNameAttribute(string propertyName) : Attribute
{
    public string PropertyName { get; } = propertyName;
}

/// <summary>
/// To use this interface, you MUST provide the TypePropertyNameAttribute.
/// </summary>
public interface ICustomConvertible
{

}

[TypePropertyName(nameof(Type))]
public interface IConvertible : ICustomConvertible
{
    string Type { get; }
}


public class TypedConverter<T> : JsonConverter<T> where T : ICustomConvertible
{
    private static string DefaultTypePropertyName = nameof(IConvertible.Type);
    private readonly string _customTypePropertyName = null;
    private IReadOnlyDictionary<string, Type> _typesByName;

    public TypedConverter()
    {
        var type = typeof(T);
        var typePropertyNameAttribute = type.GetCustomAttribute<TypePropertyNameAttribute>();

        if (typePropertyNameAttribute != null)
        {
            _customTypePropertyName = typePropertyNameAttribute.PropertyName;
        }
        // We're trying to grab every class that can be instantiated from the type specified, which should be an interface or base class, and one that implements IConvertible.
        var children = Assembly
           .GetAssembly(type)
           .GetTypes()
           .Where(t => !t.IsAbstract && t.IsClass && type.IsAssignableFrom(t) && t.GetCustomAttribute<ForbidConversionAttribute>() == null)
           .ToList();

        _typesByName = children.ToDictionary(k => k.Name, v => v);
    }

    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert == typeof(T);

    public sealed override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // The code below has the task of replacing type here with the actual, derived type name, which it'll read from the "TypeName" property that should be in the JSON.
        Type type = null;

        // Return default if we're not starting an object - the only acceptable option in that case is 'null'.
        if (reader.TokenType is not JsonTokenType.StartObject)
        {
            return default;
        }

        // when you assign a reader, the original stays in the same spot. So we leave it in place for later.
        var typeSniffer = reader;
        var typePropertyName = _customTypePropertyName ?? DefaultTypePropertyName;

        while (typeSniffer.Read() && typeSniffer.TokenType != JsonTokenType.EndObject)
        {
            // Not the property name we're looking for, skip to the next property
            if (typeSniffer.TokenType != JsonTokenType.PropertyName || !string.Equals(typePropertyName, typeSniffer.GetString(), StringComparison.InvariantCultureIgnoreCase))
            {
                typeSniffer.TrySkip();
                continue;
            }

            typeSniffer.Read();
            var typeName = typeSniffer.GetString();

            if (!_typesByName.TryGetValue(typeName, out type))
            {
                throw new ArgumentOutOfRangeException(DefaultTypePropertyName, $"Unexpected Type: {typeName}. Is there a contract version mismatch?");
            }

            break;
        }

        if (type == null)
        {
            throw new JsonException($"JSON provided does not appear to be of type '{typeof(T).Name}'.");
        }

        return (T)JsonSerializer.Deserialize(ref reader, type, options);
    }

    public sealed override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value?.GetType() ?? typeof(T), options);
    }
}
