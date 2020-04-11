using ReactiveUI;
using System.Reactive;
using MineCloudApp.Models;

namespace MineCloudApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _pseudo;
        private string _password;

        public string Pseudo
        {
            get => _pseudo;
            set => this.RaiseAndSetIfChanged(ref _pseudo, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public bool RememberMeChecked { get; set; } = false;

        public ReactiveCommand<Unit, LoginModel> ConnectButton { get; private set; }

        public ReactiveCommand<Unit, Unit> SignupButton { get; }

        public LoginViewModel()
        {
            SetConnectButton(true);
            SignupButton = ReactiveCommand.Create(() => {  });
        }

        public void SetConnectButton(bool enabled)
        {
            if(enabled)
            {
                var ConnectEnabled = this.WhenAnyValue(x => x.Pseudo, x => x.Password, (login, pass) => !string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(pass));

                ConnectButton = ReactiveCommand.Create(() => new LoginModel { Pseudo = Pseudo, Password = Password }, ConnectEnabled);
            }
            else
            {
                ConnectButton = null;
            }
        }
    }
}
