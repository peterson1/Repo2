using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.RestClients;

namespace Repo2.Core.ns11.PackageRegistration
{
    public class D8PreUploadChecker1 : IR2PreUploadChecker
    {
        private IR2RestClient _client;

        public D8PreUploadChecker1(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }

        public string ReasonWhyNot { get; private set; }


        public async Task<bool> IsUploadable(R2Package localPackage)
        {
            if (localPackage == null)
            {
                ReasonWhyNot = $"“{nameof(localPackage)}” argument is NULL.";
                return false;
            }
            var nme  = localPackage.Filename;
            var url  = D8.PACKAGE_CHECKER_1;
            var list = await _client.GetList<R2Package>(url, nme);

            if (list == null)
            {
                ReasonWhyNot = "List from server is NULL.";
                return false;
            }
            if (list.Count < 1)
            {
                ReasonWhyNot = "List from server is EMPTY.";
                return false;
            }
            var remotePkg = list[0];

            if (remotePkg.RemoteHash == localPackage.LocalHash)
            {
                ReasonWhyNot = "Local hash matches remote hash.";
                return false;
            }

            return true;
        }
    }
}
