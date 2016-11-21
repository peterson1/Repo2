using System.Collections.Generic;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;

namespace Repo2.Core.ns11.RestExportViews
{
    public class PartsByPkgHash1 : R2PackagePart, IRestExportView
    {
        public string DisplayPath => "parts-by-pkg-hash1";


        public List<string> CastArguments(object[] args)
        {
            var part = args[0] as R2PackagePart;
            if (part == null) throw Fault.BadCast<R2PackagePart>(args[0]);
            return new List<string>
            {
                part.PackageFilename,
                part.PackageHash
            };
        }
    }
}
