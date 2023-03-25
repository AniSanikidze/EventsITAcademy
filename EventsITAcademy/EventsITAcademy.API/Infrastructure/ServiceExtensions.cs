using EventsITAcademy.Application.Admin;
using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Images;
using EventsITAcademy.Application.Images.Repositories;
using EventsITAcademy.Application.Tickets;
using EventsITAcademy.Application.Tickets.Repositories;
using EventsITAcademy.Application.Users;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Domain.Users;
using EventsITAcademy.Infrastructure.Events;
using EventsITAcademy.Infrastructure.Images;
using EventsITAcademy.Infrastructure.Tickets;
using EventsITAcademy.Infrastructure.Users;
using Microsoft.AspNetCore.Identity;
using Utilities.CustomHasher;

namespace EventsITAcademy.API.Infrastructure
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IPasswordHasher<User>, CustomPasswordHasher>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IAdminService, AdminService>();



            //services.AddScoped<ISubtaskRepository, SubtaskRepository>();

            //services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        }
    }
}
