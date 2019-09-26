using Avalonia;
using Avalonia.Controls;

namespace Catalyst.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public void CreateNewProject()
        {
            Window w = new NewProjectWizard();
            _ = w.ShowDialog(Application.Current.MainWindow);

        }
    }
}
