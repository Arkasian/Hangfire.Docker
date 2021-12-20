
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace Hangfire.Jobs
{
    public class DirectoryZipArchiveJob : AbstractArchiveJob
    {
        public override void Execute(string whatToCompress, string whereToStore)
        {
            if (!Directory.Exists(whatToCompress) || !Directory.Exists(whereToStore)) return;
            try
            {
                ZipFile.CreateFromDirectory(whatToCompress,
                    $"{whereToStore}{Path.DirectorySeparatorChar}{new DirectoryInfo(whatToCompress).Name}.zip");
            }
            catch (IOException e)
            {
                Thread.CurrentThread.ExecutionContext?.Dispose();
            }
        }
    }
}
