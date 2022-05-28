using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Services
{
    public class RoleService : IRole
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountService> _log;

        public RoleService(RoleManager<IdentityRole> role, ILogger<AccountService> log)
        {
            _roleManager = role;
            _log = log;
        }

        public async Task<bool> CreateRole(string roleName)
        {
            try
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return false;
            }

        }

        public Task<bool> EditRole(User user, string role)
        {
            throw new NotImplementedException();
        }
    }
}
