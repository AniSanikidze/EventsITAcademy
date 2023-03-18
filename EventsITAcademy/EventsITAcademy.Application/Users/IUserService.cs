using EventsITAcademy.Application.Users.Requests;
using EventsITAcademy.Application.Users.Responses;
using EventsITAcademy.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Users
{
    public interface IUserService
    {
        Task<LoggedInUserResponseModel> AuthenticateAsync(CancellationToken cancellation, LoginUserRequestModel user);
        Task<UserResponseModel> CreateAsync(CancellationToken cancellation, CreateUserRequestModel user);
        Task<List<UserResponseModel>> GetAllUsersAsync(CancellationToken cancellation);

    }
}
