using System;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Validus.Console.UiTests.Helper
{
    public static class ScreenCapture
    {
        public static Bitmap GetWindowCaptureAsBitmap(IntPtr handle)
        {
            IntPtr hWnd = handle;// new IntPtr(handle);
            Win32.Rect rc = new Win32.Rect();
            if (!Win32.GetWindowRect(hWnd, ref rc))
                return null;

            //			Win32.WindowInfo wi = new Win32.WindowInfo();
            //			wi.size = Marshal.SizeOf(wi);
            //			if (!Win32.GetWindowInfo(hWnd, ref wi))
            //				return null;

            // create a bitmap from the visible clipping bounds of the graphics object from the window
            Bitmap bitmap = new Bitmap(rc.Width, rc.Height);

            // create a graphics object from the bitmap
            Graphics gfxBitmap = Graphics.FromImage(bitmap);

            // get a device context for the bitmap
            IntPtr hdcBitmap = gfxBitmap.GetHdc();

            // get a device context for the window
            IntPtr hdcWindow = Win32.GetWindowDC(hWnd);

            // bitblt the window to the bitmap
            Win32.BitBlt(hdcBitmap, 0, 0, rc.Width, rc.Height, hdcWindow, 0, 0, (int)Win32.TernaryRasterOperations.SRCCOPY);

            // release the bitmap's device context
            gfxBitmap.ReleaseHdc(hdcBitmap);

            Win32.ReleaseDC(hWnd, hdcWindow);

            // dispose of the bitmap's graphics object
            gfxBitmap.Dispose();

            // return the bitmap of the window
            return bitmap;
        }
    }
}
