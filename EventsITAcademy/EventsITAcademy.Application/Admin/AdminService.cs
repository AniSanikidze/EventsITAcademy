using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Application.Users.Responses;
using EventsITAcademy.Domain;
using EventsITAcademy.Domain.Events;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;


        public AdminService(IEventRepository eventRepository,IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }

        public async Task<int> DeleteEventAsync(CancellationToken cancellationToken, int eventId)
        {
            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == eventId
                    && x.Status == EntityStatuses.Active))
            {
                throw new Exception("Event not found");
            }

            await _eventRepository.DeleteAsync(cancellationToken, eventId);
            return eventId;
        }

        public async Task<EventResponseModel> UpdateEventAsync(CancellationToken cancellationToken, AdminUpdateEventRequestModel eventRequest)
        {
            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == eventRequest.Id
                    && x.Status == EntityStatuses.Active))
            {
                throw new Exception("Event not found");
            }

            Event @event = eventRequest.Adapt<Event>();
            var result = await _eventRepository.UpdateEventModResPeriodsAsync(cancellationToken, @event);
            return result.Adapt<EventResponseModel>();
        }

        public async Task<List<UserResponseModel>> GetAllUsersAsync(CancellationToken cancellation)
        {
            var users = await _userRepository.GetAllAsync(cancellation);
            return users.Adapt<List<UserResponseModel>>();
        }

        public async Task<UserResponseModel> GetUserAsync(CancellationToken cancellation, string id)
        {
            var user = await _userRepository.GetAsync(cancellation, id);
            if(user == null)
            {
                throw new Exception("User not found");
            }
            return user.Adapt<UserResponseModel>();
        }

        public async Task DeleteUserAsync(CancellationToken cancellation, string userId)
        {
            if(!await _userRepository.Exists(cancellation, userId))
            {
                throw new Exception("User not found");
            }
            await _userRepository.DeleteAsync(cancellation, userId);

        }

        public enum UserRolesEnum
        {

        }
    }
}
