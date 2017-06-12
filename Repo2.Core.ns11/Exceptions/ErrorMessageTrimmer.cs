using Repo2.Core.ns11.Extensions.StringExtensions;
using System;
using System.Linq;

namespace Repo2.Core.ns11.Exceptions
{
    public static class ErrorMessageTrimmer
    {
        public static string Info(this Exception ex, bool withTypeNames = false, bool withShortStackTrace = false)
        {
            var msg = ex.Message;
            var typ = $"‹{ex.GetType().Name}›";

            var inr = ex; var bullet = "";
            while (inr.InnerException != null)
            {
                inr = inr.InnerException;
                bullet += ".";

                msg += L.F + bullet + " " + inr.Message;
                typ += L.f + bullet + " " + inr.GetType().Name;
            }

            if (withTypeNames) msg += L.f + typ;

            if (withShortStackTrace)
                msg += L.f + ex.ShortStackTrace();

            return msg;
        }


        private static string ShortStackTrace(this Exception ex)
        {
            try
            {
                return TrimPaths(ex.StackTrace);
            }
            catch (Exception)
            {
                return ex.StackTrace;
            }
        }


        private static string ShortStackTrace(this AggregateException ex)
        {
            return string.Join(L.f,
                ex.InnerExceptions.Select(x => x.ShortStackTrace()));
        }


        private static string TrimPaths(string stackTrace)
        {
            if (stackTrace == null) return "";
            if (stackTrace.IsBlank()) return "";

            var ss = stackTrace.Split('\n').ToList();

            ss.RemoveAll(x => x.HasText("--- End of stack trace from previous location where exception was thrown ---"));
            ss.RemoveAll(x => x.HasText("at Repo2.SDK.WPF45.InputCommands.R2AsyncCommandWPF.<SafeRun>d__48.MoveNext()"));
            ss.RemoveAll(x => x.HasText("at System.Collections.Generic.Dictionary`2.Insert(TKey key, TValue value, Boolean add)"));
            ss.RemoveAll(x => x.HasText("at System.Collections.Generic.Dictionary`2.Add(TKey key, TValue value)"));
            ss.RemoveAll(x => x.HasText("at System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()"));
            ss.RemoveAll(x => x.HasText("at System.Runtime.CompilerServices.TaskAwaiter.GetResult()"));
            ss.RemoveAll(x => x.HasText("at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)"));
            ss.RemoveAll(x => x.HasText("at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)"));
            ss.RemoveAll(x => x.HasText("at System.ThrowHelper.ThrowArgumentException(ExceptionResource resource)"));

            for (int i = 0; i < ss.Count; i++)
            {
                var dropTxt = ss[i].Between(" in ", "\\", true);
                if (dropTxt != ss[i])
                    ss[i] = ss[i].Replace(dropTxt, "");

                ss[i] = ss[i].Replace(".MoveNext() in ", "");
            }

            return string.Join("\n", ss);
        }
    }
}
