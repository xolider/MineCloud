using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MineCloudApp.Views
{
    public class LoginView : UserControl
    {
        public LoginView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
