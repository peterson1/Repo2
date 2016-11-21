using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.RestClients
{
    public abstract class R2RestClientBase : IR2RestClient
    {
        protected R2Credentials _creds;
        protected string        _csrfToken;


        protected abstract Task<T>   BasicAuthPOST  <T>(string url, object postBody);
        protected abstract Task<T>   CookieAuthGET  <T>(D8Cookie cookie, string url);
        protected abstract Task<T>   NoAuthPOST     <T>(string url, object postBody);

        protected abstract Task<T>   BasicAuthGET   <T>(string resourceUrl);
        protected abstract void      AllowUntrustedCertificate (string serverThumbprint);


        public Task<List<T>> List<T>(params object[] args) 
            where T : IRestExportView, new()
                => GetList<T>(new T().DisplayPath, 
                              new T().CastArguments(args));


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



        private Task<List<T>> GetList<T>(string url, IEnumerable<string> args)
        {
            if (args?.Count() > 0)
                url = url.Slash(args.Join("/"));

            return BasicAuthGET<List<T>>(url);
        }



        protected string ToAbsolute(string resourceURL)
        {
            if (_creds == null)
                throw Fault.NullRef<R2Credentials>(nameof(_creds));

            return _creds.BaseURL.Slash(resourceURL);
        }


        public Task<Dictionary<string, object>> PostNode<T>(T node) where T : D8NodeBase
            => BasicAuthPOST<Dictionary<string, object>>(D8.NODE_FORMAT_HAL, 
                D8NodeMapper.Cast(node, _creds.BaseURL));
    }
}
