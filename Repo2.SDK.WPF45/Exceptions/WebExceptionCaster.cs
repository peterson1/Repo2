using System;
using System.Net;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.SDK.WPF45.Exceptions
{
    public static class WebExceptionCaster
    {
        public static WebException FromUrl<T>(this T ex, string url) where T : Exception
        {
            var agEx = ex as AggregateException;
            if (agEx == null) return AppendURL(ex, url);
            return AppendURL(agEx.InnerException ?? agEx, url);
        }


        private static WebException AppendURL<T>(T ex, string url) where T: Exception
        {
            string msg;
            WebException ret;

            var webEx = ex as WebException;
            if (webEx != null)
            {
                //msg = $"“{webEx.Message}”{L.f}[{webEx.Status}] {url}";
                var rs = webEx.Response as HttpWebResponse;
                msg = $"‹{webEx.Status}› {rs.StatusDescription}{L.f}“{webEx.Message}”{L.f}[{rs.Method}] {url}";

                ret = new WebException(msg, ex, webEx.Status, null);
            }
            else
            {
                msg = $"“{ex.Message}”{L.f}‹{ex.GetType().Name}› {url}";
                ret = new WebException(msg, ex);
            }

            Console.WriteLine(msg);
            return ret;
        }
    }
}
