using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Catalyst.Controller;
using Catalyst.ViewModels;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive;
using System.Reactive.Disposables;

namespace Catalyst
{
    public class NewProjectWizard : ReactiveWindow<NewProjectWizardViewModel>
    {
        public static string NewFileName { get; set; }

        public NewProjectWizard()
        {

            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
            this.FindControl<TextBox>("ProjectName").GetObservable(TextBox.TextProperty).Subscribe(text => {
                UpdateDirectory(text);
            });
        }

        public void UpdateDirectory(string name)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                if (name is null)
                {
                    name = "";
                }
                name = name.Replace(c.ToString(), "");
            }

            NewFileName = name;
            this.FindControl<TextBlock>("DirectoryText").Text = ProjectManager.WorkSpacePath + "\\" + name;

            if (File.Exists(ProjectManager.WorkSpacePath + "\\" + name + "\\*" + ProjectManager.Extention))
            {
                this.FindControl<Button>("Next").IsEnabled = false;
            }
            else
            {
                this.FindControl<Button>("Next").IsEnabled = true;
            }
        }
    }
}
