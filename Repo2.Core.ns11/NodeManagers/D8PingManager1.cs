using System;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.NodeManagers
{
    public class D8PingManager1 : IPingManager
    {
        private IR2RestClient _client;

        public D8PingManager1(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }


        public async Task<R2Ping> GetCurrentUserPing(string pkgFilename)
        {
            var list = await _client.List<PingsForCurrentUser1>();
            if (list.Count == 0) throw Fault.NoItems("Pings for current user");

            var named = list.Where(x 
                => x.PackageFilename == pkgFilename).ToList();

            if (named.Count == 0) throw Fault
                .NoMatch<R2Ping>("PkgFilename", pkgFilename);

            if (named.Count > 1) throw Fault
                .NonSolo($"Pings for {pkgFilename}", named.Count);

            return named.First();
        }


        public async Task<NodeReply> UpdateNode(R2Ping ping)
        {
            var reply = await _client.PatchNode(ping);

            return reply;
        }
    }
}
