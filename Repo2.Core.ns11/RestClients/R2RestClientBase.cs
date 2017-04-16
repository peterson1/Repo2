using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.RestClients
{
    public abstract class R2RestClientBase : IR2RestClient
    {
        protected EventHandler<string> _onRetry;
        public event EventHandler<string> OnRetry
        {
            add { _onRetry -= value; _onRetry += value; }
            remove { _onRetry -= value; }
        }

        private R2RestClientAuthenticator _auth;


        protected abstract Task<T> BasicAuthPOST<T>(string url, object postBody, CancellationToken cancelTkn);
        protected abstract Task<T> BasicAuthPATCH<T>(string url, object patchBody, CancellationToken cancelTkn);
        protected abstract Task<T> BasicAuthDELETE<T>(string url, CancellationToken cancelTkn);
        public abstract Task<T> CookieAuthGET<T>(D8Cookie cookie, string url, CancellationToken cancelTkn);
        public abstract Task<T> CookieAuthPOST<T>(D8Cookie cookie, string url, CancellationToken cancelTkn);
        public abstract Task<T> NoAuthPOST<T>(string url, object postBody, CancellationToken cancelTkn);

        protected abstract Task<T> BasicAuthGET<T>(string resourceUrl, CancellationToken cancelTkn);
        public abstract void AllowUntrustedCertificate(string serverThumbprint);


        public R2RestClientBase()
        {
            _auth = new R2RestClientAuthenticator(this);
        }



        public async Task<List<T>> List<T>(CancellationToken cancelTkn, params object[] args)
            where T : IRestExportView, new()
        {
            var view = new T();
            var list = await GetList<T>(view.DisplayPath, view.CastArguments(args), cancelTkn);

            foreach (var item in list)
                item?.PostProcess();

            return list;
        }


        public async Task<List<TModel>> List<TModel, TDto>(CancellationToken cancelTkn, params object[] args)
            where TDto : IRestExportView, TModel, new()
            where TModel : class
        {
            var list = await List<TDto>(cancelTkn, args);

            if ((list == null) || (list.Count == 0))
                return new List<TModel>();

            return list.Select(x => x as TModel).ToList();
        }


        public Task<List<object>> ListRevisions(string nodeTitle, CancellationToken cancelTkn)
            => GetList<object>(D8.REVISIONS_BY_TITLE, new List<string>{ nodeTitle }, cancelTkn);


        public async Task<byte[]> GetBytes<T>(CancellationToken cancelTkn, params object[] args) 
            where T : IRestExportView, IBase64Content, new()
        {
            var list = await GetList<T>(new T().DisplayPath,
                                        new T().CastArguments(args), 
                                        cancelTkn);
            return Convert.FromBase64String(list[0].Base64Content);
        }


        private Task<List<T>> GetList<T>(string url, IEnumerable<string> args, CancellationToken cancelTkn)
            => BasicAuthGET<List<T>>(ToURL(url, args), cancelTkn);



        public async Task<List<TModel>> Paged<TModel, TDto>(int pageNumber, int itemsPerPage, CancellationToken cancelTkn)
            where TModel : class
            where TDto : IRestExportView, TModel, new()
        {
            if (pageNumber < 1)
                throw new ArgumentException($"Expected “{nameof(pageNumber)}” argument to be greater than or equal to 1, but was [{pageNumber}]");

            var offset = itemsPerPage * (pageNumber - 1);
            var url    = AppendPagerArgs<TDto>(itemsPerPage, offset);
            var dtos   = await BasicAuthGET<List<TDto>>(url, cancelTkn);

            foreach (var item in dtos)
                item?.PostProcess();

            return dtos.Select(x => x as TModel).ToList();
        }


        public async Task<List<TModel>> PagedSerial<TModel, TDto>(int itemsPerPage, int totalItemsCount, CancellationToken cancelTkn)
            where TModel : class
            where TDto : IRestExportView, TModel, new()
        {
            var list = new List<TModel>();

            for (int i = 0; i < totalItemsCount; i += itemsPerPage)
            {
                var url = AppendPagerArgs<TDto>(itemsPerPage, i);
                var partial = await BasicAuthGET<List<TDto>>(url, cancelTkn);
                list.AddRange(partial.Select(x => x as TModel));
            }
            return list;
        }


        private string AppendPagerArgs<TDto>(int itemsPerPage, int offset)
            where TDto : IRestExportView, new()
        {
            var url = new TDto().DisplayPath;
            return $"{url}?items_per_page={itemsPerPage}&offset={offset}";
        }


        public async Task<List<TModel>> PagedParallel<TModel, TDto>(int itemsPerPage, int totalItemsCount, int requestGapMS, CancellationToken cancelTkn)
            where TModel : class
            where TDto : IRestExportView, TModel, new()
        {
            var jobs = new List<Task<List<TDto>>>();
            var list = new List<TModel>();

            var delay = 0;
            for (int i = 0; i < totalItemsCount; i += itemsPerPage)
            {
                var url = AppendPagerArgs<TDto>(itemsPerPage, i);
                jobs.Add(GetDelayedPage<TDto>(delay, url, cancelTkn));
                delay += requestGapMS;
            }

            await Task.WhenAll(jobs);

            foreach (var job in jobs)
            {
                var partial = await job;
                list.AddRange(partial.Select(x => x as TModel));
            }

            return list;
        }


        private async Task<List<TDto>> GetDelayedPage<TDto>(int delayMS, string url, CancellationToken cancelTkn)
        {
            await Task.Delay(delayMS);
            return await BasicAuthGET<List<TDto>>(url, cancelTkn);
        }




        #region Authentication

        public bool          IsEnablingWriteAccess          => _auth.IsEnablingWriteAccess;
        protected string        CsrfToken                      => _auth.CsrfToken;
        protected R2Credentials Creds                          => _auth.Creds;
        public    void          StopEnablingWriteAccess()      => _auth.StopEnablingWriteAccess();
        protected string        ToAbsolute(string resourceURL) => _auth.ToAbsolute(resourceURL);
        public    Task<bool>    DisableWriteAccess()           => _auth.DisableWriteAccess();

        public void SetCredentials(R2Credentials credentials, bool addCertToWhiteList)
            => _auth.SetCredentials(credentials, addCertToWhiteList);

        public Task<bool> EnableWriteAccess(R2Credentials credentials, bool addCertToWhiteList)
            => _auth.EnableWriteAccess(credentials, addCertToWhiteList);


        #endregion






        private string ToURL(string resourceUrl, IEnumerable<string> args)
            => args?.Count() > 0 
                ? resourceUrl.Slash(args.Join("/")) : resourceUrl;




        public async Task<NodeReply> PostNode<T>(T node, CancellationToken cancelTkn) 
            where T : ID8Node
        {
            var url  = D8.NODE_FORMAT_HAL;
            var mapd = D8NodeMapper.Cast(node, _auth.Creds.BaseURL);
            var dict = await BasicAuthPOST
                        <Dictionary<string, object>>(url, mapd, cancelTkn);

            return new NodeReply(dict);
        }


        public async Task<NodeReply> SilentPost<T>(T node, CancellationToken cancelTkn) 
            where T : ID8Node
        {
            try
            {
                return await PostNode(node, cancelTkn);
            }
            catch (Exception ex)
            {
                return NodeReply.Fail(ex);
            }
        }



        public async Task<NodeReply> PatchNode<T>(T node, CancellationToken cancelTkn, string revisionLog) 
            where T : ID8Node
        {
            var url  = string.Format(D8.NODE_X_FORMAT_HAL, node.nid);
            //var url  = string.Format(D8.NODE_X_REV_Y_FMT_HAL, node.nid, node.vid);
            var mapd = D8NodeMapper.Cast(node, _auth.Creds.BaseURL);

            if (!revisionLog.IsBlank())
            {
                var now = DateTime.Now.ToString("d MMM yyyy, h:mmtt");
                var log = $"[{Creds.Username}, {now}] {revisionLog}";
                mapd.Add("revision_log", D8HALJson.ValueField(log));
            }


            Dictionary<string, object> dict;

            try
            {
                dict = await BasicAuthPATCH
                    <Dictionary<string, object>>(url, mapd, cancelTkn);

                return new NodeReply(dict);
            }
            catch (Exception ex)
            {
                return NodeReply.Fail(ex);
            }

        }


        public async Task<RestReply> DeleteNode(int nodeID, CancellationToken cancelTkn)
        {
            var url = string.Format(D8.NODE_X_FORMAT_HAL, nodeID);
            var dict = await BasicAuthDELETE
                        <Dictionary<string, object>>(url, cancelTkn);
            return new RestReply(dict);
        }
    }
}
