using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.NodeManagers
{
    public class D8RemotePackageMgr1 : IRemotePackageManager
    {
        private IR2RestClient _client;

        public D8RemotePackageMgr1(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }


        public Task<List<R2Package>> ListByFilename(string pkgFilename, CancellationToken cancelTkn)
            => _client.List<R2Package, PackagesByFilename1>(cancelTkn, pkgFilename);


        public Task<NodeReply>  UpdateRemoteNode (R2Package updatedPkg, CancellationToken cancelTkn)
            => _client.PatchNode(updatedPkg, cancelTkn);
    }
}
