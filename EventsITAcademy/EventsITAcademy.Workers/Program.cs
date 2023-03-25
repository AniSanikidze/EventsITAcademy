// See https://aka.ms/new-console-template for more information
using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Images;
using EventsITAcademy.Application.Images.Repositories;
using EventsITAcademy.Application.Tickets;
using EventsITAcademy.Application.Tickets.Repositories;
using EventsITAcademy.Application.Users;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Infrastructure.Events;
using EventsITAcademy.Infrastructure.Images;
using EventsITAcademy.Infrastructure.Tickets;
using EventsITAcademy.Infrastructure.Users;
using EventsITAcademy.Persistence.Context;
using EventsITAcademy.Workers;
using EventsITAcademy.Workers.BackgroundWorkers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;



var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    
    var builder = CreateHostBuilder(args);
    Log.Information("Background worker starting...");
    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed");
}
finally
{
    await Log.CloseAndFlushAsync();
}

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString") ?? throw new InvalidOperationException("'ConnectionString' not found.")),
            ServiceLifetime.Transient
            );
        services.AddSingleton<ServiceClient>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddHostedService<TicketWorker>();
        services.AddHostedService<EventWorker>();

    })
    .UseSerilog();