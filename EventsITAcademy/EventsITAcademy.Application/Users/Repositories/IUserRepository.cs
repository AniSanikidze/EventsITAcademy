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

        Task<List<User>> GetAllAsync(CancellationToken cancellationToken);

        Task<bool> Exists(CancellationToken cancellationToken, string userId);
    }
}
