using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Domain;
using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;
using EventsITAcademy.Infrastructure.BaseRepository;
using EventsITAcademy.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Infrastructure.Users
{
    public class UserRepository : IUserRepository
    {
        readonly ApplicationContext _applicationContext;
        #region Ctor
        public UserRepository(ApplicationContext applicationContext)
        {
            //_repository = repository;
            _applicationContext = applicationContext;
        }
        #endregion
        public async Task DeleteAsync(CancellationToken cancellationToken, string userId)
        {
            var user = await GetAsync(cancellationToken, userId);
            user.ModifiedAt = DateTime.Now;
            if (user.Tickets != null && user.Tickets.Count > 0)
            {
                user.Tickets?.ForEach(x => x.Status = EntityStatuses.Deleted);
            }
            if (user.Events != null && user.Events.Count > 0)
            {
                user.Events?.ForEach(x => x.Status = EntityStatuses.Deleted);
            }
            user.Status = EntityStatuses.Deleted;
            _applicationContext.Entry(user).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, string userId)
        {
            return await _applicationContext.Users.AnyAsync(x => x.Id == userId &&
                    x.Status != EntityStatuses.Deleted, cancellationToken);
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _applicationContext.Users.Where(x => x.Status == EntityStatuses.Active).ToListAsync(cancellationToken);
        }

        public async Task<User> GetAsync(CancellationToken cancellationToken, string id)
        {
            return await _applicationContext.Users.Include(x => x.Tickets).Include(x => x.Events).SingleOrDefaultAsync(x => x.Id == id && x.Status == EntityStatuses.Active, cancellationToken);
        }

        public async Task<User> GetByEmailAsync(CancellationToken cancellationToken, string email)
        {
            return await _applicationContext.Users.SingleOrDefaultAsync(x => x.Email == email &&
                x.Status != EntityStatuses.Deleted, cancellationToken);
        }

        public async Task<List<Event>> GetUserEventsAsync(CancellationToken cancellationToken, string userId)
        {
            var user = await _applicationContext.Users.Include(x => x.Events.Where(x => x.Status == EntityStatuses.Active)).ThenInclude(x => x.Image)           
                                            .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
            return user.Events;
        }
    }
}
