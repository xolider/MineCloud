using System;
using ReactiveUI;
using System.Reactive.Linq;
using MineCloudApp.Models;
using MineCloudApp.Utils;
using MineCloudApp.Lang;
using Avalonia.Media;
using System.Reactive;

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

        public IBrush BarColor => new SolidColorBrush(Avalonia.Media.Color.FromArgb(75, 0, 65, 255));

        public ReactiveCommand<Unit, Unit> CloseWindow => ReactiveCommand.Create(delegate { App.Window.Close(); });
        public ReactiveCommand<Unit, Unit> MinimizeWindow => ReactiveCommand.Create(delegate { App.Window.WindowState = Avalonia.Controls.WindowState.Minimized; });

        private int _progressValue = 0;

        public int ProgressValue
        {
            get => _progressValue;
            set => this.RaiseAndSetIfChanged(ref _progressValue, value);
        }

        private bool _progressIndeterminate = false;

        public bool ProgressIndeterminate
        {
            get => _progressIndeterminate;
            set => this.RaiseAndSetIfChanged(ref _progressIndeterminate, value);
        }

        private MineCloudNetwork Network;
        private ProcessHelper ProcessHelper;

        public MainWindowViewModel(MineCloudNetwork Network, ProcessHelper processHelper)
        {
            this.Network = Network;
            this.ProcessHelper = processHelper;

            var savedUser = Network.SavedUser();
            if(savedUser != null)
            {
                User.CurrentUser = savedUser;
                ChangeContent(ViewModels.Main);
            }
            else
            {
                ChangeContent(ViewModels.Login);
            }
        }

        public MainWindowViewModel()
        {

        }

        private async void ConnectUser(LoginModel Model)
        {
            var user = await Network.Login(Model.Pseudo, Model.Password, ((LoginViewModel)Content).RememberMeChecked);
            if(user != null)
            {
                User.CurrentUser = user;
                ChangeContent(ViewModels.Main);
            }
            else
            {
                ((LoginViewModel)Content).SetConnectButton(true);
            }
            ProgressIndeterminate = false;
        }

        private async void SignupUser(SignupModel Model)
        {
            
        }

        private void ChangeContent(ViewModels Model)
        {
            switch(Model)
            {
                case ViewModels.Main:
                    Content = new MainViewModel()
                    {
                        ButtonText = Network.LauncherExists() ? LanguageController.CurrentLanguage.Start : LanguageController.CurrentLanguage.Download,
                        InfoText = Network.LauncherExists() ?
                        LanguageController.CurrentLanguage.Ready : LanguageController.CurrentLanguage.DownloadRequired,
                        ButtonImage = Network.LauncherExists() ? FileHelper.ParseString("avares://MineCloudApp/Assets/Minecraft.png") : FileHelper.ParseString("avares://MineCloudApp/Assets/download.png")
                    };

                    Observable.Merge(((MainViewModel)Content).DownloadButton).Subscribe(delegate
                    {
                        if(!Network.LauncherExists())
                        {
                            ((MainViewModel)Content).InfoText = LanguageController.CurrentLanguage.DownloadingLauncher;
                            Network.DownloadLauncher((value) => ProgressValue = value, ((MainViewModel)Content).FileDownloaded);
                        }
                        else
                        {
                            ProcessHelper.startProcessAndWatch(async (list) =>
                            {
                                ((MainViewModel)Content).InfoText = LanguageController.CurrentLanguage.Uploading;
                                await Network.ProcessSaving(list);
                                ((MainViewModel)Content).InfoText = LanguageController.CurrentLanguage.Ready;
                            });
                        }
                    });
                    Observable.Merge(((MainViewModel)Content).DisconnectButton).Take(1).Subscribe(delegate
                    {
                        User.CurrentUser = null;
                        Network.DeleteSavedUser();
                        ChangeContent(ViewModels.Login);
                    });
                    break;
                case ViewModels.Login:
                    Content = new LoginViewModel();

                    Observable.Merge(((LoginViewModel)Content).ConnectButton, ((LoginViewModel)Content).SignupButton.Select(_ => (LoginModel)null)).Subscribe(model =>
                    {
                        if (model != null)
                        {
                            ((LoginViewModel)Content).SetConnectButton(false);
                            ProgressIndeterminate = true;
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
