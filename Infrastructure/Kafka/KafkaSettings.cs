using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Kafka;

public sealed class KafkaSettings
{
    [Required]
    public string BootstrapServers { get; set; } = string.Empty;

    [Required]
    public string ClientId { get; set; } = string.Empty;
    
    [Required]
    public string GroupId { get; set; } = string.Empty;
}