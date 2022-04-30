using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoTranslate.Shared.Abstractions.Repositories;
using VideoTranslate.Shared.DTO;
using VideoTranslate.Shared.DTO.Configuration;

namespace VideoTranslate.DataAccess.Repositories
{
    public class FileServerRepository : BaseRepository, IFileServerRepository
    {
        public const string ActivitySourceName = nameof(FileServerRepository);

        private static readonly Version Version = typeof(FileServerRepository).Assembly.GetName().Version;
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, Version?.ToString());
        private readonly ILogger<FileServerRepository> logger;

        public FileServerRepository(
            ConnectionStringConfiguration connectionStringConfiguration,
            ILogger<FileServerRepository> logger)
            : base(connectionStringConfiguration.Main)
        {
            this.logger = logger;
        }

        public FileServer GetActiveFileServer()
        {
            return this.TraceAction(
                ActivitySource,
                nameof(FileServerRepository),
                () =>
                {
                    var sql = "SELECT * FROM [dbo].[FileServer] WHERE IsActive = 1";
                    var video = this.QuerySingle<FileServer>(sql);
                    return video;
                },
                nameof(this.GetActiveFileServer));
        }
    }
}
