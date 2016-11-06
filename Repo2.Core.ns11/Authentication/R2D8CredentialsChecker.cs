using System.Threading.Tasks;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.RestClients;

namespace Repo2.Core.ns11.Authentication
{
    public class R2D8CredentialsChecker : IR2CredentialsChecker
    {
        private IR2RestClient _client;
        protected string      _csrfToken;


        public R2D8CredentialsChecker(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }


        public bool  CanRead     { get; private set; }
        public bool  CanWrite => !_csrfToken.IsBlank();


        public async Task Check(R2Credentials credentials)
        {
            var cookie = await GetCookie(credentials);

            CanRead    = cookie != null;
            if (!CanRead) return;

            _csrfToken = await GetCsrfToken(cookie);
        }


        private Task<D8Cookie> GetCookie(R2Credentials creds)
            => _client.NoAuthPOST<D8Cookie>(D8.API_USER_LOGIN, new
            {
                username = creds.Username,
                password = creds.Password
            });


        private Task<string> GetCsrfToken(D8Cookie cookie)
            => _client.CookieAuthGET<string>(cookie, D8.REST_SESSION_TOKEN);

    }
}
