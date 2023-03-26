using EventsITAcademy.Application.CustomExceptions;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Application.Users.Requests;
using EventsITAcademy.Application.Users.Responses;
using EventsITAcademy.Domain.Users;
using Utilities.Localizations;

namespace EventsITAcademy.Application.Users
{
    public interface IUserService
    {
        Task<LoggedInUserResponseModel> AuthenticateAsync(CancellationToken cancellation, LoginUserRequestModel user);
        Task<UserResponseModel> CreateAsync(CancellationToken cancellation, CreateUserRequestModel user);
        Task<List<UserResponseModel>> GetAllUsersAsync(CancellationToken cancellation);
        Task<List<EventResponseModel>> GetUserEventsAsync(CancellationToken cancellation, string userId);
        Task CheckIfUserExists(CancellationToken cancellation, string userId);
        Task DeleteUserAsync(CancellationToken cancellation, string userId);
    }
}
