using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.RestClients;

namespace Repo2.Core.ns11.NodeManagers
{
    public class D8PkgPartManager1 : IPackagePartManager
    {
        private IR2RestClient _client;

        public D8PkgPartManager1(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }


        public async Task AddNode(R2PackagePart pkgPart)
        {
            Validate(pkgPart);

            await Task.Delay(1);
        }


        private static void Validate(R2PackagePart pkgPart)
        {
            IEnumerable<string> errMsgs;
            if (!pkgPart.IsValid(out errMsgs))
                throw Fault.BadData(pkgPart, errMsgs.Join(L.f));
        }
    }
}
