namespace Hangfire.Jobs
{
    public abstract class AbstractArchiveJob
    {
        public abstract void Execute(string whatToCompress, string whereToStore);
    }
}