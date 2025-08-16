using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka.Services.Consumer;

public class KafkaConsumerService : IKafkaConsumerService
{
    private readonly IConsumer<string, string > _consumer;
    private readonly ILogger<KafkaConsumerService> _logger;
    private Task _consumeTask;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _disposed = false;

    public event EventHandler<KafkaMessageReceivedEventArgs> MessageReceived;
    
    public KafkaConsumerService(IConfiguration configuration, ILogger<KafkaConsumerService> logger)
    {
        _logger = logger;

        var consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = configuration["Kafka:GroupId"],
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            AutoCommitIntervalMs = 5000,
            FetchWaitMaxMs = 5,
            FetchMaxBytes = 52428800
        };
        
        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
    }

    public Task StartConsumingAsync(string topic, CancellationToken cancellationToken = default)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        
        _consumer.Subscribe(topic);
        _logger.LogInformation($"Consuming topic {topic}");

        _consumeTask = Task.Run(() => ConsumeMessages(_cancellationTokenSource.Token), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    if (consumeResult?.Message == null)
                    {
                        continue;
                    }

                    OnMessageReceived(consumeResult);
                }
                catch (ConsumeException e)
                {
                    _logger.LogError(e, "Error while consuming");
                }
            }
        }
        catch (OperationCanceledException e)
        {
            _logger.LogInformation(e, "Stopping consuming");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while consuming");
        }
        finally
        {
            _consumer.Close();
        }
    }

    public Task StopConsumingAsync()
    {
        _cancellationTokenSource?.Cancel();
        return _consumeTask ?? Task.CompletedTask;
    }

    protected virtual void OnMessageReceived(ConsumeResult<string, string> consumeResult)
    {
        MessageReceived?.Invoke(this, new KafkaMessageReceivedEventArgs()
        {
            Topic = consumeResult.Topic,
            Key = consumeResult.Message.Key,
            Value = consumeResult.Message.Value,
            Partition = consumeResult.Partition.Value,
            Offset = consumeResult.Offset.Value,
            Timestamp = consumeResult.Message.Timestamp.UtcDateTime,
        });
    }
    
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        
        _consumer.Dispose();
        _consumeTask.Dispose();
        _cancellationTokenSource.Dispose();
        
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}