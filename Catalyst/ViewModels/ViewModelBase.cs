using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalyst.ViewModels
{
    public class ViewModelBase : ReactiveObject, ISupportsActivation
    {
        public ViewModelActivator Activator { get; }
    }
}
