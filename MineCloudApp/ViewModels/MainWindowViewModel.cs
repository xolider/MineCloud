using System;
using ReactiveUI;
using System.Reactive.Linq;
using MineCloudApp.Models;
using MineCloudApp.Utils;

namespace MineCloudApp.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _content;

        public ViewModelBase Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        private MineCloudNetwork Network;
        private ProcessHelper ProcessHelper;

        public MainWindowViewModel(MineCloudNetwork Network, ProcessHelper processHelper)
        {
            this.Network = Network;
            this.ProcessHelper = processHelper;

            ChangeContent(ViewModels.Login);
        }

        private async void ConnectUser(LoginModel Model)
        {
            var IsCorrect = await Network.Login(Model.Pseudo, Model.Password);
            if(IsCorrect)
            {
                ChangeContent(ViewModels.Main);
            }
        }

        private void ChangeContent(ViewModels Model)
        {
            switch(Model)
            {
                case ViewModels.Main:
                    Content = new MainViewModel();

                    if(Network.LauncherExists())
                    {
                        ((MainViewModel)Content).ButtonText = "Start";
                    }

                    Observable.Merge(((MainViewModel)Content).DownloadButton).Take(2).Subscribe(delegate
                    {
                        if(!Network.LauncherExists())
                        {
                            Network.DownloadLauncher(((MainViewModel)Content).ProgressChanged, ((MainViewModel)Content).FileDownloaded);
                        }
                        else
                        {
                            ProcessHelper.startProcessAndWatch();
                        }
                    });
                    break;
                case ViewModels.Login:
                    Content = new LoginViewModel();

                    Observable.Merge(((LoginViewModel)Content).ConnectButton, ((LoginViewModel)Content).SignupButton.Select(_ => (LoginModel)null)).Take(1).Subscribe(model =>
                    {
                        if (model != null)
                        {
                            ConnectUser(model);
                        }
                        else
                        {
                            ChangeContent(ViewModels.Signup);
                        }
                    });
                    break;
                case ViewModels.Signup:
                    Content = new SignupViewModel();
                    break;
            }
        }
    }
}
