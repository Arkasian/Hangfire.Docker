namespace Hangfire.UI.Services
{
    public interface IArchiveService
    {
        string ScheduleZipArchiveJob(string whatToCompress, string storePath);
    }
}