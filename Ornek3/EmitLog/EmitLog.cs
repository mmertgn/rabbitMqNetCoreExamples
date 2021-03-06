﻿using System;
using System.Text;
using RabbitMQ.Client;

namespace EmitLog
{
  class Program
  {
    public static void Main(string[] args)
    {
      var factory = new ConnectionFactory() { HostName = "localhost" };
      using (var connection = factory.CreateConnection())
      using (var channel = connection.CreateModel())
      {
        channel.ExchangeDeclare(exchange: "logs", type: "fanout");

        var message = GetMessage(args);
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "logs",
          routingKey: "",
          basicProperties: null,
          body: body);
        Console.WriteLine(" [x] Gönderilen Mesaj : {0}", message);
      }

      Console.WriteLine(" Çıkış için [enter]'a basınız.");
      Console.ReadLine();
    }

    private static string GetMessage(string[] args)
    {
      return ((args.Length > 0)
        ? string.Join(" ", args)
        : "bilgi: Merhaba Dünya!");
    }
  }
}
