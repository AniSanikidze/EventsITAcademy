using EventsITAcademy.API.Infrastructure;
using EventsITAcademy.API.Infrastructure.Auth;
using EventsITAcademy.API.Infrastructure.Extensions;
using EventsITAcademy.API.Infrastructure.Mappings;
using EventsITAcademy.Domain.Users;
using EventsITAcademy.Persistence.Context;
using EventsITAcademy.Persistence.Seed;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PersonManagement.Web.Infrastructure.VersionSwagger;
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

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVersionedApiExplorer(option =>
{
    option.GroupNameFormat = "'v'VVV";
    option.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
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
                            Array.Empty<string>()
                    }
                });
    option.OperationFilter<SwaggerDefaultValues>();

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
builder.Services.AddDefaultIdentity<User>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>();

//HealthChecks
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("ApplicationContextConnection"));

//AddHttpContextAccessor
builder.Services.AddHttpContextAccessor();

//Add Fluent Validator
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//Mapping 
builder.Services.RegisterMaps();

var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        foreach (var desciptions in provider.ApiVersionDescriptions)
        {
            option.SwaggerEndpoint($"/swagger/{desciptions.GroupName}/swagger.json"
                , $"{desciptions.GroupName.ToUpper()}");
        }
    });
}
//GlobalExceptionHandling middleware
app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();
//Request Response Logging Middleware
app.UseRequestLogging();
//Culture Middleware
app.UseRequestCulture();

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/quickhealth", new HealthCheckOptions()
{
    Predicate = _ => false
});

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
