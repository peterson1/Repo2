﻿using System;
using System.Threading;
using Autofac;
using FluentAssertions;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.RestClients;
using Repo2.Uploader.Lib45.Components;
using Repo2.Uploader.Lib45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.PackageUploaderSuite.PreUploadCheckerTests
{
    [Trait("Local:453", "Read")]
    public class PreUploadCheckerFacts
    {
        private IR2RestClient       _client;
        private IR2PreUploadChecker _sut;

        public PreUploadCheckerFacts()
        {
            using (var scope = UploaderIoC.BeginScope())
            {
                _client = scope.Resolve<IR2RestClient>();
                _sut    = scope.Resolve<IR2PreUploadChecker>();
            }
            var cfg = UploaderConfigFile.Parse(UploaderCfg.Localhost);
            _client.SetCredentials(cfg);
        }


        [Fact(DisplayName = "Uploadable if registered")]
        public async void Registered()
        {
            var pkg       = new R2Package("Test_Package_1.pkg");
            pkg.FileFound = true;
            pkg.Hash      = DateTime.Now.Ticks.ToString();
            var actual    = await _sut.IsUploadable(pkg, new CancellationToken());
            actual.Should().BeTrue(_sut.ReasonWhyNot);
        }


        [Fact(DisplayName = "Not registered: NO Upload")]
        public async void NotRegistered()
        {
            var pkg    = new R2Package("non-registered.pkg");
            var actual = await _sut.IsUploadable(pkg, new CancellationToken());
            actual.Should().BeFalse();
        }


        [Fact(DisplayName = "Same Hash: NO Upload")]
        public async void SameHash()
        {
            var pkg    = new R2Package("non-registered.pkg");
            var actual = await _sut.IsUploadable(pkg, new CancellationToken());
            actual.Should().BeFalse();
        }
    }
}
