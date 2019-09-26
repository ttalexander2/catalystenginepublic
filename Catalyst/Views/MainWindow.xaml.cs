using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Catalyst.ViewModels;

namespace Catalyst.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


    }
}
