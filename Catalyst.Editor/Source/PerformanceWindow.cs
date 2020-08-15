using Catalyst.Editor;
using Catalyst.Engine;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualBasic;
using Catalyst.Engine.Utilities;
using System.Runtime.InteropServices;

namespace CatalystEditor.Source
{
    public static class PerformanceWindow
    {
        public static bool Open = true;

        private static Process _process = Process.GetCurrentProcess();

        private static readonly int samples = 120;
        private static float[] _cpuActual = new float[samples];
        private static float[] _cpuAverage = new float[samples];
        private static float[] _memory = new float[samples];
        private static float _max = 1;
        private static float _processorLast = 0;
        private static int offset = 0;

        private static float _timeSinceLastRefresh = 0;
        private static double _timeSinceLastSampleUpdate = 0;
        private static double _sampleRate = 1d / samples;


        public static void Render()
        {

            if (_timeSinceLastSampleUpdate > _sampleRate)
            {
                _memory[offset] = Convert.ToSingle(_process.PrivateMemorySize64)/1024f/1024f;


                if (_timeSinceLastRefresh > 0.5)
                {
                    _process.Refresh();
                    _timeSinceLastRefresh = 0;
                }
                

                if (_memory[offset] > _max)
                    _max = _memory[offset];

                _cpuActual[offset] = ((float)_process.TotalProcessorTime.TotalMilliseconds - _processorLast) / Environment.ProcessorCount;
                

                float average = 0.0f;
                for (int n = 0; n < _cpuActual.Length; n++)
                    average += _cpuActual[n];
                average /= (float)_cpuActual.Length;

                _cpuAverage[offset] = average;


                _processorLast = (float)_process.TotalProcessorTime.TotalMilliseconds;

                
            }

            ImGui.PushFont(ImGuiLayout.SubHeadingFont);

            ImGui.Text("Cpu Usage");
            string overlay2 = "";
            ImGui.PlotLines("##Cpu rate line plot", ref _cpuAverage[0], _cpuAverage.Length, offset, overlay2, 0.0f, 100.0f, new System.Numerics.Vector2(0, 80.0f));

            ImGui.Text("Memory Usage");
            string overlay3 = "";
            ImGui.PlotLines("##memory usage line plot", ref _memory[0], _memory.Length, offset, overlay3, 0.0f, _max + 10, new System.Numerics.Vector2(0, 80.0f));
            ImGui.PopFont();

            if (_timeSinceLastSampleUpdate > _sampleRate)
            {
                offset = (offset + 1) % samples;
                _timeSinceLastSampleUpdate = 0;
            }
            _timeSinceLastSampleUpdate += Time.RawDeltaTime;
            _timeSinceLastRefresh += Time.RawDeltaTimeF;
        }
    }
}
