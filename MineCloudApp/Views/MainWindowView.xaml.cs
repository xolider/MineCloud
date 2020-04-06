using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Avalonia.Input;

namespace MineCloudApp.Views
{
    public class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Grid grid = this.FindControl<Grid>("TopBar");
            int xOffset = 0;
            int yOffset = 0;
            grid.PointerPressed += new System.EventHandler<PointerPressedEventArgs>((sender, args) =>
            {
                xOffset = (int) args.GetCurrentPoint(null).Position.X;
                yOffset = (int)args.GetCurrentPoint(null).Position.Y;
            });
            grid.PointerMoved += new System.EventHandler<PointerEventArgs>((sender, args) =>
            {
                if (args.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
                {
                    var newPos = new PixelPoint((int)(this.Position.X + args.GetCurrentPoint(null).Position.X - xOffset), (int)(this.Position.Y + args.GetCurrentPoint(null).Position.Y - yOffset));
                    this.Position = newPos;
                }
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
