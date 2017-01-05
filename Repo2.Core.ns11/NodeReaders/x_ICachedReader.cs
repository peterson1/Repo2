using System;
using System.Threading;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.NodeReaders
{
    public interface x_ICachedReader
    {
        event EventHandler ListChanged;

        void  StartPolling  ();
        void  StopPolling   ();
        bool  IsPolling     { get; }

        TimeSpan PollInterval { get; set; }

        Task  RefreshCacheNow  (CancellationToken cancelTkn);
    }
}
