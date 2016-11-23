using System.Net;
using System.Threading.Tasks;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Authentication;
using Repo2.SDK.WPF45.Serialization;
using Repo2.SDK.WPF45.TaskResilience;
using ServiceStack;

namespace Repo2.SDK.WPF45.RestClients
{
    public class ResilientClient1 : R2RestClientBase
    {
        private CrappyConnectionRetryer _retry;

        public ResilientClient1()
        {
            _retry = ConfigureNewRetryer();
        }



        protected override Task<T> BasicAuthGET<T>(string resourceUrl)
            => _retry.Forever<T>(resourceUrl, url => 
                url.GetJsonFromUrlAsync(SetBasicAuthRequest));


        protected override Task<T> BasicAuthPOST<T>(string resourceUrl, object postBody)
            => _retry.Forever<T>(resourceUrl, url =>
                 url.PostStringToUrlAsync(Json.Serialize(postBody),
                    D8.CONTENT_TYPE_HAL, D8.CONTENT_TYPE_HAL, SetBasicAuthRequest));


        protected override Task<T> BasicAuthPATCH<T>(string resourceUrl, object patchBody)
            => _retry.Forever<T>(resourceUrl, url =>
                 url.PatchStringToUrlAsync(Json.Serialize(patchBody),
                    D8.CONTENT_TYPE_HAL, D8.CONTENT_TYPE_HAL, SetBasicAuthRequest));


        protected override Task<T> BasicAuthDELETE<T>(string resourceUrl)
            => _retry.Forever<T>(resourceUrl, url =>
                url.DeleteFromUrlAsync(D8.CONTENT_TYPE_HAL, SetBasicAuthRequest));


        protected override Task<T> NoAuthPOST<T>(string resourceUrl, object postBody)
            => _retry.Forever<T>(resourceUrl, url => 
                url.PostJsonToUrlAsync(postBody));




        protected override Task<T> CookieAuthGET<T>(D8Cookie cookie, string resourceUrl)
        {
            var client = new JsonServiceClient(_creds.BaseURL);
            client.SetCookie(cookie.Name, cookie.Id);

            return _retry.Forever<T>(resourceUrl, x 
                => client.GetAsync<string>(resourceUrl));
        }


        protected override void AllowUntrustedCertificate(string serverThumbprint)
            => Certificator.AllowFrom(serverThumbprint);


        private void SetBasicAuthRequest(HttpWebRequest req)
        {
            req.AddBasicAuth(_creds.Username, _creds.Password);
            req.Headers["X-CSRF-Token"] = _csrfToken;
        }


        private CrappyConnectionRetryer ConfigureNewRetryer()
        {
            var retryr = new CrappyConnectionRetryer();
            retryr.MakeAbsolute = url => ToAbsolute(url);
            return retryr;
        }
    }
}
