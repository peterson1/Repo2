﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.NodeManagers
{
    public class D8PkgPartManager1 : IPackagePartManager
    {
        private IR2RestClient _client;

        public D8PkgPartManager1(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }


        public async Task<Reply> AddNode (R2PackagePart pkgPart)
        {
            var errors   = new List<string>();
            var warnings = new List<string>();

            if (!pkgPart.IsValid(ref errors))
                goto ReturnReply;

            if (await AlreadyInServer(pkgPart))
            {
                warnings.Add($"“{pkgPart.PackageFilename}” part {pkgPart.PartNumber} of {pkgPart.TotalParts} already exists in the server.");
                goto ReturnReply;
            }

            var dict = await _client.PostNode(pkgPart);

            //todo: add status and result to returned reply

            ReturnReply:
                return new Reply
                {
                    Errors   = errors,
                    Warnings = warnings
                };
        }


        private async Task<bool> AlreadyInServer(R2PackagePart part)
        {
            var list = await _client.List<PartsByPkgHash1>(part);
            return list.Any(x => x.PartHash == part.PartHash
                            && x.PartNumber == part.PartNumber
                            && x.TotalParts == part.TotalParts);
        }
    }
}
