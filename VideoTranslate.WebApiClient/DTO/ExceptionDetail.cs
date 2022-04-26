namespace VideoTranslate.WebApiClient.DTO
{
    public record ExceptionDetail
    {
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
    }
}
