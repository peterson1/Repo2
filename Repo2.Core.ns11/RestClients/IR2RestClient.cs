using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.RestClients
{
    public interface IR2RestClient
    {
        void        SetCredentials     (R2Credentials credentials);
        Task<bool>  EnableWriteAccess  (R2Credentials credentials);

        Task<List<T>> List<T>(params object[] args) where T : IRestExportView, new();

        Task<NodeReply>  PostNode  <T>(T node) where T : D8NodeBase;
        Task<RestReply>  DeleteNode (int nodeID);
    }
}
