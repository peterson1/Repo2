using FluentAssertions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.SDK.WPF45.FileSystems;
using Repo2.UnitTests.Lib.TestTools;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.SDK.WPF45.Tests.FileSystems
{
    [Trait("C:Temp", "write")]
    public class FileSystemAccesor1Facts
    {
        private IFileSystemAccesor _sut;

        public FileSystemAccesor1Facts()
        {
            _sut = new FileSystemAccesor1();
        }



        [Fact(DisplayName = "Write-Read Encrypted Json")]
        public void WriteReadEncryptedJson()
        {
            var orig = SampleClass1.Randomize();
            var path = _sut.GetTempFilePath();
            var pwd  = F.ke.Word.SHA1ForUTF8();

            _sut.EncryptJsonToFile(path, orig, pwd);

            var dcryptd = _sut.DecryptJsonFile<SampleClass1>(path, pwd);

            dcryptd.Number1.Should().Be(orig.Number1);
            dcryptd.Text1  .Should().Be(orig.Text1  );

            _sut.Delete(path);
        }


        class SampleClass1
        {
            public int     Number1  { get; set; }
            public string  Text1    { get; set; }


            public static SampleClass1 Randomize()
                => new SampleClass1
                {
                    Number1 = F.ke.Int(1, int.MaxValue),
                    Text1   = F.ke.Text,
                };
        }
    }
}
