using Avalonia;
using Avalonia.Markup.Xaml;

namespace Catalyst
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
