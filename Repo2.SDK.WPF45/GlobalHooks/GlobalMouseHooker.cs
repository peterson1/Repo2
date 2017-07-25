using Repo2.Core.ns11.ChangeNotification;
using Repo2.SDK.WPF45.ChangeNotification;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Repo2.SDK.WPF45.GlobalHooks
{
    public class GlobalMouseHooker : StatusChangerN45, IMouseListener, IDisposable
    {
        private      EventHandler _leftClicked;
        public event EventHandler  LeftClicked
        {
            add    { _leftClicked -= value; _leftClicked += value; }
            remove { _leftClicked -= value; }
        }

        private      EventHandler _rightClicked;
        public event EventHandler  RightClicked
        {
            add    { _rightClicked -= value; _rightClicked += value; }
            remove { _rightClicked -= value; }
        }


        private const int WH_MOUSE_LL = 14;


        private LowLevelMouseProc _proc;
        private IntPtr            _hookID = IntPtr.Zero;


        public GlobalMouseHooker()
        {
            _proc = HookCallback;
        }


        public void StartListening()
        {
            _hookID = SetHook(_proc);
            SetStatus("Hook set.");
        }


        public void StopListening()
        {
            UnhookWindowsHookEx(_hookID);
            SetStatus("Hook unset.");
        }


        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);


        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            //{
            //    MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            //    Console.WriteLine(hookStruct.pt.x + ", " + hookStruct.pt.y);
            //}

            if (MessageEquals(MouseMessages.WM_LBUTTONUP, nCode, wParam))
                _leftClicked?.Raise();

            if (MessageEquals(MouseMessages.WM_RBUTTONUP, nCode, wParam))
                _rightClicked?.Raise();

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }


        private bool MessageEquals(MouseMessages mouseMsg, int nCode, IntPtr wParam)
        {
            if (nCode < 0) return false;
            var mParam = (MouseMessages)wParam;
            return mParam == mouseMsg;
        }

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;

        }



        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StopListening();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
