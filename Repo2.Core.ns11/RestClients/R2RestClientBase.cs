using System;
using System.Collections.Generic;
using System.Linq;
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
        protected R2Credentials _creds;
        protected string _csrfToken;


        protected abstract Task<T> BasicAuthPOST<T>(string url, object postBody);
        protected abstract Task<T> BasicAuthPATCH<T>(string url, object patchBody);
        protected abstract Task<T> BasicAuthDELETE<T>(string url);
        protected abstract Task<T> CookieAuthGET<T>(D8Cookie cookie, string url);
        protected abstract Task<T> NoAuthPOST<T>(string url, object postBody);

        protected abstract Task<T> BasicAuthGET<T>(string resourceUrl);
        protected abstract void AllowUntrustedCertificate(string serverThumbprint);


        public Task<List<T>> List<T>(params object[] args)
            where T : IRestExportView, new()
                => GetList<T>(new T().DisplayPath,
                              new T().CastArguments(args));


        public async Task<byte[]> GetBytes<T>(params object[] args) 
            where T : IRestExportView, IBase64Content, new()
        {
            var list = await GetList<T>(new T().DisplayPath,
                                        new T().CastArguments(args));
            return Convert.FromBase64String(list[0].Base64Content);
        }


        private Task<List<T>> GetList<T>(string url, IEnumerable<string> args)
            => BasicAuthGET<List<T>>(ToURL(url, args));


        public void SetCredentials(R2Credentials credentials)
        {
            _creds = credentials;
            AllowUntrustedCertificate(_creds.CertificateThumb);
        }


        public async Task<bool> EnableWriteAccess(R2Credentials credentials)
        {
            SetCredentials(credentials);

            var cookie = await GetCookie(credentials);
            if (cookie == null) return false;

            _csrfToken = await GetCsrfToken(cookie);
            return !_csrfToken.IsBlank();
        }



        private Task<D8Cookie> GetCookie(R2Credentials creds)
            => NoAuthPOST<D8Cookie>(D8.API_USER_LOGIN, new
            {
                username = creds.Username,
                password = creds.Password
            });


        private Task<string> GetCsrfToken(D8Cookie cookie)
            => CookieAuthGET<string>(cookie, D8.REST_SESSION_TOKEN);





        private string ToURL(string resourceUrl, IEnumerable<string> args)
            => args?.Count() > 0 
                ? resourceUrl.Slash(args.Join("/")) : resourceUrl;


        protected string ToAbsolute(string resourceURL)
        {
            if (_creds == null)
                throw Fault.NullRef<R2Credentials>(nameof(_creds));

            return _creds.BaseURL.Slash(resourceURL);
        }


        public async Task<NodeReply> PostNode<T>(T node) where T : D8NodeBase
        {
            var url  = D8.NODE_FORMAT_HAL;
            var mapd = D8NodeMapper.Cast(node, _creds.BaseURL);
            var dict = await BasicAuthPOST
                        <Dictionary<string, object>>(url, mapd);

            return new NodeReply(dict);
        }


        public async Task<NodeReply> PatchNode<T>(T node, string revisionLog) where T : D8NodeBase
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
                    <Dictionary<string, object>>(url, mapd);

                return new NodeReply(dict);
            }
            catch (Exception ex)
            {
                return NodeReply.Fail(ex);
            }

        }


        public async Task<RestReply> DeleteNode(int nodeID)
        {
            var url = string.Format(D8.NODE_X_FORMAT_HAL, nodeID);
            var dict = await BasicAuthDELETE
                        <Dictionary<string, object>>(url);
            return new RestReply(dict);
        }

    }
}
