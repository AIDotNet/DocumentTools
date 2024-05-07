using System.Text.Json;
using FreeSql.Internal.Model;

namespace AIDotNet.Document.Services.TypeHandlers;

public class JsonTypeHandler<T> : TypeHandler<T> where T : class
{
    public override T Deserialize(object value)
    {
        return JsonSerializer.Deserialize<T>(value.ToString());
    }

    public override object Serialize(T value)
    {
        return JsonSerializer.Serialize(value);
    }
}