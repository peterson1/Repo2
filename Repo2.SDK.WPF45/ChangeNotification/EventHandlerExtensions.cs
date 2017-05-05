using System;

namespace Repo2.SDK.WPF45.ChangeNotification
{
    public static class EventHandlerExtensions
    {
        public static void TryRemoveFrom<T>(this EventHandler<T> newHandler, ref EventHandler<T> origHandlr)
        {
            if (origHandlr == null) return;
            var newKey = GetKey(newHandler);

            foreach (var @delegate in origHandlr.GetInvocationList())
            {
                var oldHandlr = @delegate as EventHandler<T>;
                if (GetKey(oldHandlr) == newKey)
                {
                    origHandlr -= oldHandlr;
                    return;
                }
            }
        }


        private static string GetKey<T>(EventHandler<T> handlr)
        {
            var typ = handlr.Method.DeclaringType.FullName;
            return $"{typ}.{handlr.Method.Name}";
        }
    }
}
