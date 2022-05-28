using Authentication.Core.Constants;
using Authentication.Core.Dtos;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Authentication.Infrastructure.ExtensionMethods;
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
        public async Task<ServiceResult> CreateAddress(string user_id, AddressDto address_dto)
        {
            var result = new ServiceResult();
            try
            {
                var user = await _userManager.Users.Where(x => x.Id == user_id).Include(x => x.Account.Addresses).FirstOrDefaultAsync();
                var addressType = await _unitOfWork.Repository<AddressType>().GetById(address_dto.TypeId);
                if (user != null)
                {

                    if (user.Account.Addresses == null)
                    {
                        user.Account.Addresses = new List<Address>();
                    }

                    var newAddress = address_dto.ToEntity();
                    newAddress.AddressType = addressType;

                    user.Account.Addresses.Add(newAddress);

                    await _userManager.UpdateAsync(user);
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

        public async Task<Address> GetAddress(string address_id)
        {
            try
            {
                var address = await _unitOfWork.Repository<Address>().GetById(address_id);
                if (address != null)
                {
                    return address;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return null;
        }

        public async Task<bool> UpdateAddress(string user_id, AddressDto address)
        {
            try
            {
                var addressDb = await _unitOfWork.Repository<Address>().GetById(address.Id);
                if (addressDb != null)
                {
                    var addressType = await _unitOfWork.Repository<AddressType>().GetById(address.TypeId);

                    addressDb.AditionalNote = address.AditionalNote;
                    addressDb.City = address.City;
                    addressDb.Country = address.Country;
                    addressDb.Email = address.Email;
                    addressDb.InteriorNumber = address.InteriorNumber;
                    addressDb.Name = address.Name;
                    addressDb.Number = address.Number;
                    addressDb.Phone = address.Phone;
                    addressDb.State = address.State;
                    addressDb.Street = address.Street;
                    addressDb.ZipCode = address.ZipCode;
                    addressDb.IsDefault = address.IsDefault;
                    addressDb.AddressType = addressType;

                    await _unitOfWork.Repository<Address>().Update(addressDb);
                    await _unitOfWork.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
            }
            return false;

        }

        public async Task<bool> Delete(string user_id, string address_id)
        {
            try
            {
                await _unitOfWork.Repository<Address>().Delete(address_id);
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return false;
            }
        }

    }
}
