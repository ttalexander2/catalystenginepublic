using Avalonia.Controls;
using Avalonia.Diagnostics.ViewModels;
using Catalyst.Controller;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reactive;
using System.Text;

namespace Catalyst.ViewModels
{ 
    public class NewProjectWizardViewModel : ViewModelBase
    {

        public NewProjectWizardViewModel()
        {
        }

        public void CreateProject()
        {
            Console.WriteLine("TEST");
            //ProjectManager.CreateNewProject(NewProjectWizard.NewFileName);
        }

    }
}
