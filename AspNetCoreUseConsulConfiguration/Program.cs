using Microsoft.AspNetCore.Hosting;
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

                    hostingContext.Configuration = config.Build();
                    string consul_url = hostingContext.Configuration["Consul_Url"];
                    config.AddConsul(
                                $"{env.ApplicationName}/appsettings.{env.EnvironmentName}.json",
                                options =>
                                {
                                    options.Optional = true;
                                    options.ReloadOnChange = true;
                                    options.PollWaitTime = TimeSpan.FromSeconds(3);
                                    options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                                    options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consul_url); };
                                })
                           .AddConsul(
                                $"{env.ApplicationName}/test.json",
                                options =>
                                {
                                    options.Optional = true;
                                    options.ReloadOnChange = true;
                                    options.PollWaitTime = TimeSpan.FromSeconds(3);
                                    options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                                    options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consul_url); };
                                });

                    hostingContext.Configuration = config.Build();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
