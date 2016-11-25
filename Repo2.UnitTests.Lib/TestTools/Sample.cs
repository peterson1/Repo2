using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Randomizers;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.UnitTests.Lib.TestTools
{
    public class Sample
    {
        //private static FakeFactory _fke = new FakeFactory();


        //public static PackagesByFilename1 LocalTestPkg1()
        //    => new PackagesByFilename1
        //    {
        //        Filename  = "Test_Package_1.pkg",
        //        FileFound = true,
        //        Hash      = _fke.Text,
        //    };

        public static PackagesByFilename1 Package()
            => new PackagesByFilename1
            {
                Filename  = F.ke.FileName,
                FileFound = true,
                Hash      = F.ke.Text,
            };
    }
}
