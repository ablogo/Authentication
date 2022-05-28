using Authentication.Core.Constants;
using Authentication.Core.Dtos;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Services
{
    public class AuthenticationService : IAuthentication
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountService> _log;

        public AuthenticationService(SignInManager<User> signInManager, ILogger<AccountService> logger)
        {
            _signInManager = signInManager;
            _log = logger;
        }

        public async Task<ServiceResult> Login(string user, string password, bool lockoutOnFailure)
        {
            var result = new ServiceResult();
            try
            {
                var resultSignIn = await _signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure);
                result.Succeeded = resultSignIn.Succeeded;
                if (resultSignIn.IsLockedOut)
                {
                    result.Message = FailedMessages.UserBlocked;
                }
                else if (resultSignIn.IsNotAllowed)
                {
                    result.Message = FailedMessages.UserNotConfirmed;
                }

            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return result;
        }

        public async Task<ServiceResult> RefreshToken(string user, string password)
        {
            var result = new ServiceResult();
            try
            {
                var resultSignIn = await _signInManager.PasswordSignInAsync(user, password, false, false);
                result.Succeeded = resultSignIn.Succeeded;
                if (resultSignIn.IsLockedOut)
                {
                    result.Message = FailedMessages.UserBlocked;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return result;
        }
    }
}
