using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Shared.Deserialization;

public sealed class JsonMessageDeserializer<T> : IDeserializer<T>
    where T : class
{
    private readonly ILogger<JsonMessageDeserializer<T>> _logger;

    public JsonMessageDeserializer(ILogger<JsonMessageDeserializer<T>> logger)
    {
        _logger = logger;
    }

    public T? Deserialize(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return null;

        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(
                body,
                SharedJsonOptions.Default);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to deserialize message body to {MessageType}.",
                typeof(T).Name);

            return null;
        }
    }
}