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
            var user = await Network.Login(Model.Pseudo, Model.Password);
            if(user != null)
            {
                ChangeContent(ViewModels.Main);
            }
        }

        private async void SignupUser(SignupModel Model)
        {
            
        }

        private void ChangeContent(ViewModels Model)
        {
            switch(Model)
            {
                case ViewModels.Main:
                    Content = new MainViewModel { ButtonText = Network.LauncherExists() ? "Start" : "Download", InfoText = Network.LauncherExists() ? "Prêt" : "Téléchargement requis"};

                    Observable.Merge(((MainViewModel)Content).DownloadButton).Subscribe(delegate
                    {
                        if(!Network.LauncherExists())
                        {
                            ((MainViewModel)Content).InfoText = "Downloading Launcher...";
                            Network.DownloadLauncher(((MainViewModel)Content).ProgressChanged, ((MainViewModel)Content).FileDownloaded);
                        }
                        else
                        {
                            ((MainViewModel)Content).InfoText = "Starting Minecraft...";
                            ProcessHelper.startProcessAndWatch();
                        }
                    });
                    break;
                case ViewModels.Login:
                    Content = new LoginViewModel();

                    Observable.Merge(((LoginViewModel)Content).ConnectButton, ((LoginViewModel)Content).SignupButton.Select(_ => (LoginModel)null)).Subscribe(model =>
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

                    Observable.Merge(((SignupViewModel)Content).SignupButton, ((SignupViewModel)Content).ConnectButton.Select(_ => (SignupModel)null)).Subscribe(model =>
                    {
                        if(model != null)
                        {
                            SignupUser(model);
                        }
                        else
                        {
                            ChangeContent(ViewModels.Login);
                        }
                    });
                    break;
            }
        }
    }
}
