using System;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
  class Program
  {
    static void Main(string[] args)
    {
      var factory = new ConnectionFactory() { HostName = "localhost" };
      using (var connection = factory.CreateConnection())
      using (var channel = connection.CreateModel())
      {
        channel.QueueDeclare(queue: "Ornek1", 
          durable: false, 
          exclusive: false, 
          autoDelete: false, 
          arguments: null);

        string message = "Merhaba Dünya!";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "", 
          routingKey: "Ornek1", 
          basicProperties: null, 
          body: body);
        Console.WriteLine(" [x] Gönderilen mesaj : {0}", message);
      }

      Console.WriteLine(" Çıkış için [enter]'a bas.");
      Console.ReadLine();
    }
  }
}
