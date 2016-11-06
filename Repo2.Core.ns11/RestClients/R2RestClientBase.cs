using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Drupal8;
using System.Net;
using Repo2.Core.ns11.Extensions.StringExtensions;
using System;

namespace Repo2.Core.ns11.RestClients
{
    public abstract class R2RestClientBase : IR2RestClient
    {
        protected R2Credentials _creds;

        public R2RestClientBase(R2Credentials r2Credentials)
        {
            _creds = r2Credentials;
        }

        public abstract Task<T>  CookieAuthGET   <T>(D8Cookie cookie, string url);
        public abstract Task<T>  NoAuthPOST      <T>(string url, object postBody);


        protected string ToAbsolute(string resourceURL)
            => _creds?.BaseURL.Slash(resourceURL);
    }
}
