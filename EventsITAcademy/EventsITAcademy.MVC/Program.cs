using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventsITAcademy.Persistence.Context;
using EventsITAcademy.Domain.Users;
using Microsoft.CodeAnalysis;
using Microsoft.VisualBasic;
using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Infrastructure.Events;
using EventsITAcademy.Application.Users;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Infrastructure.Users;
using EventsITAcademy.Application.CustomHasher;
using EventsITAcademy.Persistence.Seed;
using EventsITAcademy.Application.Tickets;
using EventsITAcademy.Application.Tickets.Repositories;
using EventsITAcademy.Infrastructure.Tickets;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();


builder.Services.AddSession(conf => conf.IdleTimeout = TimeSpan.FromMinutes(15));
builder.Services.AddScoped<IPasswordHasher<User>, CustomPasswordHasher>();
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("ConnectionString") ?? throw new InvalidOperationException("'ConnectionString' not found.");

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(opt =>
{
    opt.Password.RequiredLength = 7;
    //opt.Password.RequireDigit = false;
    //opt.Password.RequireUppercase = false;
    //A better solution would be to send an email message to the owner of this account, with the information that the account already exists.
    opt.User.RequireUniqueEmail = true;
}
//SignIn.RequireConfirmedAccount = true
)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
