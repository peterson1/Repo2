using System.Diagnostics;
using System.IO;
using FluentAssertions;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.AcceptanceTests.Lib.TestTools
{
    public class TestClient
    {
        const string SUB_DIR = @"Repo2.TestClient.WPF45\bin\Debug";

        public const string Filename = "Repo2.TestClient.WPF45.exe";

        public static string GetPath()
        {
            var @base = Directory.GetParent(@"..\..");
            return Path.Combine(@base.FullName, SUB_DIR, Filename);
        }


        public static Process Run()
        {
            var exePath = TestClient.GetPath();
            File.Exists(exePath).Should().BeTrue($"exe should be in {L.f}{exePath}");
            return Process.Start(exePath);
        }
    }
}
