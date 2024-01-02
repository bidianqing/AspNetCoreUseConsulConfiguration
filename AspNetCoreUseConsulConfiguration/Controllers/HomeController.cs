using AspNetCoreUseConsulConfiguration.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspNetCoreUseConsulConfiguration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppOptions _appOptions;
        private readonly EmailOptions _emailOptions;
        public HomeController(IConfiguration configuration, 
            IOptionsSnapshot<AppOptions> appOptionsAccessor, 
            IOptionsSnapshot<EmailOptions> emailOptionsAccessor)
        {
            _configuration = configuration;
            _appOptions = appOptionsAccessor.Value;
            _emailOptions = emailOptionsAccessor.Value;
        }

        public IActionResult Get()
        {
            var name = _configuration["Name"];
            var serviceName = _appOptions.ServiceName;
            var from = _emailOptions.From;

            return Ok($"{name} == {serviceName} == {from}");
        }
    }
}
