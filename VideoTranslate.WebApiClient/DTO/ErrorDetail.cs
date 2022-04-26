namespace VideoTranslate.WebApiClient.DTO
{
    public record ErrorDetail
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorType { get; set; }
        public string Severity { get; set; }

        public ErrorDetail()
        {
            this.ErrorCode = string.Empty;
            this.ErrorMessage = string.Empty;
            this.ErrorType = string.Empty;
            this.Severity = string.Empty;
        }
    }
}
