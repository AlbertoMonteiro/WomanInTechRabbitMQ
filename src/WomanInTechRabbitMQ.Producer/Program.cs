using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

var connectionFactory = new ConnectionFactory()
{
    Uri = new Uri("ampq://guest:guest@localhost")
};

using var connection = connectionFactory.CreateConnection();
using var model = connection.CreateModel();

model.ExchangeDeclare("woman-in-tech-exchange", ExchangeType.Direct);
model.QueueDeclare("woman-in-tech-queue", true, false, false);
model.QueueBind("woman-in-tech-queue", "woman-in-tech-exchange", "");

var consumer = new EventingBasicConsumer(model);
consumer.Received += (sender, eventArgs) =>
{
    var body = eventArgs.Body;
    var data = JsonSerializer.Deserialize<Message>(body.Span);

    Console.WriteLine($"Recebido mensagem do evento: {data.Evento}");
};

model.BasicConsume("woman-in-tech-queue", false, consumer);

Console.WriteLine("Mensagem enviada");

public record Message(string Evento);