using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
  class Program
  {
    static void Main(string[] args)
    {
      var factory = new ConnectionFactory() {HostName = "localhost"};
      using (var connection = factory.CreateConnection())
      using (var channel = connection.CreateModel())
      {
        channel.QueueDeclare(
          queue: "Ornek1",
          durable: false,
          exclusive: false,
          autoDelete: false,
          arguments: null);

        Console.WriteLine(" [*] Mesajlar bekleniyor.");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
          var body = ea.Body;
          var message = Encoding.UTF8.GetString(body);
          Console.WriteLine(" [x] Alınan Mesaj : {0}", message);
        };
        channel.BasicConsume(queue: "Ornek1",
          autoAck: true,
          consumer: consumer);

        Console.WriteLine(" Çıkış için [enter]'a bas.");
        Console.ReadLine();
      }
    }
  }
}