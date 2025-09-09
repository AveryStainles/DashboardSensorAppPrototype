using Microsoft.AspNetCore.Hosting.Server;
using MQTTnet;
using MQTTnet.Server;

namespace DashboardSensor
{
    public interface IMqttServer
    {
        Task RunMqttServer();
        Task StopMqttServer();
        Task GetNewestDataFromTopic(string topic);
        bool IsServerNull();
        List<string> GetResponses();
    }
}
