using System;
using System.Runtime.InteropServices;

namespace AWPClient.Classes
{
    public class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public const int SWP_SHOWWINDOW = 0x40;
        public const int SWP_NOZORDER = 0x4;

        public static void PositionWindowToCoverScreen(IntPtr windowHandle)
        {
            var desktopHandle = GetDesktopWindow();
            var monitorHandle = MonitorFromWindow(windowHandle, 0x00000002); // MONITOR_DEFAULTTONEAREST
            MONITORINFO monitorInfo = new MONITORINFO();
            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
            GetMonitorInfo(monitorHandle, ref monitorInfo);

            SetWindowPos(windowHandle, IntPtr.Zero,
                monitorInfo.rcMonitor.Left, monitorInfo.rcMonitor.Top,
                monitorInfo.rcMonitor.Right - monitorInfo.rcMonitor.Left,
                monitorInfo.rcMonitor.Bottom - monitorInfo.rcMonitor.Top,
                SWP_SHOWWINDOW | SWP_NOZORDER);
        }
    }
}
