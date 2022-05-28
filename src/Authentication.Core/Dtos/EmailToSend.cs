namespace Authentication.Core.Dtos
{
    public class EmailToSend
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public int NotificationType { get; set; }

    }
}
