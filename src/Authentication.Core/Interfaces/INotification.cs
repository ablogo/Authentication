using Authentication.Core.Constants;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface INotification
    {
        Task<bool> SendEmailRequest(string user_id, string user_name, string user_email, string message, Notification.Types notification_type);

        Task<bool> SendSMSRequest(string phone_number, string name, string code, string message, Notification.Types notification_type);
    }
}
