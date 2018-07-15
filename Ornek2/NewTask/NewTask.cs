﻿using System;
using RabbitMQ.Client;
using System.Text;

class NewTask
{
  public static void Main(string[] args)
  {
    var factory = new ConnectionFactory() { HostName = "localhost" };
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
      channel.QueueDeclare(queue: "Ornek2",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null);

      var message = GetMessage(args);
      var body = Encoding.UTF8.GetBytes(message);

      var properties = channel.CreateBasicProperties();
      properties.Persistent = true;

      channel.BasicPublish(exchange: "",
        routingKey: "Ornek2",
        basicProperties: properties,
        body: body);
      Console.WriteLine(" [x] Gönderilen : {0}", message);
    }

    Console.WriteLine(" Çıkış için [enter]'a bas.");
    Console.ReadLine();
  }

  private static string GetMessage(string[] args)
  {
    return ((args.Length > 0) ? string.Join(" ", args) : "Merhaba Dünya!");
  }
}