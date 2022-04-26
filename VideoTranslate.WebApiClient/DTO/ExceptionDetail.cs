namespace VideoTranslate.WebApiClient.DTO
{
    public record ExceptionDetail
    {
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }

        public ExceptionDetail()
        {
            this.ErrorMessage = string.Empty;
            this.StackTrace = string.Empty;
            this.Source = string.Empty;
            this.Message = string.Empty;
        }
    }
}
