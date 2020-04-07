using ReactiveUI;
using System.Reactive;
using MineCloudApp.Lang;
using MineCloudApp.Models;
using Avalonia.Media.Imaging;
using MineCloudApp.Utils;
namespace MineCloudApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> DownloadButton { get; }

        public ReactiveCommand<Unit, Unit> DisconnectButton { get; }

        private int _progressValue = 0;

        public int ProgressValue
        {
            get => _progressValue;
            set => this.RaiseAndSetIfChanged(ref _progressValue, value);
        }

        private string _buttonText;

        public string ButtonText
        {
            get => _buttonText;
            set => this.RaiseAndSetIfChanged(ref _buttonText, value);
        }

        private string _infoText;

        public string InfoText
        {
            get => _infoText;
            set => this.RaiseAndSetIfChanged(ref _infoText, value);
        }

        private string _hello = LanguageController.CurrentLanguage.Hello;

        public string Hello
        {
            get => _hello;
            set => this.RaiseAndSetIfChanged(ref _hello, value);
        }

        private IBitmap _buttonImage;
        public IBitmap ButtonImage
        {
            get => _buttonImage;
            set => this.RaiseAndSetIfChanged(ref _buttonImage, value);
        }

        private IUser User;

        public MainViewModel(object user)
        {
            this.User = user as IUser;
            this.Hello += this.User.Username;
            DownloadButton = ReactiveCommand.Create(() => {  });
            DisconnectButton = ReactiveCommand.Create(() => { });
        }

        public MainViewModel()
        {

        }

        public void ProgressChanged(int Value)
        {
            ProgressValue = Value;
        }

        public void FileDownloaded()
        {
            ButtonText = LanguageController.CurrentLanguage.Start;
            InfoText = LanguageController.CurrentLanguage.Ready;
            ProgressValue = 0;
            ButtonImage = FileHelper.ParseString("avares://MineCloudApp/Assets/Minecraft.png");
        }
    }
}
