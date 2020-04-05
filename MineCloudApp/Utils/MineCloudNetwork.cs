using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using MineCloudApp.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using MineCloudApp.Models;

namespace MineCloudApp.Utils
{
    public class MineCloudNetwork
    {
        private FileHelper FileHelper;

        public MineCloudNetwork(FileHelper fileHelper)
        {
            this.FileHelper = fileHelper;
        }

        public async Task<IUser> Login(string login, string password)
        {
            User user = null;
            await Task.Run(() =>
            {
                user = new User();
            });
            return user;
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
