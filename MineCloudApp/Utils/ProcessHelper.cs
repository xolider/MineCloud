using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MineCloudApp.Utils
{
    public class ProcessHelper
    {
        private FileHelper FileHelper;
        public ProcessHelper(FileHelper FileHelper)
        {
            this.FileHelper = FileHelper;
        }
        public async void startProcessAndWatch(Action<IList<string>> OnCompleted)
        {
            string path = "";
            switch(Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    path = Path.Combine(FileHelper.MineCloudFolder, FileHelper.LauncherFile);
                    break;
                case PlatformID.Unix:
                    path = "minecraft-launcher";
                    break;
            }
            var process = Process.Start(path);
            App.Window.WindowState = Avalonia.Controls.WindowState.Minimized;
            FileHelper.StartWatching();
            await Task.Run(() =>
            {
                process.WaitForExit();
            });
            OnCompleted(FileHelper.StopWatching());
            App.Window.WindowState = Avalonia.Controls.WindowState.Normal;
        }
    }
}
