using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Roles.Repositories
{
    public interface IRoleRepository
    {
        Task<IdentityUserRole<string>> GetUserRoleAsync(CancellationToken cancellationToken, string userId);
    }
}
