using System;
using FluentAssertions;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.RestClients;
using Repo2.Uploader.Lib45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.SDK.WPF45.Tests.RestClients
{
    [Trait("Local:453", "Write")]
    public class ResilientClient1Facts
    {
        private IR2RestClient _sut;
        private R2Credentials _creds;

        public ResilientClient1Facts()
        {
            _creds = LocalConfigFile.Parse(UploaderCfg.KEY);
            _sut   = new ResilientClient1();
        }


        [Fact(DisplayName = "Can POST PkgPart")]
        public async void CanPostPackagePart()
        {
            (await _sut.EnableWriteAccess(_creds)).Should().BeTrue();

            var dict = await _sut.PostNode(SamplePkgPart());
            dict.Should().NotBeNull();
        }


        private R2PackagePart SamplePkgPart()
        {
            var tix = DateTime.Now.Ticks.ToString();
            return new R2PackagePart
            {
                PackageFilename = "sample.pkg",
                PartHash        = "cdf456",
                PackageHash     = $"abc123_{tix}",
                PartNumber      = 1,
                TotalParts      = 2
            };
        }
    }
}
