using System;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.RestClients;
using System.ComponentModel;

namespace Repo2.Core.ns11.Authentication
{
    //[ImplementPropertyChanged]
    internal class R2RestClientAuthenticator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private R2RestClientBase        _client;
        private CancellationTokenSource _cancelr;
        private D8Cookie _cookie;


        internal string          CsrfToken                { get; private set; }
        internal IR2Credentials  Creds                    { get; private set; }
        internal bool            IsEnablingWriteAccess    { get; private set; }


        public R2RestClientAuthenticator(R2RestClientBase r2RestClientBase)
        {
            _client = r2RestClientBase;
        }


        internal void SetCredentials(IR2Credentials credentials, bool addCertToWhiteList)
        {
            if (credentials != null)
                Creds = credentials;

            if (addCertToWhiteList)
                _client.AllowUntrustedCertificate(Creds.CertificateThumb);
        }


        internal async Task<bool> EnableWriteAccess(IR2Credentials credentials, bool addCertToWhiteList)
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


        private async Task<bool> RequestWriteAccess(IR2Credentials credentials, 
            CancellationToken cancelTkn, bool addCertToWhiteList)
        {
            CsrfToken = string.Empty;
            SetCredentials(credentials, addCertToWhiteList);

            _cookie = null;
            await Task.Run(async () =>
            {
                _cookie = await GetCookie(cancelTkn);
            }
            ).ConfigureAwait(false);

            if (_cookie == null) return false;

            await Task.Run(async () =>
            {
                CsrfToken = await GetCsrfToken(_cookie, cancelTkn);
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


        internal async Task<bool> DisableWriteAccess()
        {
            var res = await _client.CookieAuthPOST<string>
                (_cookie, D8.API_USER_LOGOUT, new CancellationToken());

            return res.Trim() == "[]";
        }


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
