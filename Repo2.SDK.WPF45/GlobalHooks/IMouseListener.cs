using System;

namespace Repo2.SDK.WPF45.GlobalHooks
{
    public interface IMouseListener
    {
        event EventHandler LeftClicked;
        event EventHandler RightClicked;

        void  StartListening  ();
        void  StopListening   ();
    }
}
