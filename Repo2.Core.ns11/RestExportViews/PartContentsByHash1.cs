using System.Collections.Generic;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.RestExportViews
{
    public class PartContentsByHash1 : R2PackagePart, IRestExportView
    {
        public string DisplayPath => "part-content-by-hash1";


        public List<string> CastArguments(object[] args)
        {
            var partHash = args[0]?.ToString();
            if (partHash.IsBlank()) throw Fault.BlankText("Part Hash");
            return new List<string> { partHash };
        }


        public void PostProcess(object obj)
        {
        }
    }
}
