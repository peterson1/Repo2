﻿using System;
using Autofac;
using FluentAssertions;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.Randomizers;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;
using Repo2.SDK.WPF45.RestClients;
using Repo2.Uploader.Lib45.Components;
using Repo2.Uploader.Lib45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.SDK.WPF45.Tests.RestClients
{
    [Trait("Local:453", "Write")]
    public class ResilientClient1CrudFacts
    {
        private IR2RestClient       _sut;
        private R2Credentials       _creds;
        private FakeFactory         _fke;
        private IPackageManager     _pkgs;
        private IPackagePartManager _parts;

        public ResilientClient1CrudFacts()
        {
            _fke   = new FakeFactory();
            _creds = LocalConfigFile.Parse(UploaderCfg.KEY);
            //_sut   = new ResilientClient1();
            using (var scope = Registry.Build().BeginLifetimeScope())
            {
                _pkgs  = scope.Resolve<IPackageManager>();
                _parts = scope.Resolve<IPackagePartManager>();
                _sut   = scope.Resolve<IR2RestClient>();
            }
        }


        [Fact(DisplayName = "Can POST & DELETE PkgPart")]
        public async void CanPostDeletePackagePart()
        {
            (await _sut.EnableWriteAccess(_creds)).Should().BeTrue();
            var sampl = SamplePkgPart();

            var list = await _parts.ListByPkgHash(sampl);
            list.Should().HaveCount(0);

            var reply = await _sut.PostNode(sampl);
            reply.Should().NotBeNull();
            reply.IsSuccessful.Should().BeTrue();
            reply.Nid.Should().BeGreaterThan(1);

            list = await _parts.ListByPkgHash(sampl);
            list.Should().HaveCount(1);

            var delRep = await _sut.DeleteNode(reply.Nid);
            delRep.IsSuccessful.Should().BeTrue();

            list = await _parts.ListByPkgHash(sampl);
            list.Should().HaveCount(0);
        }


        [Fact(DisplayName = "Can PATCH Package")]
        public async void CanPatchPackage()
        {
            (await _sut.EnableWriteAccess(_creds)).Should().BeTrue();
            var pkg = UpdatedTestPackage2();

            var list = await _pkgs.ListByFilename(pkg);
            list.Should().HaveCount(1);
            list[0].Hash.Should().NotBe(pkg.Hash);

            var reply = await _sut.PatchNode(pkg);
            reply.IsSuccessful.Should().BeTrue();

            list = await _pkgs.ListByFilename(pkg);
            list.Should().HaveCount(1);
            list[0].Filename.Should().Be(pkg.Filename);
            list[0].Hash.Should().Be(pkg.Hash);
        }


        [Fact(DisplayName = "PATCH Package requires hash")]
        public async void PatchPackageRequiresHash()
        {
            (await _sut.EnableWriteAccess(_creds)).Should().BeTrue();
            var pkg  = UpdatedTestPackage2();
            pkg.Hash = null;

            var reply = await _sut.PatchNode(pkg);
            reply.IsSuccessful.Should().BeFalse();
        }


        private R2Package UpdatedTestPackage2() => new R2Package
        {
            nid      = 2,
            Filename = "Test_Package_2.pkg",
            Hash     = _fke.Word
        };

        private R2PackagePart SamplePkgPart()
        {
            var tix = DateTime.Now.Ticks.ToString();
            return new R2PackagePart
            {
                PackageFilename = "sample.pkg",
                PartHash        = "cdf456",
                PackageHash     = $"abc123_{tix}",
                PartNumber      = 1,
                TotalParts      = 2,
                Base64Content   = "abc"
            };
        }
    }
}
