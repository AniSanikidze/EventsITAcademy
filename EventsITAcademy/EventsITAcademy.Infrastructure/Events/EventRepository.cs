using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Domain;
using EventsITAcademy.Domain.Events;
using EventsITAcademy.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventsITAcademy.Infrastructure.Events
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationContext _applicationContext;

        #region Ctor
        public EventRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        #endregion

        public async Task<int> CreateAsync(CancellationToken cancellationToken, Event @event)
        {
            await _applicationContext.Events.AddAsync(@event, cancellationToken).ConfigureAwait(false);
            await _applicationContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return @event.Id;
        }

        public async Task<int> DeleteAsync(CancellationToken cancellationToken, int id)
        {
            var entity = await GetAsync(cancellationToken, id).ConfigureAwait(false);
            entity.Status = EntityStatuses.Deleted;
            if (entity.Tickets != null && entity.Tickets.Count > 0)
            {
                entity.Tickets?.ForEach(x => x.Status = EntityStatuses.Deleted);
            }

            if(entity.Image != null)
            {
                entity.Image.Status = EntityStatuses.Deleted;
            }

            _applicationContext.Entry(entity).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return id;
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, Expression<Func<Event, bool>> predicate)
        {
            return await _applicationContext.Events.AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Event>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<Event, bool>> predicate)
        {
            return await _applicationContext.Events.Include(x => x.Image).Where(predicate).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Event>> GetAllUnconfirmedAsync(CancellationToken cancellationToken)
        {
            return await _applicationContext.Events.Where(x => x.Status == EntityStatuses.Active && x.IsActive == false).ToListAsync(cancellationToken);
        }

        public async Task<Event> GetAsync(CancellationToken cancellationToken, int id)
        {
            return await _applicationContext.Events.Include(x => x.Image).Include(x => x.Tickets).FirstOrDefaultAsync(x => x.Id == id &&
            x.Status == EntityStatuses.Active, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Event> GetUserEventAsync(CancellationToken cancellationToken, int id, string userId)
        {
            return await _applicationContext.Events.SingleOrDefaultAsync(x => x.Id == id && x.User.Id == userId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Event>> GetUserEventsAsync(CancellationToken cancellationToken, string userId)
        {
            return await _applicationContext.Events.Where(x => x.Status == EntityStatuses.Active &&
            x.User.Id == userId).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync(CancellationToken cancellationToken, Event @event)
        {
            var retrievedEvent = await GetAsync(cancellationToken, @event.Id).ConfigureAwait(false);
            _applicationContext.Entry(retrievedEvent).State = EntityState.Detached;

            @event.ModificationPeriod = retrievedEvent.ModificationPeriod;
            @event.ReservationPeriod = retrievedEvent.ReservationPeriod;
            @event.CreatedAt = retrievedEvent.CreatedAt;
            _applicationContext.Events.Update(@event);
            await _applicationContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return @event.Id;
        }

        public async Task<Event> UpdateEventModResPeriodsAsync(CancellationToken cancellationToken, Event @event)
        {
            var retrievedEvent = await GetAsync(cancellationToken, @event.Id).ConfigureAwait(false);
            _applicationContext.Entry(retrievedEvent).State = EntityState.Detached;

            @event.CreatedAt = retrievedEvent.CreatedAt;
            @event.OwnerId = retrievedEvent.OwnerId;
            @event.IsActive = retrievedEvent.IsActive;
            _applicationContext.Events.Update(@event);
            await _applicationContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return await GetAsync(cancellationToken, @event.Id).ConfigureAwait(false);
        }
    }
}
