using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MineCloudApp.Utils
{
    internal class MineCloudFileWatcher
    {
        private string DirectoryPath;

        private bool DoStopWatching = false;

        public IList<string> Files { get; }
        public MineCloudFileWatcher(string direcotryPath)
        {
            DirectoryPath = direcotryPath;
            Files = new List<string>();
        }

        public void StartWatching()
        {
            var watcher = new FileSystemWatcher();

            watcher.Path = DirectoryPath;

            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;

            watcher.Filter = "";

            watcher.IncludeSubdirectories = true;

            watcher.Created += OnChanged;
            watcher.Changed += OnChanged;

            watcher.EnableRaisingEvents = !DoStopWatching;
        }

        public void StopWatching()
        {
            DoStopWatching = true;
        }

        private void OnChanged(object souce, FileSystemEventArgs e)
        {
            var line = "Created: " + e.FullPath;
            if(!Files.Contains(line) && !Directory.Exists(e.FullPath))
            {
                Files.Add(line);
            }
        }
    }
}
