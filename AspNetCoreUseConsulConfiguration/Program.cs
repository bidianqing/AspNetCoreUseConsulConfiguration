﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading;
using Winton.Extensions.Configuration.Consul;

namespace AspNetCoreUseConsulConfiguration
{
    public class Program
    {
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    if (env.IsDevelopment())
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            config.AddUserSecrets(appAssembly, optional: true);
                        }
                    }

                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }

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
                                });

                    hostingContext.Configuration = config.Build();
                })
                .UseStartup<Startup>();
    }
}