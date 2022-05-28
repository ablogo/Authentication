using Authentication.Core.Constants;
using Authentication.Core.Dtos;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Authentication.Infrastructure.ExtensionMethods;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Services
{
    public partial class AccountService : IAccount
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountService> _log;
        private IRepository<Account> _accountRepo;

        public AccountService(UserManager<User> userManager, ILogger<AccountService> log, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public async Task<ServiceResult<User>> Create(User new_user, string password)
        {
            var result = new ServiceResult<User>();
            try
            {
                var resultCreate = await _userManager.CreateAsync(new_user, password);
                if (resultCreate.Succeeded)
                {
                    result.Object = new_user;

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(new_user);
                        result.Message = token.StringToBase64();
                    }

                    await AddRole(new_user, Roles.User.ToString());
                    result.Succeeded = true;
                }
                else
                {
                    result.Message = GetErrors(resultCreate.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException.Message);
                _log.LogError("Error creating user. Email: " + new_user.Email);
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<bool> AddRole(User user, string role)
        {
            try
            {
                await _userManager.AddToRoleAsync(user, role);
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return false;
            }

        }

        public async Task<bool> Delete(string user_id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(user_id);
                user.IsActive = false;
                user.EmailConfirmed = false;
                user.PhoneNumberConfirmed = false;

                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return false;
            }
        }

        public async Task<ServiceResult> EmailConfirmation(string email, string token)
        {
            var result = new ServiceResult();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var confirmationResult = await _userManager.ConfirmEmailAsync(user, token.Base64ToString());
                    result.Succeeded = confirmationResult.Succeeded;
                    if (!confirmationResult.Succeeded)
                    {
                        result.Message = FailedMessages.UserConfirmationEmail;
                    }
                }
                else
                {
                    result.Message = FailedMessages.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return result;
        }

        public async Task<ServiceResult> GetEmailConfirmationToken(string email)
        {
            var result = new ServiceResult();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    result.Message = token.StringToBase64();
                    result.Succeeded = true;
                }
                else
                {
                    result.Message = FailedMessages.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return result;
        }

        public async Task<ServiceResult> ResetPassword(string email, string token, string new_password)
        {
            var result = new ServiceResult();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var changePasswordResult = await _userManager.ResetPasswordAsync(user, token, new_password);
                    result.Succeeded = changePasswordResult.Succeeded;
                    if (!changePasswordResult.Succeeded)
                    {
                        result.Message = GetErrors(changePasswordResult.Errors.ToList());
                    }
                }
                else
                {
                    result.Message = FailedMessages.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return result;
        }

        public async Task<ServiceResult> GetResetPasswordToken(string email)
        {
            var result = new ServiceResult();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    result.Succeeded = true;
                }
                else
                {
                    result.Message = FailedMessages.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return result;
        }

        public async Task<ServiceResult> GetSMSConfirmationCode(string email, string phone_number)
        {
            var result = new ServiceResult();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    result.Message = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phone_number);
                    result.Succeeded = true;
                }
                else
                {
                    result.Message = FailedMessages.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return result;
        }

        public async Task<ServiceResult> PhoneConfirmation(string email, string phone_number, string code)
        {
            var result = new ServiceResult();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var confirmationResult = await _userManager.ChangePhoneNumberAsync(user, phone_number, code);
                    result.Succeeded = confirmationResult.Succeeded;
                    if (!confirmationResult.Succeeded)
                    {
                        result.Message = FailedMessages.UserConfirmationEmail;
                    }
                }
                else
                {
                    result.Message = FailedMessages.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return result;
        }

        public async Task<bool> Update(UserUpdate user_dto)
        {
            try
            {
                _accountRepo = _unitOfWork.Repository<Account>();
                var user = await _accountRepo.Query().Where(x => x.UserId == user_dto.Id).FirstOrDefaultAsync();
                user.Name = user_dto.Name;
                user.LastName = user_dto.LastName;
                user.SecondLastName = user_dto.SecondLastName;
                user.BirthDay = user_dto.BirthDay;

                await _accountRepo.Update(user);
                await _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return false;
            }
        }

        public async Task<ServiceResult> UpdatePassword(string user_id, string old_password, string new_password)
        {
            var result = new ServiceResult();
            try
            {
                var user = await _userManager.FindByIdAsync(user_id);
                if (user != null)
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, old_password, new_password);
                    result.Succeeded = changePasswordResult.Succeeded;

                    if (!changePasswordResult.Succeeded)
                    {
                        result.Message = GetErrors(changePasswordResult.Errors.ToList());
                    }
                }
                else
                {
                    result.Message = FailedMessages.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return result;
        }

        public async Task<User> GetUser(string user_id, bool include_address)
        {
            try
            {
                var user = include_address ? 
                    await _userManager.Users.Where(x => x.Id == user_id).Include(x => x.Account.Addresses).ThenInclude(x => x.AddressType).FirstOrDefaultAsync() : 
                    await _userManager.FindByIdAsync(user_id);
                return user;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return null;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                return user;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return null;
            }
        }

        public async Task<List<string>> GetUserRoles(User user)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return null;
            }
        }

        private string GetErrors(List<IdentityError> errors)
        {
            try
            {
                var result = "";
                errors.ForEach(x => result += String.Concat(x.Description + Environment.NewLine));
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return "";
            }
        }
        
    }
}
