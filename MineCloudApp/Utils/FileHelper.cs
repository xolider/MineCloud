﻿using System;
using System.Text.Json;
using System.IO;
using MineCloudApp.Models;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Media.Imaging;

namespace MineCloudApp.Utils
{
    public class FileHelper
    {
        private string AppData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public string MineCloudFolder => Path.Combine(AppData, ".minecloud");

        public string MinecraftSavesDirectory => Path.Combine(Path.Combine(AppData, ".minecraft"), "saves");

        public string LauncherFile { get; set; }

        private MineCloudFileWatcher Watcher;

        public FileHelper()
        {
            if(!Directory.Exists(MineCloudFolder))
            {
                Directory.CreateDirectory(MineCloudFolder);
            }
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    LauncherFile = "Minecraft.exe";
                    break;

                case PlatformID.Unix:
                    LauncherFile = "Minecraft.deb";
                    break;

                case PlatformID.MacOSX:
                    LauncherFile = "Minecraft.dmg";
                    break;
            }
        }

        public void SaveUser(IUser user)
        {
            string json = JsonSerializer.Serialize(user);
            string filePath = Path.Combine(MineCloudFolder, "user.json");
            File.WriteAllText(filePath, json);
        }

        public static IBitmap ParseString(string path)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            return new Bitmap(assets.Open(new System.Uri(path)));
        }

        public void StartWatching()
        {
            Watcher = new MineCloudFileWatcher(MinecraftSavesDirectory);
            Watcher.StartWatching();
        }

        public IList<string> StopWatching()
        {
            Watcher.StopWatching();
            return Watcher.Files;
        }
    }
}
