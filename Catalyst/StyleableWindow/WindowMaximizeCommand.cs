using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Catalyst.StyleableWindow
{
    public class WindowMaximizeCommand :ICommand
    {
        private static Action EmptyDelegate = delegate () { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;


        public void Execute(object parameter)
        {
            var window = parameter as Window;

            if (window != null)
            {
                if(window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal;
                    //((TextBlock)window.FindName("MaxIcon")).Text = "&#xE922;";
                    
                }
                else
                {
                    window.WindowState = WindowState.Maximized;
                    //((TextBlock)window.FindName("MaxIcon")).Text = "&#xE923;";
                }                
            }
        }
    }
}
