using EventsITAcademy.API.Infrastructure;
using EventsITAcademy.API.Infrastructure.Auth;
using EventsITAcademy.API.Infrastructure.Extensions;
using EventsITAcademy.API.Infrastructure.Mappings;
using EventsITAcademy.Domain.Users;
using EventsITAcademy.Persistence.Context;
using EventsITAcademy.Persistence.Seed;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
//Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        //Type = SecuritySchemeType.Http,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ToDo App Api",
        Version = "v1",
        Description = "Api to manage todos and subtasks",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine($"{AppContext.BaseDirectory}", xmlFile);

    option.IncludeXmlComments(xmlPath);
    option.ExampleFilters();
});
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

//Add JWT Token
builder.Services.AddTokenAuthentication(builder.Configuration.GetSection(nameof(JWTConfiguration)).GetSection(nameof(JWTConfiguration.Secret)).Value);
builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection(nameof(JWTConfiguration)));

//Dependency Injection
builder.Services.AddServices();
var connectionString = builder.Configuration.GetConnectionString("ApplicationContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationContextConnection' not found.");

//Add DbContext
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

//Add Identity
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>();

//AddHttpContextAccessor
builder.Services.AddHttpContextAccessor();

//Add Fluent Validator
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//Mapping 
builder.Services.RegisterMaps();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//GlobalExceptionHandling middleware
app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();
//Request Response Logging Middleware
app.UseRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
//Seeding
EventsITAcademySeed.Initialize(app.Services);

try
{
    Log.Information("Starting web host...");
    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex,"Host terminated");
}
finally
{
    Log.CloseAndFlush();    
}
