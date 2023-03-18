using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Events
{
    public interface IEventService
    {
        Task<List<EventResponseModel>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<EventResponseModel>> GetUserEventsAsync(CancellationToken cancellationToken, string userId);
        Task<List<EventResponseModel>> GetAllUnconfirmedAsync(CancellationToken cancellationToken);
        Task<EventResponseModel> ConfirmEventAsync(CancellationToken cancellationToken, int id);
        Task<EventResponseModel> GetAsync(CancellationToken cancellationToken, int id);
        Task<EventResponseModel> CreateAsync(CancellationToken cancellationToken, EventRequestModel eventRequest, string userId);
        Task<EventResponseModel> UpdateAsync(CancellationToken cancellationToken, UpdateEventRequestModel eventRequest, string userId, string role);
        //Task<EventResponseModel> PatchAsync(CancellationToken cancellationToken, JsonPatchDocument<ToDoUpdateModel> patchRequest, int id, int userId);
        Task DeleteAsync(CancellationToken cancellationToken, int id, int userId);
    }
}
