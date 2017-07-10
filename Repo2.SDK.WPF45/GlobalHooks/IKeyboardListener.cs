using System;
using System.Windows.Input;

namespace Repo2.SDK.WPF45.GlobalHooks
{
    public interface IKeyboardListener : IDisposable
    {
        event EventHandler<Key> KeyPressed;
        event EventHandler<Key> CtrlAltKeyPressed;

        void  StartListening ();
        void  StopListening  ();
    }
}
