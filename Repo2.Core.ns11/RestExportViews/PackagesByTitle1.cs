using System.Collections.Generic;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;

namespace Repo2.Core.ns11.RestExportViews
{
    public class PackagesByTitle1 : R2Package, IRestExportView
    {
        public string DisplayPath => "package-checker-1";


        public List<string> CastArguments(object[] args)
        {
            var pkg = args[0] as R2Package;
            if (pkg == null) throw Fault.BadCast<R2Package>(args[0]);
            return new List<string> { pkg.Filename };
        }
    }
}
