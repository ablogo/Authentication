using System;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Core.Dtos
{
    public class UserUpdate
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string SecondLastName { get; set; }

        [Required]
        public DateTime? BirthDay { get; set; }
    }
}
