using Moq;

namespace Repo2.UnitTests.Lib.TestTools.MoqExtensions
{
    public class Any
    {
        public static string Text => It.IsAny<string>();
    }
}
