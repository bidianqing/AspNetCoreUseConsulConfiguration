using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Winton.Extensions.Configuration.Consul;

namespace AspNetCoreUseConsulConfiguration
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    IConfiguration Configuration = config.Build();
                    string consul_url = Configuration["Consul_Url"];
                    config.AddConsul($"{env.ApplicationName}/appsettings.{env.EnvironmentName}.json", ConfigureAction)
                          .AddConsul($"{env.ApplicationName}/test.json", ConfigureAction);
                    
                    void ConfigureAction(IConsulConfigurationSource options)
                    {
                        options.Optional = true;
                        options.ReloadOnChange = true;
                        options.PollWaitTime = TimeSpan.FromSeconds(3);
                        options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                        options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consul_url); };
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
