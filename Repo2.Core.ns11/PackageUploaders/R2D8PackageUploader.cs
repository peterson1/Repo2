using System;
using System.Threading.Tasks;
using Repo2.Core.ns11.RestClients;

namespace Repo2.Core.ns11.PackageUploaders
{
    public class R2D8PackageUploader : IR2PackageUploader
    {
        private IR2RestClient _client;

        public R2D8PackageUploader(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }

        public async Task<bool> Upload(string packageFilePath)
        {
            await Task.Delay(1);

            //isolate

            //compress

            //split

            //upload parts

            //update pkg node

            //try download parts

            //assemble parts

            //validate assembled file

            return false;
        }
    }
}
