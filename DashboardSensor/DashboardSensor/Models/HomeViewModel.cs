namespace DashboardSensor.Models
{
    public class HomeViewModel
    {
        public List<string> SubscriptionResponse { get; set; } = new();
        public bool IsServerRunning { get; set; }
    }
}
