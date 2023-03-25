﻿using EventsITAcademy.Application.Events.Responses;
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
        //private readonly IUserStore<User> _userStore;
        //private readonly IUserEmailStore<User> _emailStore;
        private readonly IUserRepository _userRepository;
        private const string _defaultRole = "USER";
        //private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<User> userManager,
            //IUserStore<User> userStore,
            SignInManager<User> signInManager,
            IUserRepository userRepository
            //RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            //_roleManager = roleManager;
        }

        public async Task<LoggedInUserResponseModel> AuthenticateAsync(CancellationToken cancellation, LoginUserRequestModel user)
        {
            var userEntity = user.Adapt<User>();
            var retrievedUser = _userManager.Users.Where(u => u.Email == user.Email).FirstOrDefault();
            if (retrievedUser == null || retrievedUser.Status == EntityStatuses.Deleted)
                throw new Exception("User does not exist");

            var role = _userManager.GetRolesAsync(retrievedUser).Result;

            _signInManager.Options.SignIn.RequireConfirmedAccount = false;


            var result = await _signInManager.PasswordSignInAsync(retrievedUser.UserName, user.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
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
                await _userManager.AddToRoleAsync(userEntity, _defaultRole);
            }
            return userEntity.Adapt<UserResponseModel>();
        }

        public async Task<List<UserResponseModel>> GetAllUsersAsync(CancellationToken cancellation)
        {
            var users = await _userRepository.GetAllAsync(cancellation);
            return users.Adapt<List<UserResponseModel>>();
        }

        public async Task<List<EventResponseModel>> GetUserEventsAsync(CancellationToken cancellation, string userId)
        {
            if(!await _userRepository.Exists(cancellation, userId))
            {
                throw new Exception("User Not Found");
            }
            var userEvents = await _userRepository.GetUserEventsAsync(cancellation, userId);
            return userEvents.Adapt<List<EventResponseModel>>();    
        }
    }
}