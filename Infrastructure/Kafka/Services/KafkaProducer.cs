using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka.Services;

internal sealed class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private bool _disposed;

    public KafkaProducer(KafkaSettings settings, ILogger<KafkaProducer> logger)
    {
        _logger = logger;

        var producerConfig = new ProducerConfig()
        {
            BootstrapServers = settings.BootstrapServers,
            ClientId = settings.ClientId,
            Acks = Acks.All,
            EnableIdempotence = true,
            MessageSendMaxRetries = 3,
            RetryBackoffMaxMs = 1500
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        _jsonSerializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }
    
    public async Task<DeliveryResult<string, string>> ProduceAsync(string topic, string key, string value)
    {
        try
        {
            return await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = key,
                Value = value,
                Timestamp = new Timestamp(DateTime.UtcNow)
            });
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(ex, "Error during sending message into topic {Topic}", topic);
            throw;
        }
    }

    public async Task<DeliveryResult<string, TValue>> ProduceAsync<TValue>(string topic, string key, TValue value)
    {
        var serializedValue = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        var result = await ProduceAsync(topic, key, serializedValue);

        return new DeliveryResult<string, TValue>()
        {
            Topic = result.Topic,
            Partition = result.Partition,
            Offset = result.Offset,
            Timestamp = result.Timestamp,
            Message = new Message<string, TValue>()
            {
                Key = result.Message.Key,
                Headers = result.Message.Headers,
            }
        };
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}