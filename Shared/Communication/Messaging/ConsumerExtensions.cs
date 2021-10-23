using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace Contracts.Communication.Messaging;

public static class ConsumerExtensions
{
    public static T? GetBody<T>(this BasicDeliverEventArgs args)
    {
        var body = Encoding.UTF8.GetString(args.Body.ToArray());
        return JsonConvert.DeserializeObject<T>(body);
    }
}