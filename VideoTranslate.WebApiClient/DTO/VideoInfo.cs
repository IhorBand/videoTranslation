﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranslate.WebApiClient.DTO
{
    public record VideoInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public VideoInfo()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
        }
    }
}