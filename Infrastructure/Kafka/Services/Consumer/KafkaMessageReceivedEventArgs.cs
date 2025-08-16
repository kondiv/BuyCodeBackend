namespace Infrastructure.Kafka.Services.Consumer;

public class KafkaMessageReceivedEventArgs : EventArgs
{
    public string Topic { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public int Partition { get; set; }
    public long Offset { get; set; }
    public DateTime Timestamp { get; set; }
}