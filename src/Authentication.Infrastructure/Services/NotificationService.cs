using Authentication.Core.Constants;
using Authentication.Core.Dtos;
using Authentication.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Services
{
    public class NotificationService : INotification
    {
        private readonly IAwsSqs _awsSqsService;
        private readonly ILogger<NotificationService> _log;
        private readonly AwsConfiguration _awsConfiguration;

        public NotificationService(IAwsSqs awsSqs, IOptions<AwsConfiguration> awsConfiguration, ILogger<NotificationService> log)
        {
            _awsSqsService = awsSqs;
            _awsConfiguration = awsConfiguration.Value;
            _log = log;
        }

        public async Task<bool> SendEmailRequest(string user_id, string user_name, string user_email, string message, Notification.Types email_type)
        {
            try
            {
                var emailToSend = new EmailToSend
                {
                    Email = user_email,
                    Message = message,
                    UserId = user_id,
                    UserName = user_name,
                    NotificationType = (int)email_type
                };

                await _awsSqsService.SendMessage(JsonSerializer.Serialize(emailToSend), emailToSend.NotificationType.ToString(), _awsConfiguration.QueueEmailUrl);
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendSMSRequest(string phone_number, string name, string code, string message, Notification.Types notification_type)
        {
            try
            {
                var smsToSend = new SMSToSend
                {
                    Code = code,
                    Name = name,
                    NotificationType = (int)notification_type,
                    PhoneNumber = phone_number
                };

                await _awsSqsService.SendMessage(JsonSerializer.Serialize(smsToSend), smsToSend.NotificationType.ToString(), _awsConfiguration.QueueEmailUrl);
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return false;
            }
        }

    }
}
