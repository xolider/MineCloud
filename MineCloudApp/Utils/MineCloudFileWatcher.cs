using System.IO;
using System.Collections.Generic;

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

            watcher.IncludeSubdirectories = false;

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
            var line = e.FullPath;
            if(!Files.Contains(line))
            {
                Files.Add(line);
            }
        }
    }
}
