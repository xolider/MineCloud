using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using MineCloudApp.ViewModels;
using System.ComponentModel;
using System.Diagnostics;

namespace MineCloudApp.Utils
{
    public class MineCloudNetwork
    {
        private FileHelper FileHelper;

        public MineCloudNetwork(FileHelper fileHelper)
        {
            this.FileHelper = fileHelper;
        }

        public async Task<bool> Login(string login, string password)
        {
            var IsCorrect = false;
            await Task.Run(() =>
            {
                IsCorrect = true;
            });
            return IsCorrect;
        }

        public bool LauncherExists()
        {
            return File.Exists(Path.Combine(FileHelper.MineCloudFolder, FileHelper.LauncherFile));
        }

        public async void DownloadLauncher(Action<int> ProgressChanged, Action FileDownloaded)
        {
            string URL = "https://launcher.mojang.com/download/" + FileHelper.LauncherFile;
            var client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, e) => { ProgressChanged(e.ProgressPercentage); });
            client.DownloadFileCompleted += new AsyncCompletedEventHandler((sender, e) => { ProcessFile(FileDownloaded); });
            await client.DownloadFileTaskAsync(new Uri(URL), Path.Combine(FileHelper.MineCloudFolder, FileHelper.LauncherFile));
        }

        private void ProcessFile(Action finished)
        {
            switch(Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    string command = "-c \"sudo apt install " + Path.Combine(FileHelper.MineCloudFolder, FileHelper.LauncherFile) + "\"";
                    var process = Process.Start("bash", command);
                    process.WaitForExit();
                    break;
            }
            finished();
        }
    }
}
