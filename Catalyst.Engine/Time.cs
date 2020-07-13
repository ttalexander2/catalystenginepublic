using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Catalyst.Editor")]
namespace Catalyst.Engine
{

    public static class Time
    {
        public static float RawDeltaTime { get; internal set; }
        public static float DeltaTime { get; internal set; }
        public static float TimeRate = 1.0f;
    }
}
