using Confluent.Kafka;

namespace Infrastructure.Kafka.Services.Producer;

public interface IKafkaProducer : IDisposable
{
    Task<DeliveryResult<string, string>> ProduceAsync(string topic, string key, string value);
    Task<DeliveryResult<string, TValue>> ProduceAsync<TValue>(string topic, string key, TValue value);
}