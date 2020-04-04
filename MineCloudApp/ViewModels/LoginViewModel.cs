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

        public ReactiveCommand<Unit, LoginModel> ConnectButton { get; }

        public ReactiveCommand<Unit, Unit> SignupButton { get; }

        public LoginViewModel()
        {
            var ConnectEnabled = this.WhenAnyValue(x => x.Pseudo, x => x.Password, (login, pass) => !string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(pass));

            ConnectButton = ReactiveCommand.Create(() => new LoginModel { Pseudo = Pseudo, Password = Password }, ConnectEnabled);
            SignupButton = ReactiveCommand.Create(() => {  });
        }
    }
}
