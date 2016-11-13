using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Drupal8;

namespace Repo2.Core.ns11.RestClients
{
    public interface IR2RestClient
    {
        void   SetCredentials            (R2Credentials credentials);
        void   AllowUntrustedCertificate (string serverThumbprint);

        Task<T>        NoAuthPOST      <T>(string url, object postBody);
        Task<T>        CookieAuthGET   <T>(D8Cookie cookie, string url);
        Task<List<T>>  BasicAuthList   <T>(string url, params string[] args);
    }
}
