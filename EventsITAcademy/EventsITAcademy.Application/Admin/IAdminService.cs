using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Users.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Admin
{
    public interface IAdminService
    {
        Task<EventResponseModel> UpdateEventAsync(CancellationToken cancellationToken, AdminUpdateEventRequestModel eventRequest);
        Task<int> DeleteEventAsync(CancellationToken cancellationToken, int eventId);
        Task<List<UserResponseModel>> GetAllUsersAsync(CancellationToken cancellation);
        Task<UserResponseModel> GetUserAsync(CancellationToken cancellation, string id);
        Task DeleteUserAsync(CancellationToken cancellation, string userId);
    }
}
