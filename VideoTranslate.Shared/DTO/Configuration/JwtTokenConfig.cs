namespace VideoTranslate.Shared.DTO.Configuration
{
    public class JwtTokenConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string IConnectFxApiApplicationID { get; set; }
        public int AccessTokenExpiresInSeconds { get; set; }
        public int RefreshTokenExpiresInSeconds { get; set; }
    }
}
