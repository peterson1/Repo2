using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.NodeManagers
{
    public class D8PkgPartManager1 : IPackagePartManager
    {
        private IR2RestClient      _client;
        private IFileSystemAccesor _fileIO;

        public D8PkgPartManager1(IR2RestClient r2RestClient, IFileSystemAccesor fileSystemAccesor)
        {
            _client = r2RestClient;
            _fileIO = fileSystemAccesor;
        }


        public async Task<Reply> AddNode (R2PackagePart pkgPart, CancellationToken cancelTkn)
        {
            var errors   = new List<string>();
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
                    Errors   = errors,
                    Warnings = warnings
                };
        }


        //public Task<List<R2PackagePart>> ListByPkgHash(R2PackagePart pkgPart)
        //    => ListByPkgHash(pkgPart.PackageFilename, pkgPart.PackageHash);

        public Task<List<R2PackagePart>> ListByPackage(R2Package package, CancellationToken cancelTkn)
            => ListByPackage(package.Filename, package.Hash, cancelTkn);

        public async Task<List<R2PackagePart>> ListByPackage(string packageFilename, string packageHash, CancellationToken cancelTkn)
        {
            var list = await _client.List<PartsByPackage1>(cancelTkn, packageFilename, packageHash);
            return list.Select(x => x as R2PackagePart).ToList();
        }

        private async Task<bool> AlreadyInServer(R2PackagePart part, CancellationToken cancelTkn)
        {
            var list = await ListByPackage(part.PackageFilename, part.PackageHash, cancelTkn);
            return list.Any(x => x.PartHash == part.PartHash
                            && x.PartNumber == part.PartNumber
                            && x.TotalParts == part.TotalParts);
        }



        public async Task<Reply> DeleteByPackage(R2Package package, CancellationToken cancelTkn)
        {
            var list = await ListByPackage(package, cancelTkn);
            foreach (var item in list)
            {
                var reply = await _client.DeleteNode(item.nid, cancelTkn);
                if (reply.Failed) return reply;
            }
            return Reply.Success;
        }


        public async Task<string> DownloadToTemp(R2PackagePart part, CancellationToken cancelTkn)
        {
            var byts = await _client.GetBytes<PartContentsByHash1>(cancelTkn, part.PartHash);
            return _fileIO.WriteTempFile(byts);
        }
    }
}
