using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.NodeManagers
{
    public class D8PkgPartManager1 : IPackagePartManager
    {
        private IR2RestClient _client;
        private IFileSystemAccesor _fileIO;

        public D8PkgPartManager1(IR2RestClient r2RestClient, IFileSystemAccesor fileSystemAccesor)
        {
            _client = r2RestClient;
            _fileIO = fileSystemAccesor;
        }


        public async Task<Reply> AddNode(R2PackagePart pkgPart, CancellationToken cancelTkn)
        {
            var errors = new List<string>();
            var warnings = new List<string>();

            if (!pkgPart.IsValid(ref errors))
                goto ReturnReply;

            if (await AlreadyInServer(pkgPart, cancelTkn))
            {
                warnings.Add($"“{pkgPart.PackageFilename}” part {pkgPart.PartNumber} of {pkgPart.TotalParts} already exists in the server.");
                goto ReturnReply;
            }

            var dict = await _client.PostNode(pkgPart, cancelTkn);

            //todo: add status and result to returned reply

            ReturnReply:
            return new Reply
            {
                Errors = errors,
                Warnings = warnings
            };
        }


        public Task<List<R2PackagePart>> ListByPkgHash(R2Package package, CancellationToken cancelTkn)
            => ListByPkgHash(package.Filename, package.Hash, cancelTkn);


        public Task<List<R2PackagePart>> ListByPkgHash(string packageFilename, string packageHash, CancellationToken cancelTkn)
            => _client.List<R2PackagePart, PartsByPkgHash1>(cancelTkn, packageFilename, packageHash);


        public Task<List<R2PackagePart>> ListByPkgName(string packageFilename, CancellationToken cancelTkn)
            => _client.List<R2PackagePart, PartsByPkgName1>(cancelTkn, packageFilename);
        //{
        //    try
        //    {
        //        return await _client.List<R2PackagePart, PartsByPkgName1>(cancelTkn, packageFilename);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}


        private async Task<bool> AlreadyInServer(R2PackagePart part, CancellationToken cancelTkn)
        {
            var list = await ListByPkgHash(part.PackageFilename, part.PackageHash, cancelTkn);
            return list.Any(x => x.PartHash == part.PartHash
                            && x.PartNumber == part.PartNumber
                            && x.TotalParts == part.TotalParts);
        }



        public async Task<Reply> DeleteByPkgHash(R2Package package, CancellationToken cancelTkn)
        {
            var list = await ListByPkgHash(package, cancelTkn);
            foreach (var item in list)
            {
                var reply = await _client.DeleteNode(item.nid, cancelTkn);
                if (reply.Failed) return reply;
            }
            return Reply.Success;
        }


        public Task<RestReply> DeleteByPartNid(int partNodeID, CancellationToken cancelTkn)
            => _client.DeleteNode(partNodeID, cancelTkn);


        public async Task<string> DownloadToTemp(R2PackagePart part, CancellationToken cancelTkn)
        {
            var byts = await _client.GetBytes<PartContentsByHash1>(cancelTkn, part.PartHash);

            if (byts == null) throw Fault.NullRef<byte[]>("_client.GetBytes<PartContentsByHash1>");
            if (byts.Length == 0) throw Fault.NoItems("byte[] from _client.GetBytes<PartContentsByHash1>()");

            return _fileIO.WriteTempFile(byts);
        }
    }
}
