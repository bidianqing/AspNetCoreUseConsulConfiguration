using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using Winton.Extensions.Configuration.Consul;

namespace AspNetCoreUseConsulConfiguration
{
    public class Program
    {
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
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
                                _cancellationTokenSource.Token,
                                options =>
                                {
                                    options.Optional = true;
                                    options.ReloadOnChange = true;
                                    options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                                    options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consul_url); };
                                })
                           .AddConsul(
                                $"{env.ApplicationName}/test.json",
                                _cancellationTokenSource.Token,
                                options =>
                                {
                                    options.Optional = true;
                                    options.ReloadOnChange = true;
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
