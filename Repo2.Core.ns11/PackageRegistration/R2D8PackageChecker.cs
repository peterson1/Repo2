using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.RestClients;

namespace Repo2.Core.ns11.PackageRegistration
{
    public class R2D8PackageChecker : IR2PackageChecker
    {
        private IR2RestClient _client;

        public R2D8PackageChecker(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }


        public async Task<bool> IsRegistered(string packageName, R2Credentials credentials)
        {
            _client.SetCredentials(credentials);

            var list = await _client.BasicAuthList<List<R2Package>>
                        (D8.PACKAGE_CHECKER_1, packageName);

            if (list == null) return false;

            return list.Count == 1;
        }
    }
}
