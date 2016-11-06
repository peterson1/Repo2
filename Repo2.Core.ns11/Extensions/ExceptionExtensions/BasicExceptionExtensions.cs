using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.Extensions.ExceptionExtensions
{
    public static class BasicExceptionExtensions
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

                msg += L.f + bullet + " " + inr.Message;
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

            var ss = stackTrace.Split('\n');

            for (int i = 0; i < ss.Length; i++)
            {
                var dropTxt = ss[i].Between(" in ", "\\", true);
                if (dropTxt != ss[i])
                    ss[i] = ss[i].Replace(dropTxt, "");
            }

            return string.Join("\n", ss);
        }
    }
}
