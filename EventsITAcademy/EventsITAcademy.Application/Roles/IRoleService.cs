using EventsITAcademy.Domain.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Roles
{
    public interface IRoleService
    {
        Task AssignRoleToUserAsync(CancellationToken cancellationToken, string userId, string userRoleName);
        List<IdentityRole> GetRolesAsync(CancellationToken cancellationToken);
        Task<String> GetUserRoleAsync(CancellationToken cancellationToken, string userId);

    }
}
