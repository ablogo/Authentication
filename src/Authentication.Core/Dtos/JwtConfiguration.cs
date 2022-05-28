namespace Authentication.Core.Dtos
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }

        public int ExpirationTime { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

    }
}
