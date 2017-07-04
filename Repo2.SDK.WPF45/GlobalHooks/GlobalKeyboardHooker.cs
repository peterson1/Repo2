using Repo2.Core.ns11.ChangeNotification;
using Repo2.SDK.WPF45.ChangeNotification;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Repo2.SDK.WPF45.GlobalHooks
{
    //from: https://stackoverflow.com/a/604417/3973863
    public class GlobalKeyboardHooker : StatusChangerN45, IKeyboardListener
    {
        private      EventHandler<Key> _keyPressed;
        public event EventHandler<Key>  KeyPressed
        {
            add    { _keyPressed -= value; _keyPressed += value; }
            remove { _keyPressed -= value; }
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN     = 0x0100;

        private LowLevelKeyboardProc _proc;
        private IntPtr               _hookID = IntPtr.Zero;


        public GlobalKeyboardHooker()
        {
            _proc = HookCallback;
        }


        public void StartListening () => SetHook();
        public void StopListening  () => UnsetHook();


        public void SetHook()
        {
            _hookID = SetHook(_proc);
            SetStatus("Hook set.");
        }


        public void UnsetHook()
        {
            UnhookWindowsHookEx(_hookID);
            SetStatus("Hook unset.");
        }




        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                var key    = KeyInterop.KeyFromVirtualKey(vkCode);
                //SetStatus($"{key}");
                _keyPressed?.Raise(key);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    UnsetHook();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
