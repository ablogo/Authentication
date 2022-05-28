using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IAwsSqs
    {
        Task<bool> SendMessage(string message, string attribute);

        Task<bool> SendMessage(string message, string attribute, string groupId);
    }
}
