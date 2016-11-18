using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Drupal8;

namespace Repo2.Core.ns11.RestClients
{
    public interface IR2RestClient
    {
        void        SetCredentials     (R2Credentials credentials);
        Task<bool>  EnableWriteAccess  (R2Credentials credentials);

        Task<List<T>>  GetList   <T>(string url, params string[] args);

        Task<Dictionary<string, object>>  PostNode  <T>
            (T node) where T : D8NodeBase;
    }
}
