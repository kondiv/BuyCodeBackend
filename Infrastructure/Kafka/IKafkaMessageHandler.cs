namespace Infrastructure.Kafka;

public interface IKafkaMessageHandler
{
    Task HandleMessageAsync(string topic, string key, string value);
}