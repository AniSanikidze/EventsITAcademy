using EventsITAcademy.Application.Roles.Repositories;
using EventsITAcademy.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Infrastructure.Roles
{
    public class RoleRepository : IRoleRepository
    {
        readonly ApplicationContext _applicationContext;

        #region Ctor
        public RoleRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        #endregion

        public async Task<IdentityUserRole<string>> GetUserRoleAsync(CancellationToken cancellationToken, string userId)
        {
            return await _applicationContext.UserRoles.SingleOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
