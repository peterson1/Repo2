using System.Threading;
using Moq;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Drupal8;

namespace Repo2.UnitTests.Lib.TestTools.MoqExtensions
{
    public class Any
    {
        public static string            Text => It.IsAny<string>();
        public static object            Obj  => It.IsAny<object>();
        public static D8NodeBase        Node => It.IsAny<D8NodeBase>();
        public static R2Package         Pkg  => It.IsAny<R2Package>();
        public static CancellationToken Tkn  => It.IsAny<CancellationToken>();
    }
}
