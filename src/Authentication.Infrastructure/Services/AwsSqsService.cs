using Amazon.SQS;
using Amazon.SQS.Model;
using Authentication.Core.Dtos;
using Authentication.Core.Interfaces;
using Authentication.Infrastructure.ExtensionMethods;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Services
{
    public class AwsSqsService : IAwsSqs
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly ILogger<AwsSqsService> _log;
        private readonly AwsConfiguration _awsConfiguration;

        public AwsSqsService(IAmazonSQS awsSQSClient, IOptions<AwsConfiguration> awsConfiguration, ILogger<AwsSqsService> log)
        {
            _sqsClient = awsSQSClient;
            _awsConfiguration = awsConfiguration.Value;
            _log = log;
        }

        public async Task<bool> SendMessage(string message, string attribute, string groupId)
        {
            try
            {
                var sendMessageRequest = new SendMessageRequest()
                {
                    QueueUrl = _awsConfiguration.QueueEmailUrl,
                    MessageBody = message.StringToBase64(),
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
                    {
                        { "type", new MessageAttributeValue() { StringValue = attribute.ToString(), DataType = "Number" } }
                    },
                    MessageGroupId = groupId,
                };

                var responseMessage = await _sqsClient.SendMessageAsync(sendMessageRequest);
                return responseMessage.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendMessage(string message, string attribute)
        {
            try
            {
                var sendMessageRequest = new SendMessageRequest()
                {
                    QueueUrl = _awsConfiguration.QueueEmailUrl,
                    MessageBody = message.StringToBase64(),
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
                    {
                        { "type", new MessageAttributeValue() { StringValue = attribute, DataType = "Number" } }
                    }
                };

                var responseMessage = await _sqsClient.SendMessageAsync(sendMessageRequest);
                return responseMessage.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return false;
            }
        }
    }
}
