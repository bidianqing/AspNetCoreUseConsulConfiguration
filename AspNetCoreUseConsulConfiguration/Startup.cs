using AspNetCoreUseConsulConfiguration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Threading;

namespace AspNetCoreUseConsulConfiguration
{
    public class Startup
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 如果构造函数使用IOptionsSnapshot<>或IOptionsMonitor<> 则不需要下面的注入
            // 如果构造函数使用IOptions<> 则必须添加下面的注入
            //services.AddTransient(typeof(IOptions<>), typeof(OptionsManager<>));

            services.Configure<AppOptions>(Configuration.GetSection(nameof(AppOptions)));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }


    public static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration config)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.TryAddSingleton<IConfiguration>(config);

            var root = config as IConfigurationRoot;
            if (root != null)
            {
                services.TryAddSingleton<IConfigurationRoot>(root);
            }

            return services;
        }
    }
}
