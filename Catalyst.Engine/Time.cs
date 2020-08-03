using Newtonsoft.Json.Schema;
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
        public static double RawDeltaTime
        {
            get
            {
                return _rawDeltaTime;
            }
            internal set
            {
                _rawDeltaTime = value;
                RawDeltaTimeF = (float)value;
            }
        }
        private static double _rawDeltaTime;
        public static float RawDeltaTimeF { get; private set; }

        public static double DeltaTime
        {
            get
            {
                return _DeltaTime;
            }
            internal set
            {
                _DeltaTime = value * TimeRate;
                DeltaTimeF = (float)value;
            }
        }
        private static double _DeltaTime;
        public static float DeltaTimeF { get; private set; }

        public static double TimeRate
        {
            get
            {
                return _TimeRate;
            }
            internal set
            {
                _TimeRate = value;
                TimeRateF = (float)value;
            }
        }
        private static double _TimeRate = 1.0;
        public static float TimeRateF { get; private set; } = 1.0f;

    }
}
