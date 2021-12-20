using Hangfire.Jobs;
using Microsoft.Extensions.FileProviders;

namespace Hangfire.UI.Services
{
    public class ArchiveService : IArchiveService
    {
        private readonly IBackgroundJobClient _client;
        private readonly IFileProvider _fileProvider;

        public ArchiveService(IBackgroundJobClient client, IFileProvider fileProvider)
        {
            this._client = client;
            this._fileProvider = fileProvider;
        }
        
        public string ScheduleZipArchiveJob(string whatToCompress, string storePath)
        {
            whatToCompress = _fileProvider.GetFileInfo(whatToCompress).PhysicalPath;
            return _client.Enqueue<DirectoryZipArchiveJob>(i =>
                i.Execute(whatToCompress, storePath)
            );
        }
    }
}