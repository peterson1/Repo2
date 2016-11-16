using System;
using System.Collections.Generic;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.DomainModels
{
    public class R2PackagePart
    {
        public string   PackageFilename  { get; set; }
        public string   PackageHash      { get; set; }
        public string   PartHash         { get; set; }
        public int      PartNumber       { get; set; }
        public int      TotalParts       { get; set; }


        public bool IsValid()
        {
            IEnumerable<string> errMsgs;
            return IsValid(out errMsgs);
        }


        public bool IsValid(out IEnumerable<string> validationErrors)
        {
            var typ = $"‹{GetType().Name}›";
            var ers = new List<string>();

            if (PackageFilename.IsBlank())
                ers.Add($"{typ}.{nameof(PackageFilename)} should not be blank.");

            if (PackageHash.IsBlank())
                ers.Add($"{typ}.{nameof(PackageHash)} should not be blank.");

            if (PartHash.IsBlank())
                ers.Add($"{typ}.{nameof(PartHash)} should not be blank.");

            if (PartNumber < 1)
                ers.Add($"{typ}.{nameof(PartNumber)} should be greater than zero; but found [{PartNumber}].");

            if (TotalParts < 1)
                ers.Add($"{typ}.{nameof(TotalParts)} should be greater than zero; but found [{TotalParts}].");

            if (PartNumber > TotalParts)
                ers.Add($"{typ}.{nameof(PartNumber)} [{PartNumber}] should NOT be greater than {nameof(TotalParts)} [{TotalParts}].");

            validationErrors = ers.Count == 0 ? null : ers;
            return validationErrors == null;            
        }
    }
}
