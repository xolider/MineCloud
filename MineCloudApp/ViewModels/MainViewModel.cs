using ReactiveUI;
using System.Reactive;

namespace MineCloudApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> DownloadButton { get; }

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

        public MainViewModel()
        {
            DownloadButton = ReactiveCommand.Create(() => {  });
            
        }

        public void ProgressChanged(int Value)
        {
            ProgressValue = Value;
        }

        public void FileDownloaded()
        {
            ButtonText = "Start";
        }
    }
}
