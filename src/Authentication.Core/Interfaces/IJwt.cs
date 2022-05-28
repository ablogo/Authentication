using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IJwt
    {
        string GenerateToken(string user_id, string user_email, List<string> role, int expiration_time = 60);

        Task<bool> ValidateToken(string token, string secret);

    }
}
