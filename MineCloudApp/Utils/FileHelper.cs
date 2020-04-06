using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using MineCloudApp.Models;

namespace MineCloudApp.Utils
{
    public class FileHelper
    {
        private string AppData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public string MineCloudFolder => Path.Combine(AppData, ".minecloud");

        public string LauncherFile { get; set; }

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
    }
}
