using ReactiveUI;
using System.Reactive;
using MineCloudApp.Models;

namespace MineCloudApp.ViewModels
{
    public class SignupViewModel : ViewModelBase
    {
        private string _email;
        private string _pseudo;
        private string _password;
        private string _passwordConfirm;

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

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

        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set => this.RaiseAndSetIfChanged(ref _passwordConfirm, value);
        }

        public ReactiveCommand<Unit, Unit> ConnectButton { get; }
        public ReactiveCommand<Unit, SignupModel> SignupButton { get; }

        public SignupViewModel()
        {
            ConnectButton = ReactiveCommand.Create(() => { });

            var signupEnabled = this.WhenAnyValue(x => x.Email, x => x.Pseudo, x => x.Password, x => x.PasswordConfirm, (email, pseudo, password, passwordConfirm) => !string.IsNullOrWhiteSpace(email)
                && !string.IsNullOrWhiteSpace(pseudo) && !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(passwordConfirm) && password.Equals(passwordConfirm) && this.matchEmail(email));

            SignupButton = ReactiveCommand.Create(() => new SignupModel { Pseudo = Pseudo, Email = Email, Password = Password }, signupEnabled);
        }

        private bool matchEmail(string email)
        {
            bool containsAt = email.Contains("@");
            bool correctForm = false;
            int lastDot = email.LastIndexOf(".");
            if (lastDot != -1)
            {
                if (email.Substring(lastDot).Length == 3 || email.Substring(lastDot).Length == 4)
                {
                    correctForm = true;
                }
            }
            return containsAt && correctForm;
        }
    }
}
