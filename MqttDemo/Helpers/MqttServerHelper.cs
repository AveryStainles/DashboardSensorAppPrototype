using MQTTnet.Server;

namespace MqttDemo.Helpers
{
    public class MqttServerHelper
    {
        private static MqttServer? _serverConnection { get; set; }
        private static readonly MqttServerFactory MqttServerFactory = new();

        private static MqttServer Server
        {
            get
            {
                if (_serverConnection == null)
                {
                    var mqttServerOptions = MqttServerFactory.CreateServerOptionsBuilder().WithDefaultEndpoint().Build();
                    _serverConnection = MqttServerFactory.CreateMqttServer(mqttServerOptions);
                }

                return _serverConnection;
            }
        }

        public static async Task StartMqttServer() => await Server.StartAsync();

        public static async Task StopMqttServer() => await Server.StopAsync();
    }
}