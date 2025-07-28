using Infrastructure.Kafka.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka;

public static class Kafka
{
    public static IServiceCollection AddKafka(this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        services.Configure<KafkaSettings>(configurationSection);

        services.AddSingleton<IKafkaProducer, KafkaProducer>();
        
        return services;
    }
}