using Authentication.Core.Entities;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IRole
    {
        Task<bool> CreateRole(string roleName);

        Task<bool> EditRole(User user, string role);

    }
}
