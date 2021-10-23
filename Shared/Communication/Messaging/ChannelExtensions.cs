using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Contracts.Communication.Messaging;

public static class ChannelExtensions
{
    public static void PublishJson<T>(this IModel channel, string exchange, string routingKey, T body, IBasicProperties properties = null)
    {
        var serialized = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));
        channel.BasicPublish(exchange, routingKey, properties, serialized);
    }
}