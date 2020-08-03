using Catalyst.Editor;
using Catalyst.Engine;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CatalystEditor.Source
{
    public static class PerformanceWindow
    {
        public static bool Open;

        private static Process _process = Process.GetCurrentProcess();

        private static readonly int samples = 120;
        private static float[] _frames = new float[samples];
        private static float[] _cpuActual = new float[samples];
        private static float[] _cpuAverage = new float[samples];
        private static float _processorLast = 0;
        private static int offset = 0;


        public static void Render()
        {
            _frames[offset] = 1 / Time.RawDeltaTimeF;

            _cpuActual[offset] = ((float)_process.TotalProcessorTime.TotalMilliseconds - _processorLast) / Time.DeltaTimeF / Environment.ProcessorCount;
            ImGui.PushFont(ImGuiLayout.SubHeadingFont);
            ImGui.Text("Frame Rate");

            string overlay = "";
            ImGui.PlotLines("##Frame rate line plot", ref _frames[0], _frames.Length, offset, overlay, -1.0f, 1.0f, new System.Numerics.Vector2(0, 80.0f));

            float average = 0.0f;
            for (int n = 0; n < _cpuActual.Length; n++)
                average += _cpuActual[n];
            average /= (float)_cpuActual.Length;

            _cpuAverage[offset] = average;
            ImGui.Text("Cpu Usage");
            string overlay2 = "";
            ImGui.PlotLines("##Cpu rate line plot", ref _cpuAverage[0], _cpuAverage.Length, offset, overlay2, 0.0f, 100.0f, new System.Numerics.Vector2(0, 80.0f));

            offset = (offset + 1) % samples;

            _processorLast = (float)_process.TotalProcessorTime.TotalMilliseconds;

            ImGui.PopFont();
        }
    }
}
