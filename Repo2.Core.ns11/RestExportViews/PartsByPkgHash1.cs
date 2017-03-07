using System.Collections.Generic;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.RestExportViews
{
    public class PartsByPkgHash1 : R2PackagePart, IRestExportView
    {
        public string DisplayPath => "parts-by-pkg-hash1";


        public List<string> CastArguments(object[] args)
        {
            var pkgName = args[0]?.ToString();
            var pkgHash = args[1]?.ToString();

            if (pkgName.IsBlank()) throw Fault.BlankText("Package Name");
            if (pkgHash.IsBlank()) throw Fault.BlankText("Package Hash");

            return new List<string>{ pkgName, pkgHash };
        }


        public void PostProcess()
        {
        }
    }
}
