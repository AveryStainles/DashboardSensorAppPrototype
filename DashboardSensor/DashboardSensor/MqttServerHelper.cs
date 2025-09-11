using Microsoft.AspNetCore.Hosting.Server;
using MQTTnet;
using MQTTnet.Server;
using System.Collections;
using System.IO;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DashboardSensor
{
    public class MqttServerHelper() : IMqttServer
    {
        private static MqttServer Server { get; set; }
        private static IMqttClient Client { get; set; }
        private static List<string> SubscriptionResponse { get; set; } = new List<string>();
        public List<string> GetResponses() => SubscriptionResponse;
        public bool IsServerNull() => Server == null ? true : Server.IsStarted;

        public async Task RunMqttServer()
        {
            // Run
            new Thread(async () =>
            {
                var mqttServerFactory = new MqttServerFactory();
                var mqttServerOptions = mqttServerFactory.CreateServerOptionsBuilder().WithDefaultEndpoint().Build();
                var server = mqttServerFactory.CreateMqttServer(mqttServerOptions);
                await server.StartAsync();
                Server = server;
            }).Start();

            Client = await ConnectClient();

            var topic = "test";

            await GetNewestDataFromTopic(topic);
        }

        // Simulate another client communicating on the MQTT server
        public async Task RunSecondClient(string topic)
        {
            var Client2 = await ConnectClient();

            if (SubscriptionResponse.Count == 0)
            {

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload("19.5")
                    .Build();
                await Client2.PublishAsync(applicationMessage);
            }
            else if (SubscriptionResponse.Count == 1)
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload("John Doe is the one you're after!")
                    .Build();
                await Client2.PublishAsync(applicationMessage);
            }
            else
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload("Robot Temp 50C")
                    .Build();
                await Client2.PublishAsync(applicationMessage);
            }
        }
        public async Task StopMqttServer()
        {
            await Server.StopAsync();
        }

        private static async Task<IMqttClient> ConnectClient()
        {
            var mqttFactory = new MqttClientFactory();
            var mqttClient = mqttFactory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithEndPoint(new DnsEndPoint("localhost", 1883))
                .Build();

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            return mqttClient;
        }

        public async Task GetNewestDataFromTopic(string topic)
        {
            await Client.SubscribeAsync(topic);
            Client.ApplicationMessageReceivedAsync += async (args) =>
            {
                string createText = $"Client message | Topic: {args.ApplicationMessage.Topic} | Value: {args.ApplicationMessage.ConvertPayloadToString()}";
                SubscriptionResponse.Add(createText);
            };
        }
        public static async void PublishData(string topic, string data)
        {
            await ConnectClient();

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(data)
                .Build();
            await Client.PublishAsync(applicationMessage);
            Console.WriteLine("AVERY TEST: CLIENT PUBLISHED");
        }
    }
}
