using System;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Core.Dtos
{
    public class RegisterDto
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string SecondLastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Phone]
        [MinLength(12)]
        public string Phone { get; set; }

        public DateTime BirthDay { get; set; }
    }
}
