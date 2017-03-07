using System;
using System.Collections.Generic;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.RestExportViews
{
    public class PartsByPkgName1 : R2PackagePart, IRestExportView
    {
        public string DisplayPath => "parts-by-pkg-name1";


        public List<string> CastArguments(object[] args)
        {
            var pkgName = args[0]?.ToString();

            if (pkgName.IsBlank()) throw Fault.BlankText("Package Name");

            return new List<string> { pkgName };
        }


        public void PostProcess()
        {
        }
    }
}
