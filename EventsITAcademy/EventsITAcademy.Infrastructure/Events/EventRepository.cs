using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Domain;
using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;
using EventsITAcademy.Infrastructure.BaseRepository;
using EventsITAcademy.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Infrastructure.Events
{
    public class EventRepository : IEventRepository
    {
        readonly ApplicationContext _applicationContext;

        #region Ctor
        public EventRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        #endregion

        public async Task CreateAsync(CancellationToken cancellationToken, Event @event)
        {
            await _applicationContext.Events.AddAsync(@event,cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(CancellationToken cancellationToken, int id)
        {
            return null;
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, Expression<Func<Event, bool>> predicate)
        {
            return await _applicationContext.Events.AnyAsync(predicate, cancellationToken);
        }



        public async Task<List<Event>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<Event,bool>> predicate)
        {
            return await _applicationContext.Events.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<List<Event>> GetAllUnconfirmedAsync(CancellationToken cancellationToken)
        {
            return await _applicationContext.Events.Where(x => x.Status == EntityStatuses.Active && x.IsActive == false).ToListAsync(cancellationToken);
        }

        public async Task<Event> GetAsync(CancellationToken cancellationToken, int id)
        {
            return await _applicationContext.Events.FirstOrDefaultAsync(x => x.Id == id && 
            x.Status == EntityStatuses.Active, cancellationToken);
        }

        public async Task<Event> GetUserEventAsync(CancellationToken cancellationToken, int id, string userId)
        {
            return await _applicationContext.Events.SingleOrDefaultAsync(x => x.Id == id && x.User.Id == userId, cancellationToken);
        }

        public async Task<List<Event>> GetUserEventsAsync(CancellationToken cancellationToken, string userId)
        {
            return await _applicationContext.Events.Where(x => x.Status == EntityStatuses.Active &&
            x.User.Id == userId).ToListAsync(cancellationToken);
        }

        public async Task<Event> UpdateAsync(CancellationToken cancellationToken, Event @event)
        {
            var retrievedEvent = await GetAsync(cancellationToken, @event.Id);
            _applicationContext.Entry(retrievedEvent).State = EntityState.Detached;
            @event.CreatedAt = retrievedEvent.CreatedAt;
            _applicationContext.Events.Update(@event);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return await GetAsync(cancellationToken,@event.Id);
        }
    }
}
