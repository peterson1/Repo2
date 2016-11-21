using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;

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


        public async Task<bool> IsUploadable(R2Package localPkg)
        {
            if (localPkg == null)
            {
                ReasonWhyNot = $"“{nameof(localPkg)}” argument is NULL.";
                return false;
            }

            if (!localPkg.FileFound)
            {
                ReasonWhyNot = $"“{localPkg.Filename}” not found in {localPkg.LocalDir}.";
                return false;
            }

            var list = await _client.List<PackagesByTitle1>(localPkg);
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

            if (remotePkg.RemoteHash == localPkg.LocalHash)
            {
                ReasonWhyNot = "Local hash matches remote hash.";
                return false;
            }

            return true;
        }
    }
}
