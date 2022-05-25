using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace SearchBar.UI.Handles
{
    public enum ABEdge : int
    {
        Left,
        Top,
        Right,
        Bottom,
        None
    }
    public static class AppBarHandler
    {
        private static IntPtr _HWnd;

        public static void SetAppBar(Window appbarWindow, ABEdge edge)
        {
            RegisterInfo info = GetRegisterInfo(appbarWindow);
            info.Edge = edge;

            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = new WindowInteropHelper(appbarWindow).EnsureHandle();

            if (edge == ABEdge.None)
            {
                if (info.IsRegistered)
                {
                    SHAppBarMessage((int)ABMsg.ABM_REMOVE, ref abd);
                    info.IsRegistered = false;
                }
                RestoreWindow(appbarWindow);
                return;
            }

            if (!info.IsRegistered)
            {
                info.IsRegistered = true;
                info.CallbackId = RegisterWindowMessage("AppBarMessage");
                abd.uCallbackMessage = info.CallbackId;

                uint ret = SHAppBarMessage((int)ABMsg.ABM_NEW, ref abd);

                HwndSource source = HwndSource.FromHwnd(abd.hWnd);
                source.AddHook(new HwndSourceHook(info.WndProc));
            }

            appbarWindow.WindowStyle = WindowStyle.None;
            appbarWindow.ResizeMode = ResizeMode.NoResize;
            appbarWindow.Topmost = true;

            ABSetPos(info.Edge, appbarWindow);
        }

        public static RegisterInfo GetRegisterInfo(Window appbarWindow)
        {
            RegisterInfo reg;
            if (s_RegisteredWindowInfo.ContainsKey(appbarWindow))
            {
                reg = s_RegisteredWindowInfo[appbarWindow];
            }
            else
            {
                reg = new RegisterInfo()
                {
                    CallbackId = 0,
                    Window = appbarWindow,
                    IsRegistered = false,
                    Edge = ABEdge.Top,
                    OriginalStyle = appbarWindow.WindowStyle,
                    OriginalPosition = new Point(appbarWindow.Left, appbarWindow.Top),
                    OriginalSize =
                            new Size(appbarWindow.ActualWidth, appbarWindow.ActualHeight),
                    OriginalResizeMode = appbarWindow.ResizeMode,
                };
                s_RegisteredWindowInfo.Add(appbarWindow, reg);
            }
            return reg;
        }

        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_SHOWWINDOW = 0x0040;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        private enum ABMsg : int
        {
            ABM_NEW = 0,
            ABM_REMOVE,
            ABM_QUERYPOS,
            ABM_SETPOS,
            ABM_GETSTATE,
            ABM_GETTASKBARPOS,
            ABM_ACTIVATE,
            ABM_GETAUTOHIDEBAR,
            ABM_SETAUTOHIDEBAR,
            ABM_WINDOWPOSCHANGED,
            ABM_SETSTATE
        }
        private enum ABNotify : int
        {
            ABN_STATECHANGE = 0,
            ABN_POSCHANGED,
            ABN_FULLSCREENAPP,
            ABN_WINDOWARRANGE,

        }
        private enum WM_Msg
        {
            WM_ACTIVATE = 6,
            WM_WINDOWPOSCHANGED = 71
        }

        [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
        private static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int RegisterWindowMessage(string msg);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos([In] IntPtr hWnd, [In] IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);

        public class RegisterInfo
        {
            public int CallbackId { get; set; }
            public bool IsRegistered { get; set; }
            public Window Window { get; set; }
            public ABEdge Edge { get; set; }
            public WindowStyle OriginalStyle { get; set; }
            public Point OriginalPosition { get; set; }
            public Size OriginalSize { get; set; }
            public ResizeMode OriginalResizeMode { get; set; }

            public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam,
                                    IntPtr lParam, ref bool handled)
            {
                if (msg == CallbackId)
                {
                    int wIntParam = wParam.ToInt32();
                    switch (wIntParam)
                    {
                        case (int)ABNotify.ABN_POSCHANGED:
                            ABSetPos(Edge, Window);
                            AppBarActivate();
                            handled = true;
                            break;
                        case (int)ABNotify.ABN_FULLSCREENAPP:
                            if ((int)lParam == 0)
                            {
                                AppBarActivate();
                            }
                            else
                            {
                                SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
                                SetWindowPos(hwnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (msg)
                    {
                        case (int)WM_Msg.WM_ACTIVATE:
                            AppBarActivate();
                            break;
                        case (int)WM_Msg.WM_WINDOWPOSCHANGED:
                            AppbarWindowPosChanged();
                            break;
                    }

                }
                return IntPtr.Zero;
            }

        }
        private static readonly Dictionary<Window, RegisterInfo> s_RegisteredWindowInfo
            = new Dictionary<Window, RegisterInfo>();

        private static void RestoreWindow(Window appbarWindow)
        {
            RegisterInfo info = GetRegisterInfo(appbarWindow);

            appbarWindow.WindowStyle = info.OriginalStyle;
            appbarWindow.ResizeMode = info.OriginalResizeMode;
            appbarWindow.Topmost = false;

            Rect rect = new Rect(info.OriginalPosition.X, info.OriginalPosition.Y,
                info.OriginalSize.Width, info.OriginalSize.Height);
            appbarWindow.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                    new ResizeDelegate(DoResize), appbarWindow, rect);

        }

        private delegate void ResizeDelegate(Window appbarWindow, Rect rect);
        private static void DoResize(Window appbarWindow, Rect rect)
        {

            appbarWindow.Width = rect.Width;
            appbarWindow.Height = rect.Height;
            appbarWindow.Top = rect.Top;
            appbarWindow.Left = rect.Left;
        }

        private static void ABSetPos(ABEdge edge, Window appbarWindow)
        {
            APPBARDATA barData = new APPBARDATA();
            barData.cbSize = Marshal.SizeOf(barData);
            _HWnd = barData.hWnd = new WindowInteropHelper(appbarWindow).Handle;
            barData.uEdge = (int)edge;

            barData.rc.left = 0;
            barData.rc.right = (int)SystemParameters.PrimaryScreenWidth;

            SHAppBarMessage((int)ABMsg.ABM_QUERYPOS, ref barData);

            if (barData.uEdge == (int)ABEdge.Top)
            {
                barData.rc.bottom = barData.rc.top + ((MainWindow)appbarWindow).GetAppBarDesiredheight() * GetCurrentSreenScaleFactor() / 100;
            }

            SHAppBarMessage((int)ABMsg.ABM_SETPOS, ref barData);

            Rect rect = new Rect((double)barData.rc.left, (double)barData.rc.top,
                (double)(barData.rc.right - barData.rc.left), (double)(barData.rc.bottom - barData.rc.top));
            //This is done async, because WPF will send a resize after a new appbar is added.  
            //if we size right away, WPFs resize comes last and overrides us.
            appbarWindow.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                new ResizeDelegate(DoResize), appbarWindow, rect);
        }

        public static double ScreenWidth()
             => SystemParameters.PrimaryScreenWidth;

        public static void AppBarActivate()
        {
            if (_HWnd != null)
            {
                APPBARDATA pData = new APPBARDATA();
                pData.cbSize = Marshal.SizeOf(pData);
                pData.hWnd = _HWnd;
                SHAppBarMessage(6, ref pData);
                SetWindowPos(_HWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
                SetWindowPos(_HWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
            }
        }

        private static void AppbarWindowPosChanged()
        {
            if (_HWnd != null)
            {
                APPBARDATA pData = new APPBARDATA();
                pData.cbSize = Marshal.SizeOf(pData);
                pData.hWnd = _HWnd;
                SHAppBarMessage(9, ref pData);
            }
        }

        #region Window styles
        [Flags]
        public enum ExtendedWindowStyles
        {
            WS_EX_TOOLWINDOW = 0x00000080,
        }

        public enum GetWindowLongFields
        {
            GWL_EXSTYLE = (-20),
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            IntPtr result;
            int error;
            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        #endregion

        public static void HideAppbarFromWindows()
        {
            int exStyle = (int)GetWindowLong(_HWnd, (int)GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowLong(_HWnd, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
        }

        public static int GetCurrentSreenScaleFactor()
        {
            //var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

            //var dpiX = (int)dpiXProperty.GetValue(null, null);
            var dpiY = (int)dpiYProperty.GetValue(null, null);

            return dpiY * 100 / 96;
        }
    }
}
