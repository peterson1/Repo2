using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.Serialization;

namespace Repo2.Core.ns11.NodeReaders
{
    public abstract class x_CachedReaderBase<T> : x_ICachedReader
    {
        private      EventHandler _listChanged;
        public event EventHandler  ListChanged
        {
            add    { _listChanged -= value; _listChanged += value; }
            remove { _listChanged -= value; }
        }

        private ReadOnlyCollection<T>   _memCache;
        private string                  _cacheHash;
        private CancellationTokenSource _cancelSrc;
        private IR2Serializer           _serialr;


        public x_CachedReaderBase(IR2Serializer r2Serializer)
        {
            _serialr = r2Serializer;
        }



        protected abstract Task<IEnumerable<T>> GetFullList(CancellationToken cancelTkn);


        public TimeSpan  PollInterval  { get; set; }

        public bool IsPolling => !_cancelSrc?.IsCancellationRequested ?? false;


        public async void StartPolling()
        {
            _cancelSrc = null;
            _cancelSrc = new CancellationTokenSource();

            while (IsPolling)
            {
                await Task.Delay(PollInterval);
                await RefreshCacheNow(_cancelSrc.Token);
            }
        }


        public void StopPolling() => _cancelSrc?.Cancel(true);


        protected async Task<ReadOnlyCollection<T>> GetMemCache(CancellationToken cancelTkn)
        {
            if (_memCache == null)
            {
                if (HasDiskCache())
                    _memCache = ReadDiskCache();
                else
                    await RefreshCacheNow(cancelTkn);
            }
            return _memCache;
        }


        public async Task RefreshCacheNow(CancellationToken cancelTkn)
        {
            if (IsPolling) StopPolling();

            var oldHash = _cacheHash;
            var list    = await GetFullList(cancelTkn);
            _cacheHash  = _serialr.ToString(list).SHA1ForUTF8();
            _memCache   = new ReadOnlyCollection<T>(list.ToList());

            if (oldHash != _cacheHash)
            {
                WriteDiskCache(_memCache);
                _listChanged?.Raise();
            }

            StartPolling();
        }


        private void WriteDiskCache(IEnumerable<T> _memCache)
        {
            throw new NotImplementedException();
        }


        private ReadOnlyCollection<T> ReadDiskCache()
        {
            throw new NotImplementedException();
        }


        private bool HasDiskCache()
        {
            throw new NotImplementedException();
        }
    }
}
