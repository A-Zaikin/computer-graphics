using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace Exercise4
{
    public static class Program
    {
        private static Stopwatch timer = new();
        private static Window window;

        public static float Time => timer.ElapsedMilliseconds / 1000f;

        private static void Main()
        {
            timer.Start();

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(729, 729),
                Title = "Exercise 4",
                Flags = ContextFlags.ForwardCompatible,
                NumberOfSamples = 4,
                WindowState = WindowState.Fullscreen,
            };
            window = new Window(GameWindowSettings.Default, nativeWindowSettings);
            window.VSync = VSyncMode.On;
            window.Run();
            window.Dispose();
        }
    }
}
