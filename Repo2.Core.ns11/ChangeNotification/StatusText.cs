using System;

namespace Repo2.Core.ns11.ChangeNotification
{
    public class StatusText
    {
        public StatusText(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }


    public static class StatusTextExtensions
    {
        public static void Raise(this EventHandler<StatusText> handlr, string text)
        {
            handlr?.Invoke(handlr.Target, new StatusText(text));
        }
    }
}