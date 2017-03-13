using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Drupal8.Attributes;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.Drupal8
{
    [Trait("Core", "Unit")]
    public class D8NodeMapperFacts
    {
        [Fact(DisplayName = "Maps string property")]
        public void MapsStringProperty()
        {
            var node = new TestClass { Text1 = "abc" };
            var mapd = D8NodeMapper.Cast(node, "");
            mapd.MustHave("field_text1", "value", node.Text1);
        }


        [Fact(DisplayName = "Maps date property")]
        public void MapsDateProperty()
        {
            var node = new TestClass { Date1 = 23.February(2016) };
            var mapd = D8NodeMapper.Cast(node, "");
            mapd.MustHave("field_date1", "value", node.Date1.ToString("yyyy-MM-dd H:mm:ss"));
        }


        [Fact(DisplayName = "Maps date? property - w/ value")]
        public void MapsNullableDateProperty()
        {
            var node = new TestClass { Date2 = 23.February(2016) };
            var mapd = D8NodeMapper.Cast(node, "");
            mapd.MustHave("field_date2", "value", node.Date2?.ToString("yyyy-MM-dd H:mm:ss"));
        }


        [Fact(DisplayName = "Maps date? property - null")]
        public void MapsNullableDateProperty_null()
        {
            var node = new TestClass { Date2 = null };
            var mapd = D8NodeMapper.Cast(node, "");
            mapd.MustHave("field_date2", "value", null);
        }
    }


    class TestClass : D8NodeBase
    {
        public override string D8TypeName => "test_class";

        [_("text1")] public string     Text1   { get; set; }
        [_("date1")] public DateTime   Date1   { get; set; }
        [_("date2")] public DateTime?  Date2   { get; set; }
    }
}
