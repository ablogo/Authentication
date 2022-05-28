using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Authentication.Core.Entities
{
    public class Account
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string SecondLastName { get; set; }

        public DateTime? BirthDay { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public string? UpdatedBy { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        public virtual List<Address> Addresses { get; set; }

    }
}
