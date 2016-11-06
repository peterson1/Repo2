using System;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.ExceptionCasters;
using Repo2.SDK.WPF45.Serialization;
using Repo2.SDK.WPF45.TaskResilience;
using ServiceStack;

namespace Repo2.SDK.WPF45.RestClients
{
    public class ResilientClient1 : R2RestClientBase
    {
        private CrappyConnectionRetryer _retry;

        public ResilientClient1(CrappyConnectionRetryer taskRetryer, R2Credentials r2Credentials) : base(r2Credentials)
        {
            _retry = taskRetryer;
        }


        public override Task<T> NoAuthPOST<T>(string resourceUrl, object postBody)
            => RetryForever<T>(resourceUrl, url
                => url.PostJsonToUrlAsync(postBody));


        private async Task<T> RetryForever<T>(string resourceUrl, Func<string, Task<string>> task)
        {
            var json = string.Empty;
            var url  = ToAbsolute(resourceUrl);
            try
            {
                json = await _retry.Forever(() => task(url));
            }
            catch (Exception ex){ throw ex.FromUrl(url); }

            return TryDeserialize<T>(json);
        }


        public override async Task<T> CookieAuthGET<T>(D8Cookie cookie, string resourceUrl)
        {
            var client = new JsonServiceClient(_creds.BaseURL);
            client.SetCookie(cookie.Name, cookie.Id);

            var ret = await RetryForever<T>(resourceUrl, x 
                => client.GetAsync<string>(resourceUrl));

            //Console.WriteLine($"ret: {ret?.ToString()}");
            return ret;
        }


        private T TryDeserialize<T>(string json)
        {
            var output = default(T);

            if (Json.TryDeserialize<T>(json, out output))
                return output;
            else
            {
                var msg = $"Failed to deserialize ‹{typeof(T).Name}› from json:{L.f}“{json}”";
                Console.WriteLine(msg);
                return default(T);
            }
        }


        //public override bool TryDeserialize<T>(string json, out T output)
        //    => Json.TryDeserialize(json, out output);
    }
}
