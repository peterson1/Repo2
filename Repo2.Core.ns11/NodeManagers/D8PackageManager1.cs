using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.NodeManagers
{
    public class D8PackageManager1 : IPackageManager
    {
        private IR2RestClient _client;

        public D8PackageManager1(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }


        public Task<List<R2Package>> ListByFilename(R2Package package)
            => ListByFilename(package.Filename);


        private async Task<List<R2Package>> ListByFilename(string filename)
        {
            var list = await _client.List<PackagesByFilename1>(filename);
            return list.Select(x => x as R2Package).ToList();
        }


        public Task<NodeReply>  UpdateNode (R2Package updatedPkg)
        {
            //updatedPkg.RemoteHash = updatedPkg.LocalHash;
            return _client.PatchNode(updatedPkg);
        }
    }
}
