using MqttDemo.Models;
using MQTTnet;
using System.Net;

namespace MqttDemo.Helpers
{
    public class MqttClientHelper
    {
        private static IMqttClient? _clientConnection { get; set; }

        private static readonly MqttClientFactory MqttClientFactory = new MqttClientFactory();

        private static readonly MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
            //.WithCredentials("username", "password")
            .WithEndPoint(new DnsEndPoint("localhost", 1883))
            .Build();

        public static IMqttClient Client
        {
            get
            {
                if (_clientConnection == null)
                {
                    _clientConnection = MqttClientFactory.CreateMqttClient();
                    Client.ApplicationMessageReceivedAsync += InvokeEventListenerHandler;
                }

                if (!_clientConnection.IsConnected)
                    _clientConnection.ConnectAsync(mqttClientOptions, CancellationToken.None).Wait();

                return _clientConnection;
            }
        }

        private static async Task InvokeEventListenerHandler(MqttApplicationMessageReceivedEventArgs args)
        {
            // Check to see if the new data received on the MQTT server belongs to one of our subscribed topics.
            var subscribedTopics = DataTransmissionHelper.SubscribedTopics
                .FirstOrDefault(x => x.Topic.Equals(args.ApplicationMessage.Topic));

            if (subscribedTopics == null)
                return;

            subscribedTopics.LastMessage = args.ApplicationMessage.ConvertPayloadToString();

            foreach (var eventListener in subscribedTopics.EventListeners)
                await eventListener.Invoke(args); // Execute handler during runtime
        }
    }
}
