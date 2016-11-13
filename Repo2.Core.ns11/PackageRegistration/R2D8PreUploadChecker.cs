using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.RestClients;

namespace Repo2.Core.ns11.PackageRegistration
{
    public class R2D8PreUploadChecker : IR2PreUploadChecker
    {
        private IR2RestClient _client;

        public R2D8PreUploadChecker(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }


        public async Task<bool> IsUploadable(R2Package localPackage)
        {
            var nme  = localPackage.Filename;
            var url  = D8.PACKAGE_CHECKER_1;
            var list = await _client.BasicAuthList<R2Package>(url, nme);

            if (list == null) return false;

            return list.Count == 1;
        }
    }
}
