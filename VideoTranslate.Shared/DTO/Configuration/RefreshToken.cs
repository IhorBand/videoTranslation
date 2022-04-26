namespace VideoTranslate.Shared.DTO.Configuration
{
    public class RefreshToken
    {
        public string Username { get; set; }
        public string Value { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}
