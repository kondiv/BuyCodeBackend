namespace Infrastructure.Kafka.Services.Consumer;

public interface IKafkaConsumerService : IDisposable
{
    Task StartConsumingAsync(string topic, CancellationToken cancellationToken = default);
    Task StopConsumingAsync();
    event EventHandler<KafkaMessageReceivedEventArgs> MessageReceived;
}