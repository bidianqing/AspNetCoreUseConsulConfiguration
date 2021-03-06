﻿using AspNetCoreUseConsulConfiguration.Models;
using AspNetCoreUseConsulConfiguration.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace AspNetCoreUseConsulConfiguration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppOptions _appOptions;
        private readonly EmailOptions _emailOptions;
        public HomeController(IConfiguration configuration,IOptionsSnapshot<AppOptions> appOptionsAccessor, IOptionsSnapshot<EmailOptions> emailOptionsAccessor)
        {
            _configuration = configuration;
            _appOptions = appOptionsAccessor.Value;
            _emailOptions = emailOptionsAccessor.Value;
        }

        public IActionResult Index()
        {
            ViewBag.LogLevel = _configuration["Logging:LogLevel:Default"];
            ViewBag.ServiceName = _appOptions.ServiceName;
            ViewBag.From = _emailOptions.From;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
