using AspNetCoreUseConsulConfiguration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace AspNetCoreUseConsulConfiguration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ConfigData _configData;
        public HomeController(IConfiguration configuration,IOptionsMonitor<ConfigData> options)
        {
            _configuration = configuration;
            _configData = options.CurrentValue;
        }

        public IActionResult Index()
        {
            ViewBag.ConsulUrl = _configuration["Consul_Url"];
            ViewBag.ServiceName = _configData.ServiceName;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
