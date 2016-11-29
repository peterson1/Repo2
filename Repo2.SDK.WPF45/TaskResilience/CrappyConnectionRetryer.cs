using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Serialization;

namespace Repo2.SDK.WPF45.TaskResilience
{
    public class CrappyConnectionRetryer
    {
        public Action<Exception, TimeSpan>  OnRetry  { get; set; } = delegate { };
        public Func<string, string> MakeAbsolute { get; set; }


        public async Task<T> Forever<T>(string resourceUrl, Func<string, CancellationToken, Task<string>> task, CancellationToken cancelTkn)
        {
            var json = string.Empty;
            var url = MakeAbsolute(resourceUrl);

            try
            {
                json = await KeepTrying(ct => task(url, ct), cancelTkn);
            }
            catch (Exception ex) { throw ex.FromUrl(url); }

            return Json.DeserializeOrDefault<T>(json);
        }


        private Task<T> KeepTrying <T>(Func<CancellationToken, Task<T>> task, CancellationToken cancelTkn)
        {
            var policy = Policy.Handle<AggregateException>(ex => IsRetryable(ex))
                               .WaitAndRetryForeverAsync(attempts
                                    => Delay(attempts), (ex, span) => OnRetry(ex, span));

            return policy.ExecuteAsync(task, cancelTkn);
        }


        private TimeSpan Delay(int retryAttempt)
            => TimeSpan.FromSeconds(Math.Max(5, retryAttempt));


        private bool IsRetryable(AggregateException agEx)
        {
            var webEx = agEx.InnerException as WebException;
            if (webEx == null) return false;

            switch (webEx.Status)
            {
                case WebExceptionStatus.ConnectFailure:
                case WebExceptionStatus.ReceiveFailure:
                case WebExceptionStatus.SendFailure:
                case WebExceptionStatus.PipelineFailure:
                case WebExceptionStatus.RequestCanceled:
                case WebExceptionStatus.ConnectionClosed:
                case WebExceptionStatus.KeepAliveFailure:
                case WebExceptionStatus.Pending:
                case WebExceptionStatus.Timeout:
                    return true;
                default:
                    return false;
            }
        }
    }
}
