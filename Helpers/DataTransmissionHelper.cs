using MqttDemo.Models;
using MQTTnet;

namespace MqttDemo.Helpers
{
    public class DataTransmissionHelper
    {
        public static List<SubscribedTopic> SubscribedTopics { get; set; } = new();

        public static string GetLastMessageFromTopic(string topic) => SubscribedTopics.FirstOrDefault(x => x.Topic == topic)?.LastMessage ?? string.Empty;

        public static async Task SubscribeData(Func<MqttApplicationMessageReceivedEventArgs, Task> func, string topic)
        {
            var subscribedTopic = SubscribedTopics.FirstOrDefault(x => x.Topic == topic);

            // If it is a new topic, subscribe to it.
            if (subscribedTopic == null)
            {
                var newTopic = new SubscribedTopic() { Topic = topic };
                await MqttClientHelper.Client.SubscribeAsync(topic);
                SubscribedTopics.Add(newTopic);
                subscribedTopic = newTopic;
            }

            // Code to execute when the event listener triggers
            subscribedTopic.EventListeners.Add(func);
        }

        public static async Task PublishData(string topic, string data)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic.ToString())
                .WithPayload(data)
                .Build();
            await MqttClientHelper.Client.PublishAsync(applicationMessage);
        }
    }
}
