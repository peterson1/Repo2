using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
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


        public Task<List<R2Package>> ListByFilename(R2Package package)
        {
            //var list = await _client.List<PackagesByFilename1>(package.Filename);
            //return list.Select(x => x as R2Package).ToList();
            return _client.List<R2Package, PackagesByFilename1>(package.Filename);
        }


        public Task<NodeReply>  UpdateRemoteNode (R2Package updatedPkg)
            => _client.PatchNode(updatedPkg);


        //public async Task<bool> IsOutdated(R2Package localPackage)
        //{
        //    var list = await ListByFilename(localPackage);
        //    if (list.Count == 0) return false;

        //    if (list.Count > 1) throw Fault
        //        .NonSolo($"Server packages named “{localPackage.Filename}”", list.Count);

        //    return list.Single().Hash != localPackage.Hash;
        //}
    }
}
