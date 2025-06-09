using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace finrv.Infra.Helpers;

public class CustomDeserializer<TEvent>(ILogger<CustomDeserializer<TEvent>> logger): IDeserializer<TEvent>
where TEvent : class
{
    public TEvent Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        try
        {
            string jsonString = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<TEvent>(jsonString);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Error on deserialize | Object: {Object}", nameof(TEvent));
            throw new SerializationException("Error on deserialize | Object.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on deserialize | Object: {Object}", nameof(TEvent));
            throw new SerializationException("Error on deserialize | Object.", ex);
        }
    }
}