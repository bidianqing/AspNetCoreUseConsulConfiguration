using AspNetCoreUseConsulConfiguration.Options;
using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);

void ConsulConfigurationAction(IConsulConfigurationSource options)
{
    options.Optional = true;
    options.ReloadOnChange = true;
    options.PollWaitTime = TimeSpan.FromSeconds(3);
    options.OnLoadException = consulLoadExceptionContext => 
    {
        Console.WriteLine(consulLoadExceptionContext.Exception.ToString());
        consulLoadExceptionContext.Ignore = false;
    };
    options.OnWatchException = (consulWatchExceptionContext) => 
    {
        Console.WriteLine(consulWatchExceptionContext.Exception.ToString());
        return TimeSpan.FromSeconds(3);
    };
    options.ConsulConfigurationOptions = cco => 
    { 
        cco.Address = new Uri(builder.Configuration["Consul:Address"]);
        cco.Token = builder.Configuration["Consul:Token"];
    };
}
builder.Configuration
    .AddConsul($"{builder.Environment.ApplicationName}/appsettings.{builder.Environment.EnvironmentName}.json", ConsulConfigurationAction)
    .AddConsul($"{builder.Environment.ApplicationName}/appsettings.test.json", ConsulConfigurationAction);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(nameof(AppOptions)));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(nameof(EmailOptions)));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();