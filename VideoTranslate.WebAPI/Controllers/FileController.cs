﻿using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using VideoTranslate.Shared.Abstractions.Services;
using VideoTranslate.WebApiClient.DTO;

namespace VideoTranslate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> logger;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public FileController(
            ILogger<FileController> logger,
            IMapper mapper,
            IFileService fileService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        [HttpPost("Video/Upload")]
        [RequestSizeLimit(1073741824)]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]
        public VideoInfo UploadVideoFile()
        {
            var file = this.fileService.UploadVideoFile(this.Request.Form.Files[0]);
            var fileModel = this.mapper.Map<VideoInfo>(file);
            return fileModel;
        }
    }
}