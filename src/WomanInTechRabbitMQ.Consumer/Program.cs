using RabbitMQ.Client;
using System.Text;
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

var json = JsonSerializer.Serialize(new { Evento = "Woman in Tech - Banco Carrefour" });
var bytes = Encoding.UTF8.GetBytes(json);

model.BasicPublish("woman-in-tech-exchange", "", model.CreateBasicProperties(), bytes.AsMemory());

Console.WriteLine("Mensagem enviada");