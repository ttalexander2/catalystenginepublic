using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CatalystEditor.Source
{
    public static class WindowHandler
    {


        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_Rect
        {
            public int x;
            public int y;
            public int w;
            public int h;
        }

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MaximizeWindow(IntPtr window);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MinimizeWindow(IntPtr window);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RestoreWindow(IntPtr window);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetWindowFlags(IntPtr window);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowPosition(IntPtr window, int x, int y);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowSize(IntPtr window, int x, int y);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetWindowDisplayIndex(IntPtr window);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int SDL_GetDisplayUsableBounds(int displayIndex, SDL_Rect* rect);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint SDL_GetGlobalMouseState(int* x, int* y);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint SDL_GetMouseState(int* x, int* y);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RaiseWindow(IntPtr window);


        public static bool WindowMaximized(IntPtr window)
        {
            uint flags = SDL_GetWindowFlags(window);
            return (flags & 128) != 0;
        }
    }
}
