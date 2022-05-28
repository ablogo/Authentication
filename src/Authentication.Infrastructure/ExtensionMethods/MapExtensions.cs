using Authentication.Core.Dtos;
using Authentication.Core.Entities;
using System;
using System.Collections.Generic;

namespace Authentication.Infrastructure.ExtensionMethods
{
    public static class MapExtensions
    {
        public static UserDto ToDto(this User user_entity, bool is_admin = false)
        {
            try
            {
                if (user_entity != null)
                {
                    var user = new UserDto()
                    {
                        Name = user_entity.Account.Name,
                        Email = user_entity.Email,
                        LastName = user_entity.Account.LastName,
                        SecondLastName = user_entity.Account.SecondLastName,
                        PhoneNumber = user_entity.PhoneNumber,
                        BirthDay = user_entity.Account.BirthDay,
                        Address = user_entity.Account.Addresses != null ? user_entity.Account.Addresses.ToDto() : null
                    };

                    if (is_admin) 
                    {
                        user.Id = user_entity.Id;
                    }
                    return user;
                }
            }
            catch (Exception) { throw; }
            return null;
        }

        public static User ToEntity(this RegisterDto register_dto)
        {
            try
            {
                if (register_dto != null)
                {
                    var user = new User
                    {
                        UserName = register_dto.Email,
                        Email = register_dto.Email,
                        PhoneNumber = register_dto.Phone,
                        Account = new Account
                        {
                            BirthDay = register_dto.BirthDay,
                            LastName = register_dto.LastName,
                            SecondLastName = register_dto.SecondLastName,
                            Name = register_dto.Name
                        }
                    };
                    return user;
                }
            }
            catch (Exception) { throw; }
            return null;
        }

        public static AddressDto ToDto(this Address address)
        {
            try
            {
                if (address != null)
                {
                    var addressDto = new AddressDto()
                    {
                        AddressType = address.AddressType,
                        AditionalNote = address.AditionalNote,
                        City = address.City,
                        Country = address.Country,
                        Email = address.Email,
                        Id = address.Id,
                        InteriorNumber = address.InteriorNumber,
                        IsDefault = address.IsDefault,
                        Name = address.Name,
                        Number = address.Number,
                        Phone = address.Phone,
                        State = address.State,
                        Street = address.Street,
                        ZipCode = address.ZipCode,
                        TypeId = address.AddressType.Id
                    };

                    return addressDto;
                }
            }
            catch (Exception) { throw; }
            return null;
        }

        public static Address ToEntity(this AddressDto address)
        {
            try
            {
                if (address != null)
                {
                    var addressDto = new Address()
                    {
                        AddressType = address.AddressType,
                        AditionalNote = address.AditionalNote,
                        City = address.City,
                        Country = address.Country,
                        Email = address.Email,
                        Id = address.Id,
                        InteriorNumber = address.InteriorNumber,
                        IsDefault = address.IsDefault,
                        Name = address.Name,
                        Number = address.Number,
                        Phone = address.Phone,
                        State = address.State,
                        Street = address.Street,
                        ZipCode = address.ZipCode
                    };

                    return addressDto;
                }
            }
            catch (Exception) { throw; }
            return null;
        }

        public static List<AddressDto> ToDto(this List<Address> addresses)
        {
            try
            {
                var addressesDto = new List<AddressDto>();

                if (addresses != null)
                {
                    foreach (var item in addresses)
                    {
                        addressesDto.Add(item.ToDto());
                    }

                    return addressesDto;
                }
            }
            catch (Exception) { throw; }
            return null;
        }

    }
}
