using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using VideoTranslate.Shared.Abstractions.Repositories;
using VideoTranslate.Shared.DTO.Configuration;

namespace VideoTranslate.DataAccess.Repositories
{
    public class FileRepository : BaseRepository, IFileRepository
    {
        public const string ActivitySourceName = nameof(FileRepository);

        private static readonly Version Version = typeof(FileRepository).Assembly.GetName().Version;
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, Version?.ToString());
        private readonly ILogger<FileRepository> logger;

        public FileRepository(
            ConnectionStringConfiguration connectionStringConfiguration,
            ILogger<FileRepository> logger)
            : base(connectionStringConfiguration.Main)
        {
            this.logger = logger;
        }

        public Shared.DTO.File GetFile(Guid fileId)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(FileRepository),
                () =>
                {
                    var sql = "SELECT * FROM [dbo].[File] WHERE Id = @Id";
                    var file = this.QuerySingle<Shared.DTO.File>(sql, new { Id = fileId });
                    return file;
                },
                nameof(this.GetFile));
        }

        public Guid InsertFile(Shared.DTO.File file)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(FileRepository),
                () =>
                {
                    var sql = @"INSERT INTO [dbo].[File]
                        (
                            [FileName], 
                            [FileServerId],
                            [FileTypeId],
                            [Size], 
                            [Extension],
                            [FullPath],
                            [Url]
                        )
                        OUTPUT INSERTED.Id
                        VALUES
                        (
                            @FileName, 
                            @FileServerId,
                            @FileTypeId,
                            @Size,
                            @Extension,
                            @FullPath,
                            @Url
                        )";

                    var fileId = this.QuerySingle<Guid>(
                        sql,
                        new
                        {
                            FileName = file.FileName,
                            FileServerId = file.FileServerId,
                            FileTypeId = file.FileTypeId,
                            Size = file.Size,
                            Extension = file.Extension,
                            FullPath = file.FullPath,
                            Url = file.Url
                        });

                    return fileId;
                },
                nameof(this.InsertFile));
        }
    }
}
