using Authentication.Core.Dtos;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IAuthentication
    {
        Task<ServiceResult> Login(string user, string password, bool lockoutOnFailure);

        Task<ServiceResult> RefreshToken(string user, string password);

    }
}
