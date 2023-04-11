using EventsITAcademy.Application.CustomExceptions;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Application.Users.Requests;
using EventsITAcademy.Application.Users.Responses;
using EventsITAcademy.Domain;
using EventsITAcademy.Domain.Users;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Utilities.Localizations;

namespace EventsITAcademy.Application.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private const string DefaultRole = "USER";

        public UserService(UserManager<User> userManager,SignInManager<User> signInManager,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        public async Task<LoggedInUserResponseModel> AuthenticateAsync(CancellationToken cancellation, LoginUserRequestModel user)
        {
            var userEntity = user.Adapt<User>();
            var retrievedUser = _userManager.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            if (retrievedUser == null || retrievedUser.Status == EntityStatuses.Deleted)
                throw new ItemNotFoundException(ClassNames.User + " " + ErrorMessages.NotFound, nameof(User));

            var role = _userManager.GetRolesAsync(retrievedUser).Result;

            //_signInManager.Options.SignIn.RequireConfirmedAccount = false;
            var result = await _signInManager.PasswordSignInAsync(retrievedUser.UserName, user.Password, false, lockoutOnFailure: false).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                throw new InvalidLoginException(ErrorMessages.InvalidLogin, nameof(User));
            }
            var loggedInUser = retrievedUser.Adapt<LoggedInUserResponseModel>();
            loggedInUser.Role = role[0];
            return loggedInUser;
        }

        public async Task<UserResponseModel> CreateAsync(CancellationToken cancellation, CreateUserRequestModel user)
        {
            var userEntity = user.Adapt<User>();
            userEntity.CreatedAt = DateTime.Now;
            userEntity.ModifiedAt = DateTime.Now;
            userEntity.Status = EntityStatuses.Active;

            var result = await _userManager.CreateAsync(userEntity, user.Password).ConfigureAwait(false);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userEntity, DefaultRole).ConfigureAwait(false);
            }
            return userEntity.Adapt<UserResponseModel>();
        }

        public async Task<List<UserResponseModel>> GetAllUsersAsync(CancellationToken cancellation)
        {
            var users = await _userRepository.GetAllAsync(cancellation).ConfigureAwait(false);
            return users.Adapt<List<UserResponseModel>>();
        }

        public async Task<List<EventResponseModel>> GetUserEventsAsync(CancellationToken cancellation, string userId)
        {
            await CheckIfUserExists(cancellation, userId).ConfigureAwait(false);
            var userEvents = await _userRepository.GetUserEventsAsync(cancellation, userId).ConfigureAwait(false);
            return userEvents.Adapt<List<EventResponseModel>>();
        }

        public async Task CheckIfUserExists(CancellationToken cancellation,string userId)
        {
            if (!await _userRepository.Exists(cancellation, userId).ConfigureAwait(false))
            {
                throw new ItemNotFoundException(ClassNames.User + " " + ErrorMessages.NotFound, nameof(User));
            }
        }
        public async Task DeleteUserAsync(CancellationToken cancellation, string userId)
        {
            if (!await _userRepository.Exists(cancellation, userId).ConfigureAwait(false))
            {
                throw new ItemNotFoundException(ClassNames.User + " " + ErrorMessages.NotFound, nameof(User));
            }
            await _userRepository.DeleteAsync(cancellation, userId).ConfigureAwait(false);
        }
    }
}
