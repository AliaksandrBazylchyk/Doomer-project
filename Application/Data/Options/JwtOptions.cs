namespace Application.Data.Options
{
    public class JwtOptions
    {
        public const string SectionName = "JWT";
        public string Key { get; set; }
        public bool ValidateIssuer { get; set; }
        public string Issuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
        public double RefreshTokenDurationInMinutes { get; set; }
    }
}
