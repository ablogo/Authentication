using System;
using System.Collections.Generic;

namespace Authentication.Core.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string SecondLastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? BirthDay { get; set; }

        public List<AddressDto> Address { get; set; }
    }
}
