using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace CatalystEditor
{
    public class AssemblyContextLoader: AssemblyLoadContext
    {

        public AssemblyContextLoader() : base(true)
        {

        }

        protected override Assembly Load(AssemblyName name)
        {
            return null;
        }
    }
}
