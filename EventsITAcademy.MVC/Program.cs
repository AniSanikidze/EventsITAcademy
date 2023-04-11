using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventsITAcademy.Persistence.Context;
using EventsITAcademy.Domain.Users;
using EventsITAcademy.Persistence.Seed;
using EventsITAcademy.MVC.Infrastructure.Mappings;
using EventsITAcademy.MVC.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
//Dependency Injection
builder.Services.AddServices();

builder.Services.AddSession(conf => conf.IdleTimeout = TimeSpan.FromMinutes(15));
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("ConnectionString") ?? throw new InvalidOperationException("'ConnectionString' not found.");

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Mapping 
builder.Services.RegisterMaps();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute("Ticket",
    "ticket/{userId}/{eventId}",
    new { controller = "search", action = "reserve", userId = "", eventId = "" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
//Seeding
EventsITAcademySeed.Initialize(app.Services);

app.Run();
