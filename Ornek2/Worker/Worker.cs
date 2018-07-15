using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;

class Worker
{
  public static void Main()
  {
    var factory = new ConnectionFactory() { HostName = "localhost" };
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
      channel.QueueDeclare(
        queue: "Ornek2",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null);

      channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

      Console.WriteLine(" [*] Mesajlar Bekleniyor.");

      var consumer = new EventingBasicConsumer(channel);
      consumer.Received += (model, ea) =>
      {
        var body = ea.Body;
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Alınan Mesaj : {0}", message);

        int dots = message.Split('.').Length - 1;
        Thread.Sleep(dots * 3000);

        Console.WriteLine(" [x] Bitti");

        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
      };
      channel.BasicConsume(queue: "Ornek2",
        autoAck: false,
        consumer: consumer);

      Console.WriteLine(" Çıkış için [enter]'a bas.");
      Console.ReadLine();
    }
  }
}