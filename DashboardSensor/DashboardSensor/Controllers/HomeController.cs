using DashboardSensor.Models;
using Microsoft.AspNetCore.Mvc;


namespace DashboardSensor.Controllers
{
    public class HomeController(
        IMqttServer _mqttServer) : Controller
    {
        private static bool IsServerRunning = false;
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel();
            viewModel.IsServerRunning = IsServerRunning;

            if (IsServerRunning)
            {
                var test = _mqttServer.GetResponses();
                viewModel.SubscriptionResponse = _mqttServer.GetResponses();
            }

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult RunServer()
        {
            _mqttServer.RunMqttServer();
            IsServerRunning = true;
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult StopServer()
        {
            _mqttServer.StopMqttServer();
            IsServerRunning = false;
            return RedirectToAction(nameof(Index));
        }
    }
}
