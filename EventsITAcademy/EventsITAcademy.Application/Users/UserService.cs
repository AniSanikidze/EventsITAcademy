using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Application.Users.Requests;
using EventsITAcademy.Application.Users.Responses;
using EventsITAcademy.Domain;
using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            IUserRepository userRepository,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _roleManager = roleManager;
        }

        public async Task<LoggedInUserResponseModel> AuthenticateAsync(CancellationToken cancellation, LoginUserRequestModel user)
        {
            var userEntity = user.Adapt<User>();
            //userEntity.Password = GenerateHash(userEntity.Password);
            var retrievedUser = _userManager.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            var role = _userManager.GetRolesAsync(retrievedUser).Result;


            //var retrievedUser = await _userRepository.GetByEmailAsync(cancellation, userEntity.Email);
            //var retrievedUser = await _userManager.FindByEmailAsync(user.Email);
            if (retrievedUser == null)
                throw new Exception("User does not exist");

            _signInManager.Options.SignIn.RequireConfirmedAccount = false;


            var result = await _signInManager.PasswordSignInAsync(retrievedUser.UserName, user.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                Console.WriteLine("Succeeded");
                //_logger.LogInformation("User logged in.");
                //return LocalRedirect(returnUrl);
            }
            else
            {
                throw new Exception("Invalid Login Attempt");
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


            var result = await _userManager.CreateAsync(userEntity, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userEntity, "USER");
            }
            return userEntity.Adapt<UserResponseModel>();

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //if (ModelState.IsValid)
            //{
            //    var user = CreateUser();

            //    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            //    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            //    var result = await _userManager.CreateAsync(user, Input.Password);

            //    if (result.Succeeded)
            //    {
            //        _logger.LogInformation("User created a new account with password.");

            //        var userId = await _userManager.GetUserIdAsync(user);
            //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //        var callbackUrl = Url.Page(
            //            "/Account/ConfirmEmail",
            //            pageHandler: null,
            //            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
            //            protocol: Request.Scheme);

            //        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
            //            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            //        if (_userManager.Options.SignIn.RequireConfirmedAccount)
            //        {
            //            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
            //        }
            //        else
            //        {
            //            await _signInManager.SignInAsync(user, isPersistent: false);
            //            return LocalRedirect(returnUrl);
            //        }
            //    }
            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError(string.Empty, error.Description);
            //    }
            //}
            //return user;
        }

        public async Task<List<UserResponseModel>> GetAllUsersAsync(CancellationToken cancellation)
        {
            var users = await _userRepository.GetAllAsync(cancellation);
            return users.Adapt<List<UserResponseModel>>();
        }
    }
}