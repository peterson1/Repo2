using System;
using FluentAssertions;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;
using Repo2.SDK.WPF45.RestClients;
using Repo2.Uploader.Lib45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.SDK.WPF45.Tests.RestClients
{
    [Trait("Local:453", "Write")]
    public class ResilientClient1CrudFacts
    {
        private IR2RestClient _sut;
        private R2Credentials _creds;

        public ResilientClient1CrudFacts()
        {
            _creds = LocalConfigFile.Parse(UploaderCfg.KEY);
            _sut   = new ResilientClient1();
        }


        [Fact(DisplayName = "Can POST & DELETE PkgPart")]
        public async void CanPostDeletePackagePart()
        {
            (await _sut.EnableWriteAccess(_creds)).Should().BeTrue();
            var sampl = SamplePkgPart();

            var list = await _sut.List<PartsByPkgHash1>(sampl);
            list.Should().HaveCount(0);

            var reply = await _sut.PostNode(sampl);
            reply.Should().NotBeNull();
            reply.IsSuccessful.Should().BeTrue();
            reply.Nid.Should().BeGreaterThan(1);

            list = await _sut.List<PartsByPkgHash1>(sampl);
            list.Should().HaveCount(1);

            var delRep = await _sut.DeleteNode(reply.Nid);
            delRep.IsSuccessful.Should().BeTrue();

            list = await _sut.List<PartsByPkgHash1>(sampl);
            list.Should().HaveCount(0);
        }


        [Fact(DisplayName = "Can PATCH Package")]
        public async void CanPatchPackage()
        {
            (await _sut.EnableWriteAccess(_creds)).Should().BeTrue();
            //var sampl = SamplePkgPart();

            //var nid = (await _sut.PostNode(sampl)).Nid;

            //list = await _sut.List<PartsByPkgHash1>(sampl);
            //list.Should().HaveCount(1);

            //var delRep = await _sut.DeleteNode(post.Nid);
            //delRep.IsSuccessful.Should().BeTrue();

            //list = await _sut.List<PartsByPkgHash1>(sampl);
            //list.Should().HaveCount(0);
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
