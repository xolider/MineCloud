using System;
using System.Collections.Generic;
using System.Text;
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
        public async void startProcessAndWatch()
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
            await Task.Run(() =>
            {
                process.WaitForExit();
            });
            App.Window.WindowState = Avalonia.Controls.WindowState.Normal;
        }
    }
}
