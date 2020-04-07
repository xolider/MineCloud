namespace MineCloudApp.Lang
{
    public interface ILanguage
    {
        public string Start { get; }
        public string Download { get; }
        public string Username { get; }
        public string Password { get; }
        public string Email { get; }
        public string PasswordConfirm { get; }
        public string Ready { get; }
        public string DownloadRequired { get; }
        public string Signup { get; }
        public string Signin { get; }
        public string EnterUsername { get; }
        public string EnterPassword { get; }
        public string EnterEmail { get; }
        public string EnterPasswordConfirm { get; }
        public string DownloadingLauncher { get; }
        public string Hello { get; }
        public string RememberMe { get; }
        public string Uploading { get; }
    }
}
