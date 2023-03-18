using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Domain;
using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EventsITAcademy.Application.Events
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly UserManager<User> _userManager;

        public EventService(IEventRepository toDoRepository, UserManager<User> userManager)
        {
            _eventRepository = toDoRepository;
            _userManager = userManager;
        }

        public async Task<EventResponseModel> CreateAsync(CancellationToken cancellationToken, EventRequestModel eventRequest, string userId)
        {
            var addaptedEvent = eventRequest.Adapt<Event>();
            addaptedEvent.OwnerId = userId;
            await _eventRepository.CreateAsync(cancellationToken, addaptedEvent);
            return addaptedEvent.Adapt<EventResponseModel>();
        }

        public Task DeleteAsync(CancellationToken cancellationToken, int id, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EventResponseModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync(cancellationToken);
            return events.Adapt<List<EventResponseModel>>();
        }

        public async Task<EventResponseModel> GetAsync(CancellationToken cancellationToken, int id)
        {
            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == id && x.Status == EntityStatuses.Active ))
            {
                throw new Exception("Event not found");
            }

            var @event = await _eventRepository.GetAsync(cancellationToken, id);
            return @event.Adapt<EventResponseModel>();
        }
        
        public async Task<List<EventResponseModel>> GetAllUnconfirmedAsync(CancellationToken cancellationToken)
        {
            var unconfirmedEvents = await _eventRepository.GetAllUnconfirmedAsync(cancellationToken);
            return unconfirmedEvents.Adapt<List<EventResponseModel>>();
        }

        public async Task<List<EventResponseModel>> GetUserEventsAsync(CancellationToken cancellationToken, string userId)
        {
            var userEvents = await _eventRepository.GetUserEventsAsync(cancellationToken, userId);
            return userEvents.Adapt<List<EventResponseModel>>();
        }

        public async Task<EventResponseModel> UpdateAsync(CancellationToken cancellationToken, UpdateEventRequestModel eventRequest, string userId, string userRole)
        {
            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == eventRequest.Id 
                && x.Status == EntityStatuses.Active && (x.OwnerId == userId || userRole == "Admin")))
            {
                throw new Exception("Event not found");
            }
            //var retrievedEvent = _eventRepository.GetUserEventAsync(cancellationToken, eventRequest.Id, userId);

            Event @event = eventRequest.Adapt<Event>();
            @event.IsActive = false;
            @event.OwnerId = userId;
            var result = await _eventRepository.UpdateAsync(cancellationToken, @event);
            //var result = await GetAsync(cancellationToken, @event.Id);
            return result.Adapt<EventResponseModel>();
        }

        public async Task<EventResponseModel> ConfirmEventAsync(CancellationToken cancellationToken, int id)
        {
            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == id
                && x.Status == EntityStatuses.Active))
            {
                throw new Exception("Event not found");
            }
            var @event = await GetAsync(cancellationToken, id);
            if(@event.IsActive == true)
            {
                throw new Exception("Event is already confirmed");
            }

            @event.IsActive = true;
            var confirmedEvent = @event.Adapt<Event>();
            var result = await _eventRepository.UpdateAsync(cancellationToken, confirmedEvent);
            //var result = await _eventRepository.ConfirmEvent(cancellationToken, confirmedEvent);
            return result.Adapt<EventResponseModel>();
        }
    }
}
