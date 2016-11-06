using System.Threading.Tasks;
using Repo2.Core.ns11.Drupal8;

namespace Repo2.Core.ns11.RestClients
{
    public interface IR2RestClient
    {
        Task<T>  NoAuthPOST      <T>(string url, object postBody);
        Task<T>  CookieAuthGET   <T>(D8Cookie cookie, string url);
    }
}
