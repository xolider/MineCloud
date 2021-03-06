﻿using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using MineCloudApp.Models;
using System.Text.Json;
using System.IO.Compression;

namespace MineCloudApp.Utils
{
    public class MineCloudNetwork
    {
        private FileHelper FileHelper;

        public MineCloudNetwork(FileHelper fileHelper)
        {
            this.FileHelper = fileHelper;
        }

        public async Task<IUser> Login(string login, string password, bool save)
        {
            User user = null;
            await Task.Run(() =>
            {
                user = JsonSerializer.Deserialize<User>("{\"Id\": 1, \"Username\": \"Xolider\", \"Email\": \"clem.vicart@gmail.com\"}");
            });
            if(user != null)
            {
                FileHelper.SaveUser(user);
            }
            return user;
        }

        public IUser SavedUser()
        {
            User user = null;
            string filePath = Path.Combine(FileHelper.MineCloudFolder, "user.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                user = JsonSerializer.Deserialize<User>(json);
            }
            return user;
        }

        public void DeleteSavedUser()
        {
            string filePath = Path.Combine(FileHelper.MineCloudFolder, "user.json");
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
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

        public async Task ProcessSaving(IList<string> files)
        {
            
            foreach(string path in files)
            {
                if(Directory.Exists(path))
                {
                    var destPath = Path.Combine(FileHelper.MineCloudTemp, path.Replace(FileHelper.MinecraftSavesDirectory, "").Substring(1)) + ".zip";
                    ZipFile.CreateFromDirectory(path, destPath, CompressionLevel.Optimal, true);
                    await SendFile(destPath);
                    File.Delete(destPath);
                }
                else
                {
                    await SendDeleteFile(path.Replace(FileHelper.MinecraftSavesDirectory, "").Substring(1) + ".zip");
                }
            }
            
        }

        private async Task SendFile(string filePath)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "binary/octet-stream");

            await client.UploadFileTaskAsync("https://minecloud.fr/upload.php?user=" + User.CurrentUser.Id, "POST", filePath);
        }

        private async Task SendDeleteFile(string filePath)
        {
            WebClient client = new WebClient();
            await client.DownloadStringTaskAsync("https://minecloud.fr/delete.php?user=" + User.CurrentUser.Id + "&file=" + WebUtility.UrlEncode(filePath));
        }
    }
}
