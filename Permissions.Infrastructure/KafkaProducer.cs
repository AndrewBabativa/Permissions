using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Nest;

namespace Permissions.Infrastructure
{
  public class KafkaProducer
  {
    private readonly IProducer<Null, string> _producer;
    private readonly IConfiguration _configuration;

    public KafkaProducer(IConfiguration configuration)
    {
      _configuration = configuration;

      var producerconfig = new ProducerConfig
      {
        BootstrapServers = _configuration["Kafka:BootstrapServers"]
      };

      _producer = new ProducerBuilder<Null, string>(producerconfig).Build();
    }

    public async Task ProduceMessageAsync(string topic, string message)
    {
      var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
      Console.WriteLine($"Mensaje guardado en Kafka. Offset:'{result.Offset}' Topic:'{result.Topic}' Status:'{result.Status}' Message:'{result.Message.Value}'");
    }

    public void Dispose()
    {
      _producer.Dispose();
    }
  }

}
