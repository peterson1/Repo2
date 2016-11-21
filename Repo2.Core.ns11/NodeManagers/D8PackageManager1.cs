using System;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.RestClients;

namespace Repo2.Core.ns11.NodeManagers
{
    public class D8PackageManager1 : IPackageManager
    {
        private IR2RestClient _client;

        public D8PackageManager1(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }


        public Task<Reply>  UpdateNode (R2Package updatedPkg)
        {
            throw new NotImplementedException();
        }
    }
}
