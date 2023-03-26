using EventsITAcademy.Application.CustomExceptions;
using EventsITAcademy.Domain;
using EventsITAcademy.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utilities.Localizations;

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
            var userRoleInDb = await GetUserRoleAsync(cancellationToken,userId).ConfigureAwait(false);
            var rolesInDb = GetRolesAsync(cancellationToken);
            var chosenRole = rolesInDb.First(x => x.Id == userRoleName);
            if (userRoleInDb == chosenRole.Name)
            {
                throw new ItemAlreadyExistsException(ErrorMessages.UserAlreadyAssignedToRole, "RoleAlreadyAssignedToUser");
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);
            await _userManager.RemoveFromRoleAsync(user, userRoleInDb).ConfigureAwait(false);
            await _userManager.AddToRoleAsync(user, chosenRole.Name).ConfigureAwait(false);
        }

        public async Task RemoveUserRoleAsync(CancellationToken cancellationToken, string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken).ConfigureAwait(false);
            if (user == null)
                throw new ItemNotFoundException(ClassNames.User + " " + ErrorMessages.NotFound, nameof(User));

            var userRoleInDb = await GetUserRoleAsync(cancellationToken, userId).ConfigureAwait(false);

            if(userRoleInDb == null)
            {
                throw new ItemNotFoundException(ClassNames.UserRole + " " + ErrorMessages.NotFound, null);
            }
            await _userManager.RemoveFromRoleAsync(user, userRoleInDb).ConfigureAwait(false);
        }

        public List<IdentityRole> GetRolesAsync(CancellationToken cancellationToken)
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<string> GetUserRoleAsync(CancellationToken cancellationToken, string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId && x.Status == EntityStatuses.Active, cancellationToken).ConfigureAwait(false);
            if (user == null)
                throw new ItemNotFoundException(ClassNames.User + " " + ErrorMessages.NotFound, nameof(User));

            var userRole = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            return userRole.ElementAt(0);
        }
    }
}
