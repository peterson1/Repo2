using System.Collections.Generic;
using Repo2.SDK.WPF45.Configuration;

namespace Repo2.Uploader.Lib45.Configuration
{
    public class UploaderConfigFile : LocalConfigFile
    {
        protected override string  SubFolder => @"Repo2\Uploader";
        protected override string  Prefix    => "Uploader_";

        public static UploaderConfigFile Parse(string cfgKey)
            => new UploaderConfigFile()
                .Parse<UploaderConfigFile>(cfgKey);

        public static IEnumerable<string> GetKeys()
            => new UploaderConfigFile().GetLocalKeys();
    }
}
