﻿namespace VideoTranslate.Shared.DTO
{
    public class VideoFile
    {
        public Guid Id { get; set; }
        public Guid VideoInfoId { get; set; }
        public Guid FileId { get; set; }
        public VideoType VideoTypeId { get; set; }
        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public string? Url { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool IsOriginal { get; set; }
    }
}
