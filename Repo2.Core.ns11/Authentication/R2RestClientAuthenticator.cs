using System;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.RestClients;

namespace Repo2.Core.ns11.Authentication
{
    [ImplementPropertyChanged]
    internal class R2RestClientAuthenticator
    {
        private R2RestClientBase        _client;
        private CancellationTokenSource _cancelr;

        internal string         CsrfToken                { get; private set; }
        internal R2Credentials  Creds                    { get; private set; }
        internal bool           IsEnablingWriteAccess    { get; private set; }


        public R2RestClientAuthenticator(R2RestClientBase r2RestClientBase)
        {
            _client = r2RestClientBase;
        }


        internal void SetCredentials(R2Credentials credentials, bool addCertToWhiteList)
        {
            if (credentials != null)
                Creds = credentials;

            if (addCertToWhiteList)
                _client.AllowUntrustedCertificate(Creds.CertificateThumb);
        }


        internal async Task<bool> EnableWriteAccess(R2Credentials credentials, bool addCertToWhiteList)
        {
            _cancelr = new CancellationTokenSource();
            IsEnablingWriteAccess = true;
            try
            {
                return await RequestWriteAccess(credentials, _cancelr.Token, addCertToWhiteList);
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            finally
            {
                IsEnablingWriteAccess = false;
            }
        }


        private async Task<bool> RequestWriteAccess(R2Credentials credentials, 
            CancellationToken cancelTkn, bool addCertToWhiteList)
        {
            CsrfToken = string.Empty;
            SetCredentials(credentials, addCertToWhiteList);

            D8Cookie cookie = null;
            await Task.Run(async () =>
            {
                cookie = await GetCookie(cancelTkn);
            }
            ).ConfigureAwait(false);

            if (cookie == null) return false;

            await Task.Run(async () =>
            {
                CsrfToken = await GetCsrfToken(cookie, cancelTkn);
            }
            ).ConfigureAwait(false);

            return !CsrfToken.IsBlank();
        }


        private Task<D8Cookie> GetCookie(CancellationToken cancelTkn)
            => _client.NoAuthPOST<D8Cookie>(D8.API_USER_LOGIN, new
            {
                username = Creds.Username,
                password = Creds.Password
            }, cancelTkn);


        private Task<string> GetCsrfToken(D8Cookie cookie, CancellationToken cancelTkn)
            => _client.CookieAuthGET<string>(cookie, D8.REST_SESSION_TOKEN, cancelTkn);


        internal void StopEnablingWriteAccess()
        {
            if (_cancelr == null) return;
            _cancelr.Cancel(true);
        }


        internal string ToAbsolute(string resourceURL)
        {
            if (Creds == null)
                throw Fault.NullRef<R2Credentials>(nameof(Creds));

            return Creds.BaseURL.Slash(resourceURL);
        }
    }
}
