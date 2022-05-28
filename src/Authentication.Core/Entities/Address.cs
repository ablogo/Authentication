using System;
using System.Text.Json.Serialization;

namespace Authentication.Core.Entities
{
    public class Address
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string InteriorNumber { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string AditionalNote { get; set; }

        public bool IsDefault { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }

        [JsonIgnore]
        public string? UpdatedBy { get; set; }

        //NP
        [JsonIgnore]
        public long AccountId { get; set; }

        [JsonIgnore]
        public virtual Account Account { get; set; }

        [JsonIgnore]
        public virtual AddressType AddressType { get; set; }

    }
}
