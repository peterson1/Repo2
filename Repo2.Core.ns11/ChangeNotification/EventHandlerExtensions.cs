using System;

namespace Repo2.Core.ns11.ChangeNotification
{
    public static class EventHandlerExtensions
    {
        public static void Raise(this EventHandler handlr, object sender = null)
            => handlr?.Invoke(sender, EventArgs.Empty);


        public static void Raise<T>(this EventHandler<T> handlr, T parameter, object sender = null)
            => handlr?.Invoke(sender, parameter);
    }
}
