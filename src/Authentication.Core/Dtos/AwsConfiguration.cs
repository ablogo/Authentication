namespace Authentication.Core.Dtos
{
    public class AwsConfiguration
    {
        public string QueueEmailUrl { get; set; }

        public string VisibilityTimeout { get; set; }

        public string MaxNumberOfMessages { get; set; }

        public string WaitTimeSeconds { get; set; }
    }
}
