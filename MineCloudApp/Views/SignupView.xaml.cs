using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MineCloudApp.Views
{
    public class SignupView : UserControl
    {
        public SignupView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
