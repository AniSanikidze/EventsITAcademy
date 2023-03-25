using EventsITAcademy.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Roles
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AssignRoleToUserAsync(CancellationToken cancellationToken,string userId, string userRoleName)
        {
            var userRoleInDb = await GetUserRoleAsync(cancellationToken,userId);
            var rolesInDb = GetRolesAsync(cancellationToken);
            var chosenRole = rolesInDb.First(x => x.Id == userRoleName);
            if (userRoleInDb == chosenRole.Name)
            {
                throw new Exception("User is already assigned to this role");
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            await _userManager.RemoveFromRoleAsync(user, userRoleInDb);
            await _userManager.AddToRoleAsync(user, chosenRole.Name);
        }

        public async Task RemoveUserRoleAsync(CancellationToken cancellationToken, string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
                throw new Exception("User not found");

            var userRoleInDb = await GetUserRoleAsync(cancellationToken, userId);

            if(userRoleInDb == null)
            {
                throw new Exception("User's Role not found");
            }
            await _userManager.RemoveFromRoleAsync(user, userRoleInDb);
        }

        public List<IdentityRole> GetRolesAsync(CancellationToken cancellationToken)
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<String> GetUserRoleAsync(CancellationToken cancellationToken, string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
                throw new Exception("User not found");

            var userRole = await _userManager.GetRolesAsync(user);
            return userRole.ElementAt(0);
        }


    }
}
