using Authentication.Core.Dtos;
using Authentication.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IAccount
    {
        Task<bool> AddRole(User user, string role);

        Task<ServiceResult<User>> Create(User new_user, string password);

        Task<ServiceResult> CreateAddress(string user_id, AddressDto address);

        Task<User> GetUser(string user_id, bool include_address);

        Task<User> GetUserByEmail(string email);

        Task<List<string>> GetUserRoles(User user);

        Task<ServiceResult> GetEmailConfirmationToken(string email);

        Task<ServiceResult> GetResetPasswordToken(string email);

        Task<ServiceResult> GetSMSConfirmationCode(string email, string phone_number);

        Task<ServiceResult> EmailConfirmation(string email, string token);

        Task<Address> GetAddress(string address_id);

        Task<bool> Delete(string user_id);

        Task<bool> Delete(string user_id, string address_id);

        Task<bool> Update(UserUpdate user);

        Task<ServiceResult> UpdatePassword(string user_id, string old_password, string new_password);

        Task<bool> UpdateAddress(string user_id, AddressDto user);

        Task<ServiceResult> PhoneConfirmation(string email, string phone_number, string code);

        Task<ServiceResult> ResetPassword(string email, string token, string new_password);

    }
}
