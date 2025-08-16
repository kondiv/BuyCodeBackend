using Infrastructure.Kafka.Services.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka.Services;

public class KafkaConsumerHostedService : BackgroundService
{
    private readonly IKafkaConsumerService _kafkaConsumerService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<KafkaConsumerHostedService> _logger;
    private readonly string _topic;

    public KafkaConsumerHostedService(
        IKafkaConsumerService kafkaConsumerService,
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<KafkaConsumerHostedService> logger)
    {
        _kafkaConsumerService = kafkaConsumerService;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _topic = configuration["Kafka:Topic"] ?? "default-topic";

        _kafkaConsumerService.MessageReceived += OnMessageReceived;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Kafka consumer has started");
        
        await _kafkaConsumerService.StartConsumingAsync(_topic, stoppingToken);
        
        _logger.LogInformation("Kafka consumer has ended its work");
    }

    private async void OnMessageReceived(object sender, KafkaMessageReceivedEventArgs e)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var messageHandler = scope.ServiceProvider.GetRequiredService<IKafkaMessageHandler>();

            await messageHandler.HandleMessageAsync(e.Topic, e.Key, e.Value);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Kafka hosted service");
        
        _kafkaConsumerService.MessageReceived -= OnMessageReceived;
        await _kafkaConsumerService.StopConsumingAsync();
        
        await base.StopAsync(cancellationToken);
    }
}