using EventsITAcademy.Application.CustomExceptions;
using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Images;
using EventsITAcademy.Domain;
using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;
using Mapster;
using Utilities.Localizations;

namespace EventsITAcademy.Application.Events
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IImageService _imageService;

        public EventService(IEventRepository eventRepository, IImageService imageService)
        {
            _eventRepository = eventRepository;
            _imageService = imageService;
        }

        public async Task<int> CreateAsync(CancellationToken cancellationToken, EventRequestModel eventRequest, string userId)
        {
            var adaptedEvent = eventRequest.Adapt<Event>();
            adaptedEvent.OwnerId = userId;
            adaptedEvent.ModificationPeriod = 1;
            adaptedEvent.ReservationPeriod = 10;
            var eventId = await _eventRepository.CreateAsync(cancellationToken, adaptedEvent).ConfigureAwait(false);
            if (eventRequest.ImageFile != null)
            {
                await _imageService.SaveImageAsync(cancellationToken, eventRequest.ImageFile, eventId).ConfigureAwait(false);
            }
            return eventId;
        }

        public async Task<int> DeleteAsync(CancellationToken cancellationToken, int id)
        {
            if (!await _eventRepository.Exists(cancellationToken, x => x.Status != EntityStatuses.Deleted && x.Id == id).ConfigureAwait(false))
            {
                throw new ItemNotFoundException(ClassNames.Event + " " + ErrorMessages.NotFound, nameof(User));
            }
            await _eventRepository.DeleteAsync(cancellationToken, id).ConfigureAwait(false);
            return id;
        }

        public async Task<List<EventResponseModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync(cancellationToken, x => x.Status == EntityStatuses.Active && x.IsActive == true).ConfigureAwait(false);
            return events.Adapt<List<EventResponseModel>>();
        }

        public async Task<List<EventResponseModel>> GetAllConfirmedAsync(CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync(cancellationToken, x => x.Status == EntityStatuses.Active && x.IsActive == true && x.IsArchived == false).ConfigureAwait(false);
            var adaptedEvents = events.Adapt<List<EventResponseModel>>();

            return adaptedEvents;
        }

        public async Task<EventResponseModel> GetAsync(CancellationToken cancellationToken, int id)
        {
            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == id && x.Status == EntityStatuses.Active).ConfigureAwait(false))
            {
                throw new ItemNotFoundException(ClassNames.Event + " " + ErrorMessages.NotFound, nameof(User));
            }

            var @event = await _eventRepository.GetAsync(cancellationToken, id).ConfigureAwait(false);
            var result = @event.Adapt<EventResponseModel>();
            return result;
        }

        public async Task<List<EventResponseModel>> GetAllUnconfirmedAsync(CancellationToken cancellationToken)
        {
            var unconfirmedEvents = await _eventRepository.GetAllAsync(cancellationToken, x => x.Status == EntityStatuses.Active && x.IsArchived == false && x.IsActive == false).ConfigureAwait(false);
            return unconfirmedEvents.Adapt<List<EventResponseModel>>();
        }

        //public async Task<List<EventResponseModel>> GetUserEventsAsync(CancellationToken cancellationToken, string userId)
        //{
        //    var userEvents = await _eventRepository.GetUserEventsAsync(cancellationToken, userId).ConfigureAwait(false);
        //    return userEvents.Adapt<List<EventResponseModel>>();
        //}

        public async Task<int> UpdateAsync(CancellationToken cancellationToken, UpdateEventRequestModel eventRequest, string userId)
        {
            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == eventRequest.Id
                && x.Status == EntityStatuses.Active && x.OwnerId == userId).ConfigureAwait(false))
            {
                throw new ItemNotFoundException(ClassNames.Event + " " + ErrorMessages.NotFound, nameof(User));
            }
            var retrievedEvent = await GetAsync(cancellationToken, eventRequest.Id).ConfigureAwait(false);
            if (retrievedEvent.ModificationDeadline <= DateTime.Now)
            {
                throw new EventModificationException(ErrorMessages.EventModificationForbidden,nameof(Event));
            }

            var @event = eventRequest.Adapt<Event>();
            @event.IsActive = false;
            @event.OwnerId = userId;
            var result = await _eventRepository.UpdateAsync(cancellationToken, @event).ConfigureAwait(false);
            return result;
        }
        public async Task<int> UpdateEventByAdminAsync(CancellationToken cancellationToken, AdminUpdateEventRequestModel eventRequest)
        {
            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == eventRequest.Id
                && x.Status == EntityStatuses.Active).ConfigureAwait(false))
            {
                throw new ItemNotFoundException(ClassNames.Event + " " + ErrorMessages.NotFound, nameof(User));
            }

            var @event = eventRequest.Adapt<Event>();
            var result = await _eventRepository.UpdateAsync(cancellationToken, @event).ConfigureAwait(false);
            return result;
        }

        public async Task<int> ArchiveEvent(CancellationToken cancellationToken, int id)
        {
            var @event = await GetAsync(cancellationToken, id).ConfigureAwait(false);
            var archivedEvent = @event.Adapt<Event>();
            archivedEvent.IsArchived = true;
            archivedEvent.Status = EntityStatuses.Archived;
            var result = await _eventRepository.UpdateAsync(cancellationToken, archivedEvent).ConfigureAwait(false);
            return result;
        }

        public async Task<int> ActivateEvent(CancellationToken cancellationToken, int id)
        {
            var @event = await GetAsync(cancellationToken, id).ConfigureAwait(false);
            var archivedEvent = @event.Adapt<Event>();
            archivedEvent.IsActive = true;
            var result = await _eventRepository.UpdateAsync(cancellationToken, archivedEvent).ConfigureAwait(false);
            return result;
        }

        public async Task<int> SetEventUneditableAsync(CancellationToken cancellationToken, int id)
        {
            var @event = await GetAsync(cancellationToken, id).ConfigureAwait(false);
            var archivedEvent = @event.Adapt<Event>();
            var result = await _eventRepository.UpdateAsync(cancellationToken, archivedEvent).ConfigureAwait(false);
            return result;
        }

        public async Task<List<EventResponseModel>> GetAllArchivedAsync(CancellationToken cancellationToken)
        {
            var unconfirmedEvents = await _eventRepository.GetAllAsync(cancellationToken, x => x.Status == EntityStatuses.Active && x.IsArchived == true).ConfigureAwait(false);
            return unconfirmedEvents.Adapt<List<EventResponseModel>>();
        }
    }
}
