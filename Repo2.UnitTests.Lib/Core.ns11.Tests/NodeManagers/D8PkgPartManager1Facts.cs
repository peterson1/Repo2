﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Moq;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;
using Repo2.SDK.WPF45.Serialization;
using Repo2.UnitTests.Lib.TestTools.MoqExtensions;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.NodeManagers
{
    [Trait("Core", "Unit")]
    public class D8PkgPartManager1Facts
    {
        [Fact(DisplayName = "Warning if already in server")]
        public async void WarningIfAlreadyInServer()
        {
            var part = SamplePkgPart();
            var moq  = MockClientReturning(part);
            var sut  = new D8PkgPartManager1(moq.Object, null);

            var rply = await sut.AddNode(part);

            rply.IsSuccessful.Should().BeTrue();
            rply.Errors  .Should().HaveCount(0);
            rply.Warnings.Should().HaveCount(1);

            moq.Verify(c => c.PostNode(Any.Node), Times.Never);
        }


        [Fact(DisplayName = "AddNode: Happy Path")]
        public async void AddNode_HappyPath()
        {
            var moq  = MockClientReturning();
            var part = SamplePkgPart();
            var sut  = new D8PkgPartManager1(moq.Object, null);

            var rply = await sut.AddNode(part);

            rply.IsSuccessful.Should().BeTrue();
            rply.Errors  .Should().HaveCount(0);
            rply.Warnings.Should().HaveCount(0);

            moq.Verify(c => c.PostNode(Any.Node), Times.Once);
        }


        private Mock<IR2RestClient> MockClientReturning(params PartsByPkgHash1[] parts)
        {
            var moq = new Mock<IR2RestClient>();

            moq.Setup(x => x.List<PartsByPkgHash1>(Any.Text, Any.Text))
               .ReturnsAsync(parts.ToList());

            var dict = new Dictionary<string, object>();
            dict.Add("nid", Json.Serialize(D8HALJson.ValueField(123)));

            moq.Setup(x => x.PostNode(Any.Node))
                .ReturnsAsync(new NodeReply(dict));

            return moq;
        }


        private PartsByPkgHash1 SamplePkgPart()
            => new PartsByPkgHash1
            {
                PackageFilename = "sample.pkg",
                PackageHash = "abc123",
                PartHash = "cdf456",
                PartNumber = 1,
                TotalParts = 2
            };
    }
}
