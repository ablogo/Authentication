namespace Authentication.Core.Dtos
{
    public class SMSToSend
    {
        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int NotificationType { get; set; }

    }
}
