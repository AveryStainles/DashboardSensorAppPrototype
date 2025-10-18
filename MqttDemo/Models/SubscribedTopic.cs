using MQTTnet;

namespace MqttDemo.Models
{
    public class SubscribedTopic
    {
        // Constructors
        public SubscribedTopic() { Topic = string.Empty; }
        public SubscribedTopic(string topic) { Topic = topic; }

        // Properties
        public string Topic { get; set; }
        public string LastMessage { get; set; } = string.Empty;
        public List<Func<MqttApplicationMessageReceivedEventArgs, Task>> EventListeners { get; set; } = new();
    }
}
