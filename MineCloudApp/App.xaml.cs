using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MineCloudApp.ViewModels;
using MineCloudApp.Views;
using MineCloudApp.Utils;
using Avalonia.Controls;

namespace MineCloudApp
{
    public class App : Application
    {
        private MineCloudNetwork MineCloudNetwork;

        private ProcessHelper ProcessHelper;

        private FileHelper FileHelper;

        public static Window Window { get; private set; }

        public App()
        {
            FileHelper = new FileHelper();
            MineCloudNetwork = new MineCloudNetwork(FileHelper);
            ProcessHelper = new ProcessHelper(FileHelper);
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindowView
                {
                    DataContext = new MainWindowViewModel(MineCloudNetwork, ProcessHelper)
                };
                Window = desktop.MainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
