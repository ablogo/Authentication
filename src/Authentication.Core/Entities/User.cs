using Microsoft.AspNetCore.Identity;

namespace Authentication.Core.Entities
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; } = true;

        public virtual Account Account { get; set; }

    }
}
