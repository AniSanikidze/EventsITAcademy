using EventsITAcademy.Application.CustomHasher;
using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Users;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Domain.Users;
using EventsITAcademy.Infrastructure.Events;
using EventsITAcademy.Infrastructure.Users;
using Microsoft.AspNetCore.Identity;

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
            //services.AddScoped<ISubtaskRepository, SubtaskRepository>();

            //services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        }
    }
}
