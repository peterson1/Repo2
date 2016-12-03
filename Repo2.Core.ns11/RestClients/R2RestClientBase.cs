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
        protected    EventHandler<string> _onRetry;
        public event EventHandler<string>  OnRetry
        {
            add    { _onRetry -= value; _onRetry += value; }
            remove { _onRetry -= value; }
        }


        protected R2Credentials _creds;
        protected string _csrfToken;


        protected abstract Task<T> BasicAuthPOST<T>(string url, object postBody, CancellationToken cancelTkn);
        protected abstract Task<T> BasicAuthPATCH<T>(string url, object patchBody, CancellationToken cancelTkn);
        protected abstract Task<T> BasicAuthDELETE<T>(string url, CancellationToken cancelTkn);
        protected abstract Task<T> CookieAuthGET<T>(D8Cookie cookie, string url, CancellationToken cancelTkn);
        protected abstract Task<T> NoAuthPOST<T>(string url, object postBody, CancellationToken cancelTkn);

        protected abstract Task<T> BasicAuthGET<T>(string resourceUrl, CancellationToken cancelTkn);
        protected abstract void AllowUntrustedCertificate(string serverThumbprint);


        public Task<List<T>> List<T>(CancellationToken cancelTkn, params object[] args)
            where T : IRestExportView, new()
                => GetList<T>(new T().DisplayPath,
                              new T().CastArguments(args),
                              cancelTkn);


        public async Task<List<TModel>> List<TModel, TDto>(CancellationToken cancelTkn, params object[] args) 
            where TDto   : IRestExportView, TModel, new()
            where TModel : class
        {
            var list = await List<TDto>(cancelTkn, args);

            if ((list == null) || (list.Count == 0))
                return new List<TModel>();

            return list.Select(x => x as TModel).ToList();
        }


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


        public void SetCredentials(R2Credentials credentials, bool addCertToWhiteList)
        {
            _creds = credentials;

            if (addCertToWhiteList)
                AllowUntrustedCertificate(_creds.CertificateThumb);
        }


        public async Task<bool> EnableWriteAccess(R2Credentials credentials, CancellationToken cancelTkn, bool addCertToWhiteList)
        {
            if (!_csrfToken.IsBlank()) return true;

            SetCredentials(credentials, addCertToWhiteList);

            var cookie = await GetCookie(credentials, cancelTkn);
            if (cookie == null) return false;

            _csrfToken = await GetCsrfToken(cookie, cancelTkn);
            return !_csrfToken.IsBlank();
        }



        private Task<D8Cookie> GetCookie(R2Credentials creds, CancellationToken cancelTkn)
            => NoAuthPOST<D8Cookie>(D8.API_USER_LOGIN, new
            {
                username = creds.Username,
                password = creds.Password
            }, cancelTkn);


        private Task<string> GetCsrfToken(D8Cookie cookie, CancellationToken cancelTkn)
            => CookieAuthGET<string>(cookie, D8.REST_SESSION_TOKEN, cancelTkn);





        private string ToURL(string resourceUrl, IEnumerable<string> args)
            => args?.Count() > 0 
                ? resourceUrl.Slash(args.Join("/")) : resourceUrl;


        protected string ToAbsolute(string resourceURL)
        {
            if (_creds == null)
                throw Fault.NullRef<R2Credentials>(nameof(_creds));

            return _creds.BaseURL.Slash(resourceURL);
        }


        public async Task<NodeReply> PostNode<T>(T node, CancellationToken cancelTkn) 
            where T : D8NodeBase
        {
            var url  = D8.NODE_FORMAT_HAL;
            var mapd = D8NodeMapper.Cast(node, _creds.BaseURL);
            var dict = await BasicAuthPOST
                        <Dictionary<string, object>>(url, mapd, cancelTkn);

            return new NodeReply(dict);
        }


        public async Task<NodeReply> PatchNode<T>(T node, CancellationToken cancelTkn, string revisionLog) 
            where T : D8NodeBase
        {
            var url  = string.Format(D8.NODE_X_FORMAT_HAL, node.nid);
            //var url  = string.Format(D8.NODE_X_REV_Y_FMT_HAL, node.nid, node.vid);
            var mapd = D8NodeMapper.Cast(node, _creds.BaseURL);

            //if (!revisionLog.IsBlank())
            //{
            //    mapd.Add("vid", D8HALJson.ValueField(node.vid));
            //    mapd.Add("revision", D8HALJson.ValueField(1));
            //    mapd.Add("log", D8HALJson.ValueField(revisionLog));
            //}

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
