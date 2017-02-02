using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Authentication;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Serialization;
using Repo2.SDK.WPF45.TaskResilience;
using ServiceStack;

namespace Repo2.SDK.WPF45.RestClients
{
    public class ResilientClient1 : R2RestClientBase
    {
        //private CrappyConnectionRetryer _retry;

        public ResilientClient1()
        {
            //_retry = ConfigureNewRetryer();
        }


        private CrappyConnectionRetryer Retry => CreateNewRetryer();



        protected override Task<T> BasicAuthGET<T>(string resourceUrl, CancellationToken cancelTkn)
            => Retry.Forever<T>(resourceUrl, (url, ct) => 
                url.GetJsonFromUrlAsync(SetBasicAuthRequest), cancelTkn);


        protected override Task<T> BasicAuthPOST<T>(string resourceUrl, object postBody, CancellationToken cancelTkn)
            => Retry.Forever<T>(resourceUrl, (url, ct) =>
                 url.PostStringToUrlAsync(Json.Serialize(postBody),
                    D8.CONTENT_TYPE_HAL, D8.CONTENT_TYPE_HAL, SetBasicAuthRequest), cancelTkn);


        protected override Task<T> BasicAuthPATCH<T>(string resourceUrl, object patchBody, CancellationToken cancelTkn)
            => Retry.Forever<T>(resourceUrl, (url, ct) =>
                 url.PatchStringToUrlAsync(Json.Serialize(patchBody),
                    D8.CONTENT_TYPE_HAL, D8.CONTENT_TYPE_HAL, SetBasicAuthRequest), cancelTkn);


        protected override Task<T> BasicAuthDELETE<T>(string resourceUrl, CancellationToken cancelTkn)
            => Retry.Forever<T>(resourceUrl, (url, ct) =>
                url.DeleteFromUrlAsync(D8.CONTENT_TYPE_HAL, SetBasicAuthRequest), cancelTkn);


        public override Task<T> NoAuthPOST<T>(string resourceUrl, object postBody, CancellationToken cancelTkn)
            => Retry.Forever<T>(resourceUrl, (url, ct) => 
                url.PostJsonToUrlAsync(postBody), cancelTkn);




        public override Task<T> CookieAuthGET<T>(D8Cookie cookie, string resourceUrl, CancellationToken cancelTkn)
        {
            var client = new JsonServiceClient(Creds.BaseURL);
            client.SetCookie(cookie.Name, cookie.Id);

            return Retry.Forever<T>(resourceUrl, (x, ct) 
                => client.GetAsync<string>(resourceUrl), cancelTkn);
        }

        public override Task<T> CookieAuthPOST<T>(D8Cookie cookie, string resourceUrl, CancellationToken cancelTkn)
        {
            var client = new JsonServiceClient(Creds.BaseURL);
            client.SetCookie(cookie.Name, cookie.Id);

            return Retry.Forever<T>(resourceUrl, (url, ct)
                //=> client.PostAsync<string>(resourceUrl, null), cancelTkn);
                //=> resourceUrl.PostToUrlAsync(string.Empty, requestFilter: req => { req.Headers["X-CSRF-Token"] = CsrfToken; }), cancelTkn);
                => url.PostToUrlAsync(string.Empty, "application/json", r => SetBasicAuthRequest(r)), cancelTkn);
        }


        public override void AllowUntrustedCertificate(string serverThumbprint)
            => Certificator.AllowFrom(serverThumbprint);


        private void SetBasicAuthRequest(HttpWebRequest req)
        {
            req.AddBasicAuth(Creds.Username, Creds.Password);
            req.Headers["X-CSRF-Token"] = CsrfToken;
        }


        private CrappyConnectionRetryer CreateNewRetryer()
        {
            var retryr = new CrappyConnectionRetryer();
            retryr.MakeAbsolute = url => ToAbsolute(url);

            retryr.OnRetry = (ex, span) => _onRetry?.Invoke(this, 
                $"{ex.FromUrl(Creds.BaseURL).Message}{L.f}Retrying in {span.Seconds} seconds ...");

            return retryr;
        }
    }
}
