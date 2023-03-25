using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Users.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(CancellationToken cancellationToken, User user);

        Task<User> GetByEmailAsync(CancellationToken cancellationToken, string email);
        Task<User> GetByUsernameAsync(CancellationToken cancellationToken, string username);

        Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<User> GetAsync(CancellationToken cancellationToken, string id);

        Task<bool> Exists(CancellationToken cancellationToken, string userId);

        Task<List<Event>> GetUserEventsAsync(CancellationToken cancellationToken, string userId);
        Task DeleteAsync(CancellationToken cancellationToken, string userId);
    }
}
