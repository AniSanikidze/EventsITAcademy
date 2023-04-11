using Microsoft.AspNetCore.Identity;

namespace EventsITAcademy.Application.Roles
{
    public interface IRoleService
    {
        Task AssignRoleToUserAsync(CancellationToken cancellationToken, string userId, string userRoleName);
        List<IdentityRole> GetRolesAsync(CancellationToken cancellationToken);
        Task<string> GetUserRoleAsync(CancellationToken cancellationToken, string userId);
        Task RemoveUserRoleAsync(CancellationToken cancellationToken, string userId);
    }
}
