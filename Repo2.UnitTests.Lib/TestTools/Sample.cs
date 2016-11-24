using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Randomizers;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.UnitTests.Lib.TestTools
{
    internal class Sample
    {
        private static FakeFactory _fke = new FakeFactory();


        internal static PackagesByFilename1 Package()
            => new PackagesByFilename1
            {
                Filename  = _fke.FileName,
                FileFound = true,
                Hash      = _fke.Text,
            };
    }
}
