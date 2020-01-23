using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.XNA
{
    public static class Crunch
    {

        [DllImport("crunch.dll", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern int Start(int argc , [MarshalAs(UnmanagedType.LPArray)]string[] argv);

        public static int Start(int argc, string output, string input, params string[] options)
        {
            List<string> arr = new List<string>();
            arr.Add(output);
            arr.Add(input);
            foreach(string s in options)
            {
                arr.Add(s);
            }
            string[] p = arr.ToArray();
            return Start(argc, p);
        }
    }
}
