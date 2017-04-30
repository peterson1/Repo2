using Repo2.Core.ns11.ChangeNotification;
using System;

namespace Repo2.SDK.WPF45.ChangeNotification
{
    public class StatusChangerN45 : StatusChanger
    {
        protected override void TryRemoveExisting(EventHandler<string> newHandler)
        {
            if (_statusChanged == null) return;
            var newKey = GetKey(newHandler);
            foreach (var @delegate in _statusChanged.GetInvocationList())
            {
                var oldHandlr = @delegate as EventHandler<string>;
                if (GetKey(oldHandlr) == newKey)
                {
                    _statusChanged -= oldHandlr;
                    return;
                }
            }
        }


        private string GetKey(EventHandler<string> handlr)
        {
            var typ = handlr.Method.DeclaringType.FullName;
            return $"{typ}.{handlr.Method.Name}";
        }
    }
}
