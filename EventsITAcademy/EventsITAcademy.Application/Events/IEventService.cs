using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;

namespace EventsITAcademy.Application.Events
{
    public interface IEventService
    {
        Task<List<EventResponseModel>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<EventResponseModel>> GetAllConfirmedAsync(CancellationToken cancellationToken);
        Task<List<EventResponseModel>> GetAllArchivedAsync(CancellationToken cancellationToken);
        Task<List<EventResponseModel>> GetAllUnconfirmedAsync(CancellationToken cancellationToken);
        Task<EventResponseModel> GetAsync(CancellationToken cancellationToken, int id);
        Task<int> ArchiveEvent(CancellationToken cancellationToken, int id);
        Task<int> ActivateEvent(CancellationToken cancellationToken, int id);
        Task<int> SetEventUneditableAsync(CancellationToken cancellationToken, int id);
        Task<int> CreateAsync(CancellationToken cancellationToken, EventRequestModel eventRequest, string userId);
        Task<int> UpdateAsync(CancellationToken cancellationToken, UpdateEventRequestModel eventRequest, string userId);
        Task<int> UpdateEventByAdminAsync(CancellationToken cancellationToken, AdminUpdateEventRequestModel eventRequest);
        Task<int> DeleteAsync(CancellationToken cancellationToken, int id);
    }
}
