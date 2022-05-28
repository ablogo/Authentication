using Authentication.Core.Entities;

namespace Authentication.Core.Dtos
{
    public class AddressDto : Address
    {
        public int TypeId { get; set; }
    }
}
