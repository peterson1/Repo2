using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.RestClients
{
    public abstract class R2RestClientBase : IR2RestClient
    {
        protected R2Credentials _creds;

        public R2RestClientBase()
        {
        }

        public    abstract Task<T>  CookieAuthGET <T>(D8Cookie cookie, string url);
        public    abstract Task<T>  NoAuthPOST    <T>(string url, object postBody);
        protected abstract Task<T>  BasicAuthGET  <T>(string resourceUrl);
        public    abstract void     AllowUntrustedCertificate(string serverThumbprint);


        protected string ToAbsolute(string resourceURL)
        {
            if (_creds == null)
                throw Fault.NullRef<R2Credentials>(nameof(_creds));

            return _creds.BaseURL.Slash(resourceURL);
        }




        public Task<List<T>> BasicAuthList<T>(string url, params string[] args)
        {
            if (args?.Length > 0)
                url = url.Slash(args.Join("/"));

            return BasicAuthGET<List<T>>(url);
        }


        public void SetCredentials(R2Credentials credentials)
        {
            _creds = credentials;
        }

    }
}
