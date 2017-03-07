using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.Drupal8
{
    public static class MappedNodeExtensions
    {
        public static void MustHave(this Dictionary<string, object> mapd,
            string rootKey, string nextKey, object expectdVal)
        {
            mapd.Should().ContainKey(rootKey);
            mapd[rootKey].Should().BeAssignableTo<List<Dictionary<string, object>>>();
            var list = mapd[rootKey] as List<Dictionary<string, object>>;
            list.Should().HaveCount(1);
            list[0].Should().BeAssignableTo<Dictionary<string, object>>();
            var dict = list[0] as Dictionary<string, object>;
            dict.Should().ContainKey(nextKey);
            dict[nextKey].Should().Be(expectdVal);
        }
    }
}
